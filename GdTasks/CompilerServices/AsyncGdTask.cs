using GdTasks.Interfaces.States;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace GdTasks.CompilerServices;

internal sealed class AsyncGdTask<TStateMachine> : IStateMachineRunnerPromise, IGdTaskSource, ITaskPoolNode<AsyncGdTask<TStateMachine>>
	where TStateMachine : IAsyncStateMachine
{
	private static TaskPool<AsyncGdTask<TStateMachine>> _pool;

	private GdTaskCompletionSourceCore<AsyncUnit> _core;

	private AsyncGdTask<TStateMachine> _nextNode;

	private TStateMachine _stateMachine;

	static AsyncGdTask()
	{
		TaskPool.RegisterSizeGetter(typeof(AsyncGdTask<TStateMachine>), () => _pool.Size);
	}

	private AsyncGdTask()
	{
		MoveNext = Run;
	}
	public Action MoveNext { get; }
	public ref AsyncGdTask<TStateMachine> NextNode => ref _nextNode;
	public GdTask Task
	{
		[DebuggerHidden]
		get => new(this, _core.Version);
	}

	public static void SetStateMachine(ref TStateMachine stateMachine, ref IStateMachineRunnerPromise runnerPromiseFieldRef)
	{
		if (!_pool.TryPop(out var result))
			result = new AsyncGdTask<TStateMachine>();

		TaskTracker.TrackActiveTask(result, 3);

		runnerPromiseFieldRef = result; // set runner before copied.
		result._stateMachine = stateMachine; // copy struct StateMachine(in release build).
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
	public void SetException(Exception exception) => _core.TrySetException(exception);

	[DebuggerHidden]
	public void SetResult() => _core.TrySetResult(AsyncUnit.Default);

	[DebuggerHidden]
	public GdTaskStatus UnsafeGetStatus() => _core.UnsafeGetStatus();

	private void Return()
	{
		TaskTracker.RemoveTracking(this);
		_core.Reset();
		_stateMachine = default;
		_pool.TryPush(this);
	}

	[DebuggerHidden]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Run() => _stateMachine.MoveNext();

	private bool TryReturn()
	{
		TaskTracker.RemoveTracking(this);
		_core.Reset();
		_stateMachine = default;
		return _pool.TryPush(this);
	}
}