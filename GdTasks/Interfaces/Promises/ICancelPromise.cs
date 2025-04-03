namespace GdTasks.Interfaces.Promises;

public interface ICancelPromise
{
	bool TrySetCanceled(CancellationToken cancellationToken = default);
}