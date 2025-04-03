using System.Runtime.ExceptionServices;

namespace GdTasks.Models.Observers;

internal class ThrowObserver<T> : IObserver<T>
{
	public static readonly ThrowObserver<T> Instance = new();

	private ThrowObserver()
	{ }

	public void OnCompleted()
	{ }

	public void OnError(Exception error) => ExceptionDispatchInfo.Capture(error).Throw();

	public void OnNext(T value)
	{ }
}