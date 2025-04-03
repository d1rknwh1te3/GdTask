namespace GdTasks.Models.Observers;

internal class DisposedObserver<T> : IObserver<T>
{
	public static readonly DisposedObserver<T> Instance = new();

	private DisposedObserver()
	{ }

	public void OnCompleted() => throw new ObjectDisposedException("");

	public void OnError(Exception error) => throw new ObjectDisposedException("");

	public void OnNext(T value) => throw new ObjectDisposedException("");
}