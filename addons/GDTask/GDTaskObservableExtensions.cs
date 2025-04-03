﻿using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using Fractural.Tasks.Internal;

namespace Fractural.Tasks
{
    public static class GdTaskObservableExtensions
    {
        public static GdTask<T> ToGdTask<T>(this IObservable<T> source, bool useFirstValue = false, CancellationToken cancellationToken = default)
        {
            var promise = new GdTaskCompletionSource<T>();
            var disposable = new SingleAssignmentDisposable();

            var observer = useFirstValue
                ? (IObserver<T>)new FirstValueToGdTaskObserver<T>(promise, disposable, cancellationToken)
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

        private class ToGdTaskObserver<T> : IObserver<T>
        {
	        private static readonly Action<object> Callback = OnCanceled;

	        private readonly GdTaskCompletionSource<T> _promise;
	        private readonly SingleAssignmentDisposable _disposable;
	        private readonly CancellationToken _cancellationToken;
	        private readonly CancellationTokenRegistration _registration;

	        private bool _hasValue;
	        private T _latestValue;

            public ToGdTaskObserver(GdTaskCompletionSource<T> promise, SingleAssignmentDisposable disposable, CancellationToken cancellationToken)
            {
                _promise = promise;
                _disposable = disposable;
                _cancellationToken = cancellationToken;

                if (_cancellationToken.CanBeCanceled)
                {
                    _registration = _cancellationToken.RegisterWithoutCaptureExecutionContext(Callback, this);
                }
            }

            private static void OnCanceled(object state)
            {
                var self = (ToGdTaskObserver<T>)state;
                self._disposable.Dispose();
                self._promise.TrySetCanceled(self._cancellationToken);
            }

            public void OnNext(T value)
            {
                _hasValue = true;
                _latestValue = value;
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
        }

        private class FirstValueToGdTaskObserver<T> : IObserver<T>
        {
	        private static readonly Action<object> Callback = OnCanceled;

	        private readonly GdTaskCompletionSource<T> _promise;
	        private readonly SingleAssignmentDisposable _disposable;
	        private readonly CancellationToken _cancellationToken;
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

            private static void OnCanceled(object state)
            {
                var self = (FirstValueToGdTaskObserver<T>)state;
                self._disposable.Dispose();
                self._promise.TrySetCanceled(self._cancellationToken);
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
        }

        private class ReturnObservable<T>(T value) : IObservable<T>
        {
	        public IDisposable Subscribe(IObserver<T> observer)
            {
                observer.OnNext(value);
                observer.OnCompleted();
                return EmptyDisposable.instance;
            }
        }

        private class ThrowObservable<T>(Exception value) : IObservable<T>
        {
	        public IDisposable Subscribe(IObserver<T> observer)
            {
                observer.OnError(value);
                return EmptyDisposable.instance;
            }
        }
    }
}

namespace Fractural.Tasks.Internal
{
    // Bridges for Rx.

    internal class EmptyDisposable : IDisposable
    {
        public static EmptyDisposable instance = new EmptyDisposable();

        private EmptyDisposable()
        {

        }

        public void Dispose()
        {
        }
    }

    internal sealed class SingleAssignmentDisposable : IDisposable
    {
	    private readonly object _gate = new object();
	    private IDisposable _current;
	    private bool _disposed;

        public bool IsDisposed { get { lock (_gate) { return _disposed; } } }

        public IDisposable Disposable
        {
            get
            {
                return _current;
            }
            set
            {
                var old = default(IDisposable);
                bool alreadyDisposed;
                lock (_gate)
                {
                    alreadyDisposed = _disposed;
                    old = _current;
                    if (!alreadyDisposed)
                    {
                        if (value == null) return;
                        _current = value;
                    }
                }

                if (alreadyDisposed && value != null)
                {
                    value.Dispose();
                    return;
                }

                if (old != null) throw new InvalidOperationException("Disposable is already set");
            }
        }


        public void Dispose()
        {
            IDisposable old = null;

            lock (_gate)
            {
                if (!_disposed)
                {
                    _disposed = true;
                    old = _current;
                    _current = null;
                }
            }

            if (old != null) old.Dispose();
        }
    }

    internal sealed class AsyncSubject<T> : IObservable<T>, IObserver<T>
    {
	    private object _observerLock = new object();

	    private T _lastValue;
	    private bool _hasValue;
	    private bool _isStopped;
	    private bool _isDisposed;
	    private Exception _lastError;
	    private IObserver<T> _outObserver = EmptyObserver<T>.Instance;

        public T Value
        {
            get
            {
                ThrowIfDisposed();
                if (!_isStopped) throw new InvalidOperationException("AsyncSubject is not completed yet");
                if (_lastError != null) ExceptionDispatchInfo.Capture(_lastError).Throw();
                return _lastValue;
            }
        }

        public bool HasObservers
        {
            get
            {
                return !(_outObserver is EmptyObserver<T>) && !_isStopped && !_isDisposed;
            }
        }

        public bool IsCompleted { get { return _isStopped; } }

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
            {
                old.OnNext(v);
                old.OnCompleted();
            }
            else
            {
                old.OnCompleted();
            }
        }

        public void OnError(Exception error)
        {
            if (error == null) throw new ArgumentNullException("error");

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
            if (observer == null) throw new ArgumentNullException("observer");

            var ex = default(Exception);
            var v = default(T);
            var hv = false;

            lock (_observerLock)
            {
                ThrowIfDisposed();
                if (!_isStopped)
                {
                    var listObserver = _outObserver as ListObserver<T>;
                    if (listObserver != null)
                    {
                        _outObserver = listObserver.Add(observer);
                    }
                    else
                    {
                        var current = _outObserver;
                        if (current is EmptyObserver<T>)
                        {
                            _outObserver = observer;
                        }
                        else
                        {
                            _outObserver = new ListObserver<T>(new ImmutableList<IObserver<T>>(new[] { current, observer }));
                        }
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

            return EmptyDisposable.instance;
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

        private void ThrowIfDisposed()
        {
            if (_isDisposed) throw new ObjectDisposedException("");
        }

        private class Subscription(AsyncSubject<T> parent, IObserver<T> unsubscribeTarget) : IDisposable
        {
	        private readonly object _gate = new object();
	        private AsyncSubject<T> _parent = parent;
	        private IObserver<T> _unsubscribeTarget = unsubscribeTarget;

	        public void Dispose()
            {
                lock (_gate)
                {
                    if (_parent != null)
                    {
                        lock (_parent._observerLock)
                        {
                            var listObserver = _parent._outObserver as ListObserver<T>;
                            if (listObserver != null)
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
    }

    internal class ListObserver<T>(ImmutableList<IObserver<T>> observers) : IObserver<T>
    {
	    public void OnCompleted()
        {
            var targetObservers = observers.Data;
            for (int i = 0; i < targetObservers.Length; i++)
            {
                targetObservers[i].OnCompleted();
            }
        }

        public void OnError(Exception error)
        {
            var targetObservers = observers.Data;
            for (int i = 0; i < targetObservers.Length; i++)
            {
                targetObservers[i].OnError(error);
            }
        }

        public void OnNext(T value)
        {
            var targetObservers = observers.Data;
            for (int i = 0; i < targetObservers.Length; i++)
            {
                targetObservers[i].OnNext(value);
            }
        }

        internal IObserver<T> Add(IObserver<T> observer)
        {
            return new ListObserver<T>(observers.Add(observer));
        }

        internal IObserver<T> Remove(IObserver<T> observer)
        {
            var i = Array.IndexOf(observers.Data, observer);
            if (i < 0)
                return this;

            if (observers.Data.Length == 2)
            {
                return observers.Data[1 - i];
            }
            else
            {
                return new ListObserver<T>(observers.Remove(observer));
            }
        }
    }

    internal class EmptyObserver<T> : IObserver<T>
    {
        public static readonly EmptyObserver<T> Instance = new EmptyObserver<T>();

        private EmptyObserver()
        {

        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(T value)
        {
        }
    }

    internal class ThrowObserver<T> : IObserver<T>
    {
        public static readonly ThrowObserver<T> Instance = new ThrowObserver<T>();

        private ThrowObserver()
        {

        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
            ExceptionDispatchInfo.Capture(error).Throw();
        }

        public void OnNext(T value)
        {
        }
    }

    internal class DisposedObserver<T> : IObserver<T>
    {
        public static readonly DisposedObserver<T> Instance = new DisposedObserver<T>();

        private DisposedObserver()
        {

        }

        public void OnCompleted()
        {
            throw new ObjectDisposedException("");
        }

        public void OnError(Exception error)
        {
            throw new ObjectDisposedException("");
        }

        public void OnNext(T value)
        {
            throw new ObjectDisposedException("");
        }
    }

    internal class ImmutableList<T>(T[] data)
    {
        public static readonly ImmutableList<T> Empty = new ImmutableList<T>();

        public T[] Data
        {
            get { return data; }
        }

        private ImmutableList() : this(new T[0])
        {
        }

        public ImmutableList<T> Add(T value)
        {
            var newData = new T[data.Length + 1];
            Array.Copy(data, newData, data.Length);
            newData[data.Length] = value;
            return new ImmutableList<T>(newData);
        }

        public ImmutableList<T> Remove(T value)
        {
            var i = IndexOf(value);
            if (i < 0) return this;

            var length = data.Length;
            if (length == 1) return Empty;

            var newData = new T[length - 1];

            Array.Copy(data, 0, newData, 0, i);
            Array.Copy(data, i + 1, newData, i, length - i - 1);

            return new ImmutableList<T>(newData);
        }

        public int IndexOf(T value)
        {
            for (var i = 0; i < data.Length; ++i)
            {
                // ImmutableList only use for IObserver(no worry for boxed)
                if (Equals(data[i], value)) return i;
            }
            return -1;
        }
    }
}

