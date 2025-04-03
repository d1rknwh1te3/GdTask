namespace GdTasks.Models.Observers;

internal class EmptyObserver<T> : IObserver<T>
{
	public static readonly EmptyObserver<T> Instance = new();

	private EmptyObserver()
	{ }

	public void OnCompleted()
	{ }

	public void OnError(Exception error)
	{ }

	public void OnNext(T value)
	{ }
}