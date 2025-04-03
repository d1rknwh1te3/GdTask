namespace GdTasks.Interfaces.Promises;

public interface IResolvePromise
{
	bool TrySetResult();
}

public interface IResolvePromise<in T>
{
	bool TrySetResult(T value);
}