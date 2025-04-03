using GdTasks.Models.Disposable;
using GdTasks.Models.Observers;
using System.Runtime.ExceptionServices;

namespace GdTasks.Models;

internal sealed class AsyncSubject<T> : IObservable<T>, IObserver<T>
{
	private bool _hasValue;
	private bool _isDisposed;
	private bool _isStopped;
	private Exception _lastError;
	private T _lastValue;
	private object _observerLock = new();
	private IObserver<T> _outObserver = EmptyObserver<T>.Instance;

	public bool HasObservers => _outObserver is not EmptyObserver<T> && !_isStopped && !_isDisposed;

	public bool IsCompleted => _isStopped;

	public T Value
	{
		get
		{
			ThrowIfDisposed();

			if (!_isStopped)
				throw new InvalidOperationException("AsyncSubject is not completed yet");

			if (_lastError != null)
				ExceptionDispatchInfo.Capture(_lastError).Throw();

			return _lastValue;
		}
	}
	public void Dispose()
	{
		lock (_observerLock)
		{
			_isDisposed = true;
			_outObserver = DisposedObserver<T>.Instance;
			_lastError = null;
			_lastValue = default(T);
		}
	}

	public void OnCompleted()
	{
		IObserver<T> old;
		T v;
		bool hv;
		lock (_observerLock)
		{
			ThrowIfDisposed();
			if (_isStopped) return;

			old = _outObserver;
			_outObserver = EmptyObserver<T>.Instance;
			_isStopped = true;
			v = _lastValue;
			hv = _hasValue;
		}

		if (hv)
			old.OnNext(v);

		old.OnCompleted();
	}

	public void OnError(Exception error)
	{
		ArgumentNullException.ThrowIfNull(error);

		IObserver<T> old;
		lock (_observerLock)
		{
			ThrowIfDisposed();
			if (_isStopped) return;

			old = _outObserver;
			_outObserver = EmptyObserver<T>.Instance;
			_isStopped = true;
			_lastError = error;
		}

		old.OnError(error);
	}

	public void OnNext(T value)
	{
		lock (_observerLock)
		{
			ThrowIfDisposed();
			if (_isStopped) return;

			_hasValue = true;
			_lastValue = value;
		}
	}

	public IDisposable Subscribe(IObserver<T> observer)
	{
		ArgumentNullException.ThrowIfNull(observer);

		Exception ex;
		T v;
		bool hv;

		lock (_observerLock)
		{
			ThrowIfDisposed();
			if (!_isStopped)
			{
				if (_outObserver is ListObserver<T> listObserver)
				{
					_outObserver = listObserver.Add(observer);
				}
				else
				{
					var current = _outObserver;

					_outObserver = current is EmptyObserver<T>
						? observer
						: new ListObserver<T>(new ImmutableList<IObserver<T>>([current, observer]));
				}

				return new Subscription(this, observer);
			}

			ex = _lastError;
			v = _lastValue;
			hv = _hasValue;
		}

		if (ex != null)
		{
			observer.OnError(ex);
		}
		else if (hv)
		{
			observer.OnNext(v);
			observer.OnCompleted();
		}
		else
		{
			observer.OnCompleted();
		}

		return EmptyDisposable.Instance;
	}
	private void ThrowIfDisposed()
	{
		if (_isDisposed) throw new ObjectDisposedException("");
	}

	private class Subscription(AsyncSubject<T> parent, IObserver<T> unsubscribeTarget) : IDisposable
	{
		private readonly object _gate = new();
		private AsyncSubject<T> _parent = parent;
		private IObserver<T> _unsubscribeTarget = unsubscribeTarget;

		public void Dispose()
		{
			lock (_gate)
			{
				if (_parent == null)
					return;

				lock (_parent._observerLock)
				{
					if (_parent._outObserver is ListObserver<T> listObserver)
					{
						_parent._outObserver = listObserver.Remove(_unsubscribeTarget);
					}
					else
					{
						_parent._outObserver = EmptyObserver<T>.Instance;
					}

					_unsubscribeTarget = null;
					_parent = null;
				}
			}
		}
	}
}