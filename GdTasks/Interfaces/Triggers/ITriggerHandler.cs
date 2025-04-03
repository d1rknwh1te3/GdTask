namespace GdTasks.Interfaces.Triggers;

public interface ITriggerHandler<T>
{
	public void OnNext(T value);

	public void OnError(Exception ex);

	public void OnCompleted();

	public void OnCanceled(CancellationToken cancellationToken);

	// set/get from TriggerEvent<T>
	public ITriggerHandler<T> Prev { get; set; }

	public ITriggerHandler<T> Next { get; set; }
}