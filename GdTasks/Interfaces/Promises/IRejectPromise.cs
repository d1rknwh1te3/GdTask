namespace GdTasks.Interfaces.Promises;

public interface IRejectPromise
{
	bool TrySetException(Exception exception);
}