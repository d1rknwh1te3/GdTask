namespace GdTasks.Models.Disposable;

internal class EmptyDisposable : IDisposable
{
	public static readonly EmptyDisposable Instance = new();

	private EmptyDisposable()
	{ }

	public void Dispose()
	{ }
}