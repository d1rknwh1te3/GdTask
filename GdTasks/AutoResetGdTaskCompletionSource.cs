using GdTasks.Interfaces.Promises;
using System.Diagnostics;

namespace GdTasks;

public partial class AutoResetGdTaskCompletionSource<T> : IGdTaskSource<T>, ITaskPoolNode<AutoResetGdTaskCompletionSource<T>>, IPromise<T>
{
	private static TaskPool<AutoResetGdTaskCompletionSource<T>> _pool;
	private GdTaskCompletionSourceCore<T> _core;
	private AutoResetGdTaskCompletionSource<T> _nextNode;
	static AutoResetGdTaskCompletionSource()
		=> TaskPool.RegisterSizeGetter(typeof(AutoResetGdTaskCompletionSource<T>), () => _pool.Size);

	private AutoResetGdTaskCompletionSource()
	{ }

	public ref AutoResetGdTaskCompletionSource<T> NextNode => ref _nextNode;
	public GdTask<T> Task
	{
		[DebuggerHidden]
		get => new(this, _core.Version);
	}

	[DebuggerHidden]
	public static AutoResetGdTaskCompletionSource<T> Create()
	{
		if (!_pool.TryPop(out var result))
			result = new AutoResetGdTaskCompletionSource<T>();

		TaskTracker.TrackActiveTask(result, 2);
		return result;
	}

	[DebuggerHidden]
	public static AutoResetGdTaskCompletionSource<T> CreateFromCanceled(CancellationToken cancellationToken, out short token)
	{
		var source = Create();
		source.TrySetCanceled(cancellationToken);
		token = source._core.Version;
		return source;
	}

	[DebuggerHidden]
	public static AutoResetGdTaskCompletionSource<T> CreateFromException(Exception exception, out short token)
	{
		var source = Create();
		source.TrySetException(exception);
		token = source._core.Version;
		return source;
	}

	[DebuggerHidden]
	public static AutoResetGdTaskCompletionSource<T> CreateFromResult(T result, out short token)
	{
		var source = Create();
		source.TrySetResult(result);
		token = source._core.Version;
		return source;
	}
	[DebuggerHidden]
	public T GetResult(short token)
	{
		try
		{
			return _core.GetResult(token);
		}
		finally
		{
			TryReturn();
		}
	}

	[DebuggerHidden]
	void IGdTaskSource.GetResult(short token) => GetResult(token);

	[DebuggerHidden]
	public GdTaskStatus GetStatus(short token) => _core.GetStatus(token);

	[DebuggerHidden]
	public void OnCompleted(Action<object> continuation, object state, short token) => _core.OnCompleted(continuation, state, token);

	[DebuggerHidden]
	public bool TrySetCanceled(CancellationToken cancellationToken = default) => _core.TrySetCanceled(cancellationToken);

	[DebuggerHidden]
	public bool TrySetException(Exception exception) => _core.TrySetException(exception);

	[DebuggerHidden]
	public bool TrySetResult(T result) => _core.TrySetResult(result);
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