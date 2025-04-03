using GdTasks.Interfaces.Promises;
using System.Diagnostics;
using System.Runtime.ExceptionServices;

namespace GdTasks;

public partial class GdTaskCompletionSource : IGdTaskSource, IPromise
{
	private CancellationToken _cancellationToken;
	private ExceptionHolder _exception;
	private object _gate;
	private bool _handled;
	private int _intStatus;
	private List<(Action<object>, object)> _secondaryContinuationList;
	private Action<object> _singleContinuation;
	private object _singleState;
	 // GDTaskStatus
	public GdTaskCompletionSource() => TaskTracker.TrackActiveTask(this, 2);

	public GdTask Task
	{
		[DebuggerHidden]
		get => new(this, 0);
	}

	[DebuggerHidden]
	public void GetResult(short token)
	{
		MarkHandled();

		var status = (GdTaskStatus)_intStatus;
		switch (status)
		{
			case GdTaskStatus.Succeeded:
				return;

			case GdTaskStatus.Faulted:
				_exception.GetException().Throw();
				return;

			case GdTaskStatus.Canceled:
				throw new OperationCanceledException(_cancellationToken);
			default:
			case GdTaskStatus.Pending:
				throw new InvalidOperationException("not yet completed.");
		}
	}

	[DebuggerHidden]
	public GdTaskStatus GetStatus(short token) => (GdTaskStatus)_intStatus;

	[DebuggerHidden]
	public void OnCompleted(Action<object> continuation, object state, short token)
	{
		if (_gate == null)
			Interlocked.CompareExchange(ref _gate, new object(), null);

		var lockGate = Thread.VolatileRead(ref _gate);

		lock (lockGate) // wait TrySignalCompletion, after status is not pending.
		{
			if ((GdTaskStatus)_intStatus != GdTaskStatus.Pending)
			{
				continuation(state);
				return;
			}

			if (_singleContinuation == null)
			{
				_singleContinuation = continuation;
				_singleState = state;
			}
			else
			{
				_secondaryContinuationList ??= new List<(Action<object>, object)>();
				_secondaryContinuationList.Add((continuation, state));
			}
		}
	}

	[DebuggerHidden]
	public bool TrySetCanceled(CancellationToken cancellationToken = default)
	{
		if (UnsafeGetStatus() != GdTaskStatus.Pending) return false;

		_cancellationToken = cancellationToken;
		return TrySignalCompletion(GdTaskStatus.Canceled);
	}

	[DebuggerHidden]
	public bool TrySetException(Exception exception)
	{
		if (exception is OperationCanceledException oce)
			return TrySetCanceled(oce.CancellationToken);

		if (UnsafeGetStatus() != GdTaskStatus.Pending) return false;

		_exception = new ExceptionHolder(ExceptionDispatchInfo.Capture(exception));

		return TrySignalCompletion(GdTaskStatus.Faulted);
	}

	[DebuggerHidden]
	public bool TrySetResult()
	{
		return TrySignalCompletion(GdTaskStatus.Succeeded);
	}

	[DebuggerHidden]
	public GdTaskStatus UnsafeGetStatus() => (GdTaskStatus)_intStatus;

	[DebuggerHidden]
	internal void MarkHandled()
	{
		if (_handled)
			return;

		_handled = true;
		TaskTracker.RemoveTracking(this);
	}
	[DebuggerHidden]
	private bool TrySignalCompletion(GdTaskStatus status)
	{
		if (Interlocked.CompareExchange(ref _intStatus, (int)status, (int)GdTaskStatus.Pending) != (int)GdTaskStatus.Pending)
			return false;

		if (_gate == null)
			Interlocked.CompareExchange(ref _gate, new object(), null);

		var lockGate = Thread.VolatileRead(ref _gate);
		lock (lockGate) // wait OnCompleted.
		{
			if (_singleContinuation != null)
			{
				try
				{
					_singleContinuation(_singleState);
				}
				catch (Exception ex)
				{
					GdTaskScheduler.PublishUnobservedTaskException(ex);
				}
			}

			if (_secondaryContinuationList != null)
			{
				foreach (var (c, state) in _secondaryContinuationList)
				{
					try
					{
						c(state);
					}
					catch (Exception ex)
					{
						GdTaskScheduler.PublishUnobservedTaskException(ex);
					}
				}
			}

			_singleContinuation = null;
			_singleState = null;
			_secondaryContinuationList = null;
		}
		return true;
	}
}

public partial class GdTaskCompletionSource<T> : IGdTaskSource<T>, IPromise<T>
{
	private CancellationToken _cancellationToken;
	private ExceptionHolder _exception;
	private object _gate;
	private bool _handled;
	private int _intStatus;
	private T _result;
	private List<(Action<object>, object)> _secondaryContinuationList;
	private Action<object> _singleContinuation;
	private object _singleState;
	 // GDTaskStatus
	public GdTaskCompletionSource()
	{
		TaskTracker.TrackActiveTask(this, 2);
	}

	public GdTask<T> Task
	{
		[DebuggerHidden]
		get => new(this, 0);
	}

	[DebuggerHidden]
	public T GetResult(short token)
	{
		MarkHandled();

		var status = (GdTaskStatus)_intStatus;
		switch (status)
		{
			case GdTaskStatus.Succeeded:
				return _result;

			case GdTaskStatus.Faulted:
				_exception.GetException().Throw();
				return default;

			case GdTaskStatus.Canceled:
				throw new OperationCanceledException(_cancellationToken);
			default:
			case GdTaskStatus.Pending:
				throw new InvalidOperationException("not yet completed.");
		}
	}

	[DebuggerHidden]
	void IGdTaskSource.GetResult(short token) => GetResult(token);

	[DebuggerHidden]
	public GdTaskStatus GetStatus(short token) => (GdTaskStatus)_intStatus;

	[DebuggerHidden]
	public void OnCompleted(Action<object> continuation, object state, short token)
	{
		if (_gate == null)
			Interlocked.CompareExchange(ref _gate, new object(), null);

		var lockGate = Thread.VolatileRead(ref _gate);

		lock (lockGate) // wait TrySignalCompletion, after status is not pending.
		{
			if ((GdTaskStatus)_intStatus != GdTaskStatus.Pending)
			{
				continuation(state);
				return;
			}

			if (_singleContinuation == null)
			{
				_singleContinuation = continuation;
				_singleState = state;
			}
			else
			{
				_secondaryContinuationList ??= new List<(Action<object>, object)>();
				_secondaryContinuationList.Add((continuation, state));
			}
		}
	}

	[DebuggerHidden]
	public bool TrySetCanceled(CancellationToken cancellationToken = default)
	{
		if (UnsafeGetStatus() != GdTaskStatus.Pending) return false;

		_cancellationToken = cancellationToken;
		return TrySignalCompletion(GdTaskStatus.Canceled);
	}

	[DebuggerHidden]
	public bool TrySetException(Exception exception)
	{
		if (exception is OperationCanceledException oce)
			return TrySetCanceled(oce.CancellationToken);

		if (UnsafeGetStatus() != GdTaskStatus.Pending)
			return false;

		_exception = new ExceptionHolder(ExceptionDispatchInfo.Capture(exception));
		return TrySignalCompletion(GdTaskStatus.Faulted);
	}

	[DebuggerHidden]
	public bool TrySetResult(T result)
	{
		if (UnsafeGetStatus() != GdTaskStatus.Pending) return false;

		_result = result;
		return TrySignalCompletion(GdTaskStatus.Succeeded);
	}

	[DebuggerHidden]
	public GdTaskStatus UnsafeGetStatus() => (GdTaskStatus)_intStatus;

	[DebuggerHidden]
	internal void MarkHandled()
	{
		if (_handled)
			return;

		_handled = true;
		TaskTracker.RemoveTracking(this);
	}
	[DebuggerHidden]
	private bool TrySignalCompletion(GdTaskStatus status)
	{
		if (Interlocked.CompareExchange(ref _intStatus, (int)status, (int)GdTaskStatus.Pending) != (int)GdTaskStatus.Pending)
			return false;

		if (_gate == null)
			Interlocked.CompareExchange(ref _gate, new object(), null);

		var lockGate = Thread.VolatileRead(ref _gate);

		lock (lockGate) // wait OnCompleted.
		{
			if (_singleContinuation != null)
			{
				try
				{
					_singleContinuation(_singleState);
				}
				catch (Exception ex)
				{
					GdTaskScheduler.PublishUnobservedTaskException(ex);
				}
			}

			if (_secondaryContinuationList != null)
			{
				foreach (var (c, state) in _secondaryContinuationList)
				{
					try
					{
						c(state);
					}
					catch (Exception ex)
					{
						GdTaskScheduler.PublishUnobservedTaskException(ex);
					}
				}
			}

			_singleContinuation = null;
			_singleState = null;
			_secondaryContinuationList = null;
		}
		return true;
	}
}