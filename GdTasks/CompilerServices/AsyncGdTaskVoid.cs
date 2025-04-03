using GdTasks.Interfaces.States;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace GdTasks.CompilerServices;

internal sealed class AsyncGdTaskVoid<TStateMachine> : IStateMachineRunner, ITaskPoolNode<AsyncGdTaskVoid<TStateMachine>>, IGdTaskSource
	where TStateMachine : IAsyncStateMachine
{
	private static TaskPool<AsyncGdTaskVoid<TStateMachine>> _pool;

	private AsyncGdTaskVoid<TStateMachine> _nextNode;

	private TStateMachine _stateMachine;

	static AsyncGdTaskVoid()
	{
		TaskPool.RegisterSizeGetter(typeof(AsyncGdTaskVoid<TStateMachine>), () => _pool.Size);
	}

	public AsyncGdTaskVoid()
	{
		MoveNext = Run;
	}
	public Action MoveNext { get; }
	public ref AsyncGdTaskVoid<TStateMachine> NextNode => ref _nextNode;
	public Action ReturnAction { get; }
	public static void SetStateMachine(ref TStateMachine stateMachine, ref IStateMachineRunner runnerFieldRef)
	{
		if (!_pool.TryPop(out var result))
			result = new AsyncGdTaskVoid<TStateMachine>();

		TaskTracker.TrackActiveTask(result, 3);

		runnerFieldRef = result; // set runner before copied.
		result._stateMachine = stateMachine; // copy struct StateMachine(in release build).
	}

	void IGdTaskSource.GetResult(short token)
	{
	}

	GdTaskStatus IGdTaskSource.GetStatus(short token) => GdTaskStatus.Pending;

	void IGdTaskSource.OnCompleted(Action<object> continuation, object state, short token)
	{
	}

	public void Return()
	{
		TaskTracker.RemoveTracking(this);
		_stateMachine = default;
		_pool.TryPush(this);
	}

	// dummy interface implementation for TaskTracker.
	GdTaskStatus IGdTaskSource.UnsafeGetStatus() => GdTaskStatus.Pending;

	[DebuggerHidden]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Run() => _stateMachine.MoveNext();
}