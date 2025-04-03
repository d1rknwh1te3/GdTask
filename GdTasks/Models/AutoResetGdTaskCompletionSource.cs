using GdTasks.Interfaces.Promises;
using System.Diagnostics;

namespace GdTasks.Models;

public partial class AutoResetGdTaskCompletionSource : IGdTaskSource, ITaskPoolNode<AutoResetGdTaskCompletionSource>, IPromise
{
	private static TaskPool<AutoResetGdTaskCompletionSource> _pool;
	private GdTaskCompletionSourceCore<AsyncUnit> _core;
	private AutoResetGdTaskCompletionSource _nextNode;
	static AutoResetGdTaskCompletionSource() => TaskPool.RegisterSizeGetter(typeof(AutoResetGdTaskCompletionSource), () => _pool.Size);

	private AutoResetGdTaskCompletionSource()
	{ }

	public ref AutoResetGdTaskCompletionSource NextNode => ref _nextNode;
	public GdTask Task
	{
		[DebuggerHidden]
		get => new(this, _core.Version);
	}

	[DebuggerHidden]
	public static AutoResetGdTaskCompletionSource Create()
	{
		if (!_pool.TryPop(out var result))
			result = new AutoResetGdTaskCompletionSource();

		TaskTracker.TrackActiveTask(result, 2);
		return result;
	}

	[DebuggerHidden]
	public static AutoResetGdTaskCompletionSource CreateCompleted(out short token)
	{
		var source = Create();
		source.TrySetResult();
		token = source._core.Version;
		return source;
	}

	[DebuggerHidden]
	public static AutoResetGdTaskCompletionSource CreateFromCanceled(CancellationToken cancellationToken, out short token)
	{
		var source = Create();
		source.TrySetCanceled(cancellationToken);
		token = source._core.Version;
		return source;
	}

	[DebuggerHidden]
	public static AutoResetGdTaskCompletionSource CreateFromException(Exception exception, out short token)
	{
		var source = Create();
		source.TrySetException(exception);
		token = source._core.Version;
		return source;
	}
	[DebuggerHidden]
	public void GetResult(short token)
	{
		try
		{
			_core.GetResult(token);
		}
		finally
		{
			TryReturn();
		}
	}

	[DebuggerHidden]
	public GdTaskStatus GetStatus(short token) => _core.GetStatus(token);

	[DebuggerHidden]
	public void OnCompleted(Action<object> continuation, object state, short token) => _core.OnCompleted(continuation, state, token);

	[DebuggerHidden]
	public bool TrySetCanceled(CancellationToken cancellationToken = default) => _core.TrySetCanceled(cancellationToken);

	[DebuggerHidden]
	public bool TrySetException(Exception exception) => _core.TrySetException(exception);

	[DebuggerHidden]
	public bool TrySetResult() => _core.TrySetResult(AsyncUnit.Default);
	[DebuggerHidden]
	public GdTaskStatus UnsafeGetStatus() => _core.UnsafeGetStatus();
	[DebuggerHidden]
	private bool TryReturn()
	{
		TaskTracker.RemoveTracking(this);
		_core.Reset();

		return _pool.TryPush(this);
	}
}