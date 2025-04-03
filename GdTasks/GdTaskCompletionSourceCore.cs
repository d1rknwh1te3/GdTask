using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;

namespace GdTasks;

[StructLayout(LayoutKind.Auto)]
public struct GdTaskCompletionSourceCore<TResult>
{
	// Struct Size: TResult + (8 + 2 + 1 + 1 + 8 + 8)

	private TResult result;
	private object error; // ExceptionHolder or OperationCanceledException
	private bool hasUnhandledError;
	private int completedCount; // 0: completed == false
	private Action<object> continuation;
	private object continuationState;

	[DebuggerHidden]
	public void Reset()
	{
		ReportUnhandledError();

		unchecked
		{
			Version += 1; // incr version.
		}
		completedCount = 0;
		result = default;
		error = null;
		hasUnhandledError = false;
		continuation = null;
		continuationState = null;
	}

	private void ReportUnhandledError()
	{
		if (!hasUnhandledError)
			return;

		try
		{
			switch (error)
			{
				case OperationCanceledException oc:
					GdTaskScheduler.PublishUnobservedTaskException(oc);
					break;

				case ExceptionHolder e:
					GdTaskScheduler.PublishUnobservedTaskException(e.GetException().SourceException);
					break;
			}
		}
		catch { }
	}

	internal void MarkHandled()
	{
		hasUnhandledError = false;
	}

	/// <summary>Completes with a successful result.</summary>
	/// <param name="result">The result.</param>
	[DebuggerHidden]
	public bool TrySetResult(TResult result)
	{
		if (Interlocked.Increment(ref completedCount) != 1)
			return false;

		// setup result
		this.result = result;

		if (continuation == null && Interlocked.CompareExchange(ref continuation, GdTaskCompletionSourceCoreShared.SSentinel, null) == null)
			return false;

		continuation(continuationState);

		return true;
	}

	/// <summary>Completes with an error.</summary>
	/// <param name="error">The exception.</param>
	[DebuggerHidden]
	public bool TrySetException(Exception error)
	{
		if (Interlocked.Increment(ref completedCount) != 1)
			return false;

		// setup result
		hasUnhandledError = true;

		this.error = error is OperationCanceledException
			? error
			: new ExceptionHolder(ExceptionDispatchInfo.Capture(error));

		if (continuation == null && Interlocked.CompareExchange(ref continuation, GdTaskCompletionSourceCoreShared.SSentinel, null) == null)
			return false;

		continuation(continuationState);

		return true;
	}

	[DebuggerHidden]
	public bool TrySetCanceled(CancellationToken cancellationToken = default)
	{
		if (Interlocked.Increment(ref completedCount) != 1)
			return false;

		// setup result
		hasUnhandledError = true;
		error = new OperationCanceledException(cancellationToken);

		if (continuation == null && Interlocked.CompareExchange(ref continuation, GdTaskCompletionSourceCoreShared.SSentinel, null) == null)
			return false;

		continuation(continuationState);

		return true;
	}

	/// <summary>Gets the operation version.</summary>
	[DebuggerHidden]
	public short Version { get; private set; }

	/// <summary>Gets the status of the operation.</summary>
	/// <param name="token">Opaque value that was provided to the <see cref="GdTasks.GdTask"/>'s constructor.</param>
	[DebuggerHidden]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public GdTaskStatus GetStatus(short token)
	{
		ValidateToken(token);
		return (continuation == null || (completedCount == 0)) ? GdTaskStatus.Pending
			: (error == null) ? GdTaskStatus.Succeeded
			: (error is OperationCanceledException) ? GdTaskStatus.Canceled
			: GdTaskStatus.Faulted;
	}

	/// <summary>Gets the status of the operation without token validation.</summary>
	[DebuggerHidden]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public GdTaskStatus UnsafeGetStatus()
	{
		return (continuation == null || (completedCount == 0)) ? GdTaskStatus.Pending
			: (error == null) ? GdTaskStatus.Succeeded
			: (error is OperationCanceledException) ? GdTaskStatus.Canceled
			: GdTaskStatus.Faulted;
	}

	/// <summary>Gets the result of the operation.</summary>
	/// <param name="token">Opaque value that was provided to the <see cref="GdTasks.GdTask"/>'s constructor.</param>
	// [StackTraceHidden]
	[DebuggerHidden]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public TResult GetResult(short token)
	{
		ValidateToken(token);
		if (completedCount == 0)
			throw new InvalidOperationException("Not yet completed, GDTask only allow to use await.");

		if (error == null)
			return result;

		hasUnhandledError = false;

		switch (error)
		{
			case OperationCanceledException oce:
				throw oce;
			case ExceptionHolder eh:
				eh.GetException().Throw();
				break;
		}

		throw new InvalidOperationException("Critical: invalid exception type was held.");
	}

	/// <summary>Schedules the continuation action for this operation.</summary>
	/// <param name="continuation">The continuation to invoke when the operation has completed.</param>
	/// <param name="state">The state object to pass to <paramref name="continuation"/> when it's invoked.</param>
	/// <param name="token">Opaque value that was provided to the <see cref="GdTasks.GdTask"/>'s constructor.</param>
	[DebuggerHidden]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void OnCompleted(Action<object> continuation, object state, short token /*, ValueTaskSourceOnCompletedFlags flags */)
	{
		if (continuation == null)
			throw new ArgumentNullException(nameof(continuation));

		ValidateToken(token);

		/* no use ValueTaskSourceOnCOmpletedFlags, always no capture ExecutionContext and SynchronizationContext. */

		/*
		    PatternA: GetStatus=Pending => OnCompleted => TrySet*** => GetResult
		    PatternB: TrySet*** => GetStatus=!Pending => GetResult
		    PatternC: GetStatus=Pending => TrySet/OnCompleted(race condition) => GetResult
		    C.1: win OnCompleted -> TrySet invoke saved continuation
		    C.2: win TrySet -> should invoke continuation here.
		*/

		// not set continuation yet.
		object oldContinuation = this.continuation;

		if (oldContinuation == null)
		{
			continuationState = state;
			oldContinuation = Interlocked.CompareExchange(ref this.continuation, continuation, null);
		}

		if (oldContinuation == null)
			return;

		// already running continuation in TrySet.
		// It will cause call OnCompleted multiple time, invalid.
		if (!ReferenceEquals(oldContinuation, GdTaskCompletionSourceCoreShared.SSentinel))
			throw new InvalidOperationException("Already continuation registered, can not await twice or get Status after await.");

		continuation(state);
	}

	[DebuggerHidden]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void ValidateToken(short token)
	{
		if (token != Version)
			throw new InvalidOperationException("Token version is not matched, can not await twice or get Status after await.");
	}
}