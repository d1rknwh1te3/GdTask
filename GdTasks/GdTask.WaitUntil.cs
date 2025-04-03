using Godot;

namespace GdTasks;

public partial struct GdTask
{
	public static GdTask WaitUntil(GodotObject target, Func<bool> predicate, PlayerLoopTiming timing = PlayerLoopTiming.Process, CancellationToken cancellationToken = default)
		=> new(WaitUntilPromise.Create(target, predicate, timing, cancellationToken, out var token), token);

	public static GdTask WaitUntil(Func<bool> predicate, PlayerLoopTiming timing = PlayerLoopTiming.Process, CancellationToken cancellationToken = default)
		=> WaitUntil(null, predicate, timing, cancellationToken);

	public static GdTask WaitUntilCanceled(GodotObject target, CancellationToken cancellationToken, PlayerLoopTiming timing = PlayerLoopTiming.Process)
		=> new(WaitUntilCanceledPromise.Create(target, cancellationToken, timing, out var token), token);

	public static GdTask WaitUntilCanceled(CancellationToken cancellationToken, PlayerLoopTiming timing = PlayerLoopTiming.Process)
		=> WaitUntilCanceled(null, cancellationToken, timing);

	public static GdTask<TU> WaitUntilValueChanged<T, TU>(T target, Func<T, TU> monitorFunction, PlayerLoopTiming monitorTiming = PlayerLoopTiming.Process, IEqualityComparer<TU> equalityComparer = null, CancellationToken cancellationToken = default)
		where T : class
	{
		return new GdTask<TU>(target is GodotObject
			? WaitUntilValueChangedGodotObjectPromise<T, TU>.Create(target, monitorFunction, equalityComparer, monitorTiming, cancellationToken, out var token)
			: WaitUntilValueChangedStandardObjectPromise<T, TU>.Create(target, monitorFunction, equalityComparer, monitorTiming, cancellationToken, out token), token);
	}

	public static GdTask WaitWhile(GodotObject target, Func<bool> predicate, PlayerLoopTiming timing = PlayerLoopTiming.Process, CancellationToken cancellationToken = default)
					=> new(WaitWhilePromise.Create(target, predicate, timing, cancellationToken, out var token), token);

	public static GdTask WaitWhile(Func<bool> predicate, PlayerLoopTiming timing = PlayerLoopTiming.Process, CancellationToken cancellationToken = default)
		=> WaitWhile(null, predicate, timing, cancellationToken);
	private sealed class WaitUntilCanceledPromise : IGdTaskSource, IPlayerLoopItem, ITaskPoolNode<WaitUntilCanceledPromise>
	{
		private static TaskPool<WaitUntilCanceledPromise> _pool;
		private CancellationToken _cancellationToken;
		private GdTaskCompletionSourceCore<object> _core;
		private WaitUntilCanceledPromise _nextNode;
		private GodotObject _target;
		static WaitUntilCanceledPromise() => TaskPool.RegisterSizeGetter(typeof(WaitUntilCanceledPromise), () => _pool.Size);

		private WaitUntilCanceledPromise()
		{ }

		public ref WaitUntilCanceledPromise NextNode => ref _nextNode;
		public static IGdTaskSource Create(GodotObject target, CancellationToken cancellationToken, PlayerLoopTiming timing, out short token)
		{
			if (cancellationToken.IsCancellationRequested)
				return AutoResetGdTaskCompletionSource.CreateFromCanceled(cancellationToken, out token);

			if (!_pool.TryPop(out var result))
				result = new WaitUntilCanceledPromise();

			result._target = target;
			result._cancellationToken = cancellationToken;

			TaskTracker.TrackActiveTask(result, 3);
			GdTaskPlayerLoopAutoload.AddAction(timing, result);

			token = result._core.Version;
			return result;
		}

		public void GetResult(short token)
		{
			try
			{
				_core.GetResult(token);
			}
			finally
			{
				TryReturn();
			}
		}

		public GdTaskStatus GetStatus(short token) => _core.GetStatus(token);

		public bool MoveNext()
		{
			if (!_cancellationToken.IsCancellationRequested && (_target is null || GodotObject.IsInstanceValid(_target)))
				return true; // Cancel when destroyed

			_core.TrySetResult(null);
			return false;
		}

		public void OnCompleted(Action<object> continuation, object state, short token) => _core.OnCompleted(continuation, state, token);

		public GdTaskStatus UnsafeGetStatus() => _core.UnsafeGetStatus();
		private bool TryReturn()
		{
			TaskTracker.RemoveTracking(this);
			_core.Reset();
			_cancellationToken = CancellationToken.None;
			return _pool.TryPush(this);
		}
	}

	private sealed class WaitUntilPromise : IGdTaskSource, IPlayerLoopItem, ITaskPoolNode<WaitUntilPromise>
	{
		private static TaskPool<WaitUntilPromise> _pool;
		private CancellationToken _cancellationToken;
		private GdTaskCompletionSourceCore<object> _core;
		private WaitUntilPromise _nextNode;
		private Func<bool> _predicate;
		private GodotObject _target;
		static WaitUntilPromise() => TaskPool.RegisterSizeGetter(typeof(WaitUntilPromise), () => _pool.Size);

		private WaitUntilPromise()
		{ }

		public ref WaitUntilPromise NextNode => ref _nextNode;
		public static IGdTaskSource Create(GodotObject target, Func<bool> predicate, PlayerLoopTiming timing, CancellationToken cancellationToken, out short token)
		{
			if (cancellationToken.IsCancellationRequested)
				return AutoResetGdTaskCompletionSource.CreateFromCanceled(cancellationToken, out token);

			if (!_pool.TryPop(out var result))
				result = new WaitUntilPromise();

			result._target = target;
			result._predicate = predicate;
			result._cancellationToken = cancellationToken;

			TaskTracker.TrackActiveTask(result, 3);
			GdTaskPlayerLoopAutoload.AddAction(timing, result);

			token = result._core.Version;
			return result;
		}

		public void GetResult(short token)
		{
			try
			{
				_core.GetResult(token);
			}
			finally
			{
				TryReturn();
			}
		}

		public GdTaskStatus GetStatus(short token) => _core.GetStatus(token);

		public bool MoveNext()
		{
			if (_cancellationToken.IsCancellationRequested || (_target is not null && !GodotObject.IsInstanceValid(_target))) // Cancel when destroyed
			{
				_core.TrySetCanceled(_cancellationToken);
				return false;
			}

			try
			{
				if (!_predicate())
				{
					return true;
				}
			}
			catch (Exception ex)
			{
				_core.TrySetException(ex);
				return false;
			}

			_core.TrySetResult(null);
			return false;
		}

		public void OnCompleted(Action<object> continuation, object state, short token) => _core.OnCompleted(continuation, state, token);

		public GdTaskStatus UnsafeGetStatus() => _core.UnsafeGetStatus();
		private bool TryReturn()
		{
			TaskTracker.RemoveTracking(this);
			_core.Reset();
			_predicate = null;
			_cancellationToken = CancellationToken.None;
			return _pool.TryPush(this);
		}
	}

	// Cannot add `where T : GodotObject` because `WaitUntilValueChanged` doesn't have the constraint.
	private sealed class WaitUntilValueChangedGodotObjectPromise<T, TU> : IGdTaskSource<TU>, IPlayerLoopItem, ITaskPoolNode<WaitUntilValueChangedGodotObjectPromise<T, TU>>
	{
		private static TaskPool<WaitUntilValueChangedGodotObjectPromise<T, TU>> _pool;
		private CancellationToken _cancellationToken;
		private GdTaskCompletionSourceCore<TU> _core;
		private TU _currentValue;
		private IEqualityComparer<TU> _equalityComparer;
		private Func<T, TU> _monitorFunction;
		private WaitUntilValueChangedGodotObjectPromise<T, TU> _nextNode;
		private T _target;
		private GodotObject _targetGodotObject;
		static WaitUntilValueChangedGodotObjectPromise()
		{
			TaskPool.RegisterSizeGetter(typeof(WaitUntilValueChangedGodotObjectPromise<T, TU>), () => _pool.Size);
		}

		private WaitUntilValueChangedGodotObjectPromise()
		{
		}

		public ref WaitUntilValueChangedGodotObjectPromise<T, TU> NextNode => ref _nextNode;
		public static IGdTaskSource<TU> Create(T target, Func<T, TU> monitorFunction, IEqualityComparer<TU> equalityComparer, PlayerLoopTiming timing, CancellationToken cancellationToken, out short token)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return AutoResetGdTaskCompletionSource<TU>.CreateFromCanceled(cancellationToken, out token);
			}

			if (!_pool.TryPop(out var result))
			{
				result = new WaitUntilValueChangedGodotObjectPromise<T, TU>();
			}

			result._target = target;
			result._targetGodotObject = target as GodotObject;
			result._monitorFunction = monitorFunction;
			result._currentValue = monitorFunction(target);
			result._equalityComparer = equalityComparer ?? GodotEqualityComparer.GetDefault<TU>();
			result._cancellationToken = cancellationToken;

			TaskTracker.TrackActiveTask(result, 3);

			GdTaskPlayerLoopAutoload.AddAction(timing, result);

			token = result._core.Version;
			return result;
		}

		public TU GetResult(short token)
		{
			try
			{
				return _core.GetResult(token);
			}
			finally
			{
				TryReturn();
			}
		}

		void IGdTaskSource.GetResult(short token)
		{
			GetResult(token);
		}

		public GdTaskStatus GetStatus(short token)
		{
			return _core.GetStatus(token);
		}

		public bool MoveNext()
		{
			if (_cancellationToken.IsCancellationRequested || (_target is not null && !GodotObject.IsInstanceValid(_targetGodotObject))) // Cancel when destroyed
			{
				_core.TrySetCanceled(_cancellationToken);
				return false;
			}

			TU nextValue = default;
			try
			{
				nextValue = _monitorFunction(_target);
				if (_equalityComparer.Equals(_currentValue, nextValue))
				{
					return true;
				}
			}
			catch (Exception ex)
			{
				_core.TrySetException(ex);
				return false;
			}

			_core.TrySetResult(nextValue);
			return false;
		}

		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			_core.OnCompleted(continuation, state, token);
		}

		public GdTaskStatus UnsafeGetStatus()
		{
			return _core.UnsafeGetStatus();
		}
		private bool TryReturn()
		{
			TaskTracker.RemoveTracking(this);
			_core.Reset();
			_target = default;
			_currentValue = default;
			_monitorFunction = null;
			_equalityComparer = null;
			_cancellationToken = CancellationToken.None;
			return _pool.TryPush(this);
		}
	}

	private sealed class WaitUntilValueChangedStandardObjectPromise<T, TU> : IGdTaskSource<TU>, IPlayerLoopItem, ITaskPoolNode<WaitUntilValueChangedStandardObjectPromise<T, TU>>
		where T : class
	{
		private static TaskPool<WaitUntilValueChangedStandardObjectPromise<T, TU>> _pool;
		private CancellationToken _cancellationToken;
		private GdTaskCompletionSourceCore<TU> _core;
		private TU _currentValue;
		private IEqualityComparer<TU> _equalityComparer;
		private Func<T, TU> _monitorFunction;
		private WaitUntilValueChangedStandardObjectPromise<T, TU> _nextNode;
		private WeakReference<T> _target;
		static WaitUntilValueChangedStandardObjectPromise()
		{
			TaskPool.RegisterSizeGetter(typeof(WaitUntilValueChangedStandardObjectPromise<T, TU>), () => _pool.Size);
		}

		private WaitUntilValueChangedStandardObjectPromise()
		{
		}

		public ref WaitUntilValueChangedStandardObjectPromise<T, TU> NextNode => ref _nextNode;
		public static IGdTaskSource<TU> Create(T target, Func<T, TU> monitorFunction, IEqualityComparer<TU> equalityComparer, PlayerLoopTiming timing, CancellationToken cancellationToken, out short token)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return AutoResetGdTaskCompletionSource<TU>.CreateFromCanceled(cancellationToken, out token);
			}

			if (!_pool.TryPop(out var result))
			{
				result = new WaitUntilValueChangedStandardObjectPromise<T, TU>();
			}

			result._target = new WeakReference<T>(target, false); // wrap in WeakReference.
			result._monitorFunction = monitorFunction;
			result._currentValue = monitorFunction(target);
			result._equalityComparer = equalityComparer ?? GodotEqualityComparer.GetDefault<TU>();
			result._cancellationToken = cancellationToken;

			TaskTracker.TrackActiveTask(result, 3);

			GdTaskPlayerLoopAutoload.AddAction(timing, result);

			token = result._core.Version;
			return result;
		}

		public TU GetResult(short token)
		{
			try
			{
				return _core.GetResult(token);
			}
			finally
			{
				TryReturn();
			}
		}

		void IGdTaskSource.GetResult(short token)
		{
			GetResult(token);
		}

		public GdTaskStatus GetStatus(short token)
		{
			return _core.GetStatus(token);
		}

		public bool MoveNext()
		{
			if (_cancellationToken.IsCancellationRequested || !_target.TryGetTarget(out var t)) // doesn't find = cancel.
			{
				_core.TrySetCanceled(_cancellationToken);
				return false;
			}

			TU nextValue = default;
			try
			{
				nextValue = _monitorFunction(t);
				if (_equalityComparer.Equals(_currentValue, nextValue))
				{
					return true;
				}
			}
			catch (Exception ex)
			{
				_core.TrySetException(ex);
				return false;
			}

			_core.TrySetResult(nextValue);
			return false;
		}

		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			_core.OnCompleted(continuation, state, token);
		}

		public GdTaskStatus UnsafeGetStatus()
		{
			return _core.UnsafeGetStatus();
		}
		private bool TryReturn()
		{
			TaskTracker.RemoveTracking(this);
			_core.Reset();
			_target = null;
			_currentValue = default;
			_monitorFunction = null;
			_equalityComparer = null;
			_cancellationToken = CancellationToken.None;
			return _pool.TryPush(this);
		}
	}

	private sealed class WaitWhilePromise : IGdTaskSource, IPlayerLoopItem, ITaskPoolNode<WaitWhilePromise>
	{
		private static TaskPool<WaitWhilePromise> _pool;
		private CancellationToken _cancellationToken;
		private GdTaskCompletionSourceCore<object> _core;
		private WaitWhilePromise _nextNode;
		private Func<bool> _predicate;
		private GodotObject _target;
		static WaitWhilePromise() => TaskPool.RegisterSizeGetter(typeof(WaitWhilePromise), () => _pool.Size);

		private WaitWhilePromise()
		{ }

		public ref WaitWhilePromise NextNode => ref _nextNode;
		public static IGdTaskSource Create(GodotObject target, Func<bool> predicate, PlayerLoopTiming timing, CancellationToken cancellationToken, out short token)
		{
			if (cancellationToken.IsCancellationRequested)
				return AutoResetGdTaskCompletionSource.CreateFromCanceled(cancellationToken, out token);

			if (!_pool.TryPop(out var result))
				result = new WaitWhilePromise();

			result._target = target;
			result._predicate = predicate;
			result._cancellationToken = cancellationToken;

			TaskTracker.TrackActiveTask(result, 3);
			GdTaskPlayerLoopAutoload.AddAction(timing, result);

			token = result._core.Version;
			return result;
		}

		public void GetResult(short token)
		{
			try
			{
				_core.GetResult(token);
			}
			finally
			{
				TryReturn();
			}
		}

		public GdTaskStatus GetStatus(short token) => _core.GetStatus(token);

		public bool MoveNext()
		{
			if (_cancellationToken.IsCancellationRequested || (_target is not null && !GodotObject.IsInstanceValid(_target))) // Cancel when destroyed
			{
				_core.TrySetCanceled(_cancellationToken);
				return false;
			}

			try
			{
				if (_predicate())
				{
					return true;
				}
			}
			catch (Exception ex)
			{
				_core.TrySetException(ex);
				return false;
			}

			_core.TrySetResult(null);
			return false;
		}

		public void OnCompleted(Action<object> continuation, object state, short token) => _core.OnCompleted(continuation, state, token);

		public GdTaskStatus UnsafeGetStatus() => _core.UnsafeGetStatus();
		private bool TryReturn()
		{
			TaskTracker.RemoveTracking(this);
			_core.Reset();
			_predicate = null;
			_cancellationToken = CancellationToken.None;
			return _pool.TryPush(this);
		}
	}
}