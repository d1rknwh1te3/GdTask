#pragma warning disable CS1591

using GdTasks.Interfaces.States;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace GdTasks.CompilerServices;

internal sealed class AsyncGdTask<TStateMachine, T> : IStateMachineRunnerPromise<T>, IGdTaskSource<T>, ITaskPoolNode<AsyncGdTask<TStateMachine, T>>
	where TStateMachine : IAsyncStateMachine
{
	private AsyncGdTask()
	{
		MoveNext = Run;
	}

	static AsyncGdTask()
	{
		TaskPool.RegisterSizeGetter(typeof(AsyncGdTask<TStateMachine, T>), () => _pool.Size);
	}

	public Action MoveNext { get; }

	public ref AsyncGdTask<TStateMachine, T> NextNode => ref _nextNode;

	private static TaskPool<AsyncGdTask<TStateMachine, T>> _pool;

	private AsyncGdTask<TStateMachine, T> _nextNode;

	private TStateMachine _stateMachine;

	private GdTaskCompletionSourceCore<T> _core;

	public static void SetStateMachine(ref TStateMachine stateMachine, ref IStateMachineRunnerPromise<T> runnerPromiseFieldRef)
	{
		if (!_pool.TryPop(out var result))
			result = new AsyncGdTask<TStateMachine, T>();

		TaskTracker.TrackActiveTask(result, 3);

		runnerPromiseFieldRef = result; // set runner before copied.
		result._stateMachine = stateMachine; // copy struct StateMachine(in release build).
	}

	private void Return()
	{
		TaskTracker.RemoveTracking(this);
		_core.Reset();
		_stateMachine = default;
		_pool.TryPush(this);
	}

	private bool TryReturn()
	{
		TaskTracker.RemoveTracking(this);
		_core.Reset();
		_stateMachine = default;
		return _pool.TryPush(this);
	}

	[DebuggerHidden]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Run()
	{
		_stateMachine.MoveNext();
	}

	public GdTask<T> Task
	{
		[DebuggerHidden]
		get
		{
			return new GdTask<T>(this, _core.Version);
		}
	}

	[DebuggerHidden]
	public void SetResult(T result)
	{
		_core.TrySetResult(result);
	}

	[DebuggerHidden]
	public void SetException(Exception exception)
	{
		_core.TrySetException(exception);
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
	void IGdTaskSource.GetResult(short token)
	{
		GetResult(token);
	}

	[DebuggerHidden]
	public GdTaskStatus GetStatus(short token)
	{
		return _core.GetStatus(token);
	}

	[DebuggerHidden]
	public GdTaskStatus UnsafeGetStatus()
	{
		return _core.UnsafeGetStatus();
	}

	[DebuggerHidden]
	public void OnCompleted(Action<object> continuation, object state, short token)
	{
		_core.OnCompleted(continuation, state, token);
	}
}