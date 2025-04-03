namespace GdTasks.Interfaces.States;

#pragma warning disable CS1591

internal interface IStateMachineRunnerPromise<T> : IGdTaskSource<T>
{
	Action MoveNext { get; }
	GdTask<T> Task { get; }

	void SetResult(T result);

	void SetException(Exception exception);
}

#pragma warning restore CS1591

internal interface IStateMachineRunnerPromise : IGdTaskSource
{
	Action MoveNext { get; }
	GdTask Task { get; }

	void SetResult();

	void SetException(Exception exception);
}