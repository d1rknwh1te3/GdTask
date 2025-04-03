namespace GdTasks.Models.Observers;

internal class ListObserver<T>(ImmutableList<IObserver<T>> observers) : IObserver<T>
{
	public void OnCompleted()
	{
		var targetObservers = observers.Data;

		foreach (var t in targetObservers)
		{
			t.OnCompleted();
		}
	}

	public void OnError(Exception error)
	{
		var targetObservers = observers.Data;

		foreach (var t in targetObservers)
		{
			t.OnError(error);
		}
	}

	public void OnNext(T value)
	{
		var targetObservers = observers.Data;

		foreach (var t in targetObservers)
		{
			t.OnNext(value);
		}
	}

	internal IObserver<T> Add(IObserver<T> observer) => new ListObserver<T>(observers.Add(observer));

	internal IObserver<T> Remove(IObserver<T> observer)
	{
		var i = Array.IndexOf(observers.Data, observer);

		if (i < 0)
			return this;

		return observers.Data.Length == 2
			? observers.Data[1 - i]
			: new ListObserver<T>(observers.Remove(observer));
	}
}