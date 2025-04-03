using GdTasks.Models.Disposable;

namespace GdTasks.Extensions;

public static class GdTaskObservableExtensions
{
	public static GdTask<T> ToGdTask<T>(this IObservable<T> source, bool useFirstValue = false, CancellationToken cancellationToken = default)
	{
		var promise = new GdTaskCompletionSource<T>();
		var disposable = new SingleAssignmentDisposable();

		var observer = useFirstValue
			? new FirstValueToGdTaskObserver<T>(promise, disposable, cancellationToken)
			: (IObserver<T>)new ToGdTaskObserver<T>(promise, disposable, cancellationToken);

		try
		{
			disposable.Disposable = source.Subscribe(observer);
		}
		catch (Exception ex)
		{
			promise.TrySetException(ex);
		}

		return promise.Task;
	}

	public static IObservable<T> ToObservable<T>(this GdTask<T> task)
	{
		if (task.Status.IsCompleted())
		{
			try
			{
				return new ReturnObservable<T>(task.GetAwaiter().GetResult());
			}
			catch (Exception ex)
			{
				return new ThrowObservable<T>(ex);
			}
		}

		var subject = new AsyncSubject<T>();
		Fire(subject, task).Forget();
		return subject;
	}

	/// <summary>
	/// Ideally returns IObservabl[Unit] is best but GDTask does not have Unit so return AsyncUnit instead.
	/// </summary>
	public static IObservable<AsyncUnit> ToObservable(this GdTask task)
	{
		if (task.Status.IsCompleted())
		{
			try
			{
				task.GetAwaiter().GetResult();
				return new ReturnObservable<AsyncUnit>(AsyncUnit.Default);
			}
			catch (Exception ex)
			{
				return new ThrowObservable<AsyncUnit>(ex);
			}
		}

		var subject = new AsyncSubject<AsyncUnit>();
		Fire(subject, task).Forget();
		return subject;
	}

	private static async GdTaskVoid Fire<T>(AsyncSubject<T> subject, GdTask<T> task)
	{
		T value;
		try
		{
			value = await task;
		}
		catch (Exception ex)
		{
			subject.OnError(ex);
			return;
		}

		subject.OnNext(value);
		subject.OnCompleted();
	}

	private static async GdTaskVoid Fire(AsyncSubject<AsyncUnit> subject, GdTask task)
	{
		try
		{
			await task;
		}
		catch (Exception ex)
		{
			subject.OnError(ex);
			return;
		}

		subject.OnNext(AsyncUnit.Default);
		subject.OnCompleted();
	}

	private class FirstValueToGdTaskObserver<T> : IObserver<T>
	{
		private static readonly Action<object> Callback = OnCanceled;

		private readonly CancellationToken _cancellationToken;
		private readonly SingleAssignmentDisposable _disposable;
		private readonly GdTaskCompletionSource<T> _promise;
		private readonly CancellationTokenRegistration _registration;

		private bool _hasValue;

		public FirstValueToGdTaskObserver(GdTaskCompletionSource<T> promise, SingleAssignmentDisposable disposable, CancellationToken cancellationToken)
		{
			_promise = promise;
			_disposable = disposable;
			_cancellationToken = cancellationToken;

			if (_cancellationToken.CanBeCanceled)
			{
				_registration = _cancellationToken.RegisterWithoutCaptureExecutionContext(Callback, this);
			}
		}

		public void OnCompleted()
		{
			try
			{
				if (!_hasValue)
				{
					_promise.TrySetException(new InvalidOperationException("Sequence has no elements"));
				}
			}
			finally
			{
				_registration.Dispose();
				_disposable.Dispose();
			}
		}

		public void OnError(Exception error)
		{
			try
			{
				_promise.TrySetException(error);
			}
			finally
			{
				_registration.Dispose();
				_disposable.Dispose();
			}
		}

		public void OnNext(T value)
		{
			_hasValue = true;
			try
			{
				_promise.TrySetResult(value);
			}
			finally
			{
				_registration.Dispose();
				_disposable.Dispose();
			}
		}

		private static void OnCanceled(object state)
		{
			var self = (FirstValueToGdTaskObserver<T>)state;
			self._disposable.Dispose();
			self._promise.TrySetCanceled(self._cancellationToken);
		}
	}

	private class ReturnObservable<T>(T value) : IObservable<T>
	{
		public IDisposable Subscribe(IObserver<T> observer)
		{
			observer.OnNext(value);
			observer.OnCompleted();
			return EmptyDisposable.Instance;
		}
	}

	private class ThrowObservable<T>(Exception value) : IObservable<T>
	{
		public IDisposable Subscribe(IObserver<T> observer)
		{
			observer.OnError(value);
			return EmptyDisposable.Instance;
		}
	}

	private class ToGdTaskObserver<T> : IObserver<T>
	{
		private static readonly Action<object> Callback = OnCanceled;

		private readonly CancellationToken _cancellationToken;
		private readonly SingleAssignmentDisposable _disposable;
		private readonly GdTaskCompletionSource<T> _promise;
		private readonly CancellationTokenRegistration _registration;

		private bool _hasValue;
		private T _latestValue;

		public ToGdTaskObserver(GdTaskCompletionSource<T> promise, SingleAssignmentDisposable disposable, CancellationToken cancellationToken)
		{
			_promise = promise;
			_disposable = disposable;
			_cancellationToken = cancellationToken;

			if (_cancellationToken.CanBeCanceled)
				_registration = _cancellationToken.RegisterWithoutCaptureExecutionContext(Callback, this);
		}

		public void OnCompleted()
		{
			try
			{
				if (_hasValue)
				{
					_promise.TrySetResult(_latestValue);
				}
				else
				{
					_promise.TrySetException(new InvalidOperationException("Sequence has no elements"));
				}
			}
			finally
			{
				_registration.Dispose();
				_disposable.Dispose();
			}
		}

		public void OnError(Exception error)
		{
			try
			{
				_promise.TrySetException(error);
			}
			finally
			{
				_registration.Dispose();
				_disposable.Dispose();
			}
		}

		public void OnNext(T value)
		{
			_hasValue = true;
			_latestValue = value;
		}

		private static void OnCanceled(object state)
		{
			var self = (ToGdTaskObserver<T>)state;
			self._disposable.Dispose();
			self._promise.TrySetCanceled(self._cancellationToken);
		}
	}
}

// Bridges for Rx.