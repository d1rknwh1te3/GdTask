using Fractural.Tasks.Internal;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading;

namespace Fractural.Tasks;

public partial struct GdTask
{
	private static readonly GdTask CanceledGDTask = new Func<GdTask>(() =>
	{
		return new GdTask(new CanceledResultSource(CancellationToken.None), 0);
	})();

	private static class CanceledGdTaskCache<T>
	{
		public static readonly GdTask<T> Task;

		static CanceledGdTaskCache()
		{
			Task = new GdTask<T>(new CanceledResultSource<T>(CancellationToken.None), 0);
		}
	}

	public static readonly GdTask CompletedTask = new GdTask();

	public static GdTask FromException(Exception ex)
	{
		if (ex is OperationCanceledException oce)
		{
			return FromCanceled(oce.CancellationToken);
		}

		return new GdTask(new ExceptionResultSource(ex), 0);
	}

	public static GdTask<T> FromException<T>(Exception ex)
	{
		if (ex is OperationCanceledException oce)
		{
			return FromCanceled<T>(oce.CancellationToken);
		}

		return new GdTask<T>(new ExceptionResultSource<T>(ex), 0);
	}

	public static GdTask<T> FromResult<T>(T value)
	{
		return new GdTask<T>(value);
	}

	public static GdTask FromCanceled(CancellationToken cancellationToken = default)
	{
		if (cancellationToken == CancellationToken.None)
		{
			return CanceledGDTask;
		}
		else
		{
			return new GdTask(new CanceledResultSource(cancellationToken), 0);
		}
	}

	public static GdTask<T> FromCanceled<T>(CancellationToken cancellationToken = default)
	{
		if (cancellationToken == CancellationToken.None)
		{
			return CanceledGdTaskCache<T>.Task;
		}
		else
		{
			return new GdTask<T>(new CanceledResultSource<T>(cancellationToken), 0);
		}
	}

	public static GdTask Create(Func<GdTask> factory)
	{
		return factory();
	}

	public static GdTask<T> Create<T>(Func<GdTask<T>> factory)
	{
		return factory();
	}

	public static AsyncLazy Lazy(Func<GdTask> factory)
	{
		return new AsyncLazy(factory);
	}

	public static AsyncLazy<T> Lazy<T>(Func<GdTask<T>> factory)
	{
		return new AsyncLazy<T>(factory);
	}

	/// <summary>
	/// helper of fire and forget void action.
	/// </summary>
	public static void Void(Func<GdTaskVoid> asyncAction)
	{
		asyncAction().Forget();
	}

	/// <summary>
	/// helper of fire and forget void action.
	/// </summary>
	public static void Void(Func<CancellationToken, GdTaskVoid> asyncAction, CancellationToken cancellationToken)
	{
		asyncAction(cancellationToken).Forget();
	}

	/// <summary>
	/// helper of fire and forget void action.
	/// </summary>
	public static void Void<T>(Func<T, GdTaskVoid> asyncAction, T state)
	{
		asyncAction(state).Forget();
	}

	/// <summary>
	/// helper of create add GDTaskVoid to delegate.
	/// For example: FooAction = GDTask.Action(async () => { /* */ })
	/// </summary>
	public static Action Action(Func<GdTaskVoid> asyncAction)
	{
		return () => asyncAction().Forget();
	}

	/// <summary>
	/// helper of create add GDTaskVoid to delegate.
	/// </summary>
	public static Action Action(Func<CancellationToken, GdTaskVoid> asyncAction, CancellationToken cancellationToken)
	{
		return () => asyncAction(cancellationToken).Forget();
	}

	/// <summary>
	/// Defer the task creation just before call await.
	/// </summary>
	public static GdTask Defer(Func<GdTask> factory)
	{
		return new GdTask(new DeferPromise(factory), 0);
	}

	/// <summary>
	/// Defer the task creation just before call await.
	/// </summary>
	public static GdTask<T> Defer<T>(Func<GdTask<T>> factory)
	{
		return new GdTask<T>(new DeferPromise<T>(factory), 0);
	}

	/// <summary>
	/// Never complete.
	/// </summary>
	public static GdTask Never(CancellationToken cancellationToken)
	{
		return new GdTask<AsyncUnit>(new NeverPromise<AsyncUnit>(cancellationToken), 0);
	}

	/// <summary>
	/// Never complete.
	/// </summary>
	public static GdTask<T> Never<T>(CancellationToken cancellationToken)
	{
		return new GdTask<T>(new NeverPromise<T>(cancellationToken), 0);
	}

	private sealed class ExceptionResultSource(Exception exception) : IGdTaskSource
	{
		private readonly ExceptionDispatchInfo _exception = ExceptionDispatchInfo.Capture(exception);
		private bool _calledGet;

		public void GetResult(short token)
		{
			if (!_calledGet)
			{
				_calledGet = true;
				GC.SuppressFinalize(this);
			}
			_exception.Throw();
		}

		public GdTaskStatus GetStatus(short token)
		{
			return GdTaskStatus.Faulted;
		}

		public GdTaskStatus UnsafeGetStatus()
		{
			return GdTaskStatus.Faulted;
		}

		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			continuation(state);
		}

		~ExceptionResultSource()
		{
			if (!_calledGet)
			{
				GdTaskScheduler.PublishUnobservedTaskException(_exception.SourceException);
			}
		}
	}

	private sealed class ExceptionResultSource<T>(Exception exception) : IGdTaskSource<T>
	{
		private readonly ExceptionDispatchInfo _exception = ExceptionDispatchInfo.Capture(exception);
		private bool _calledGet;

		public T GetResult(short token)
		{
			if (!_calledGet)
			{
				_calledGet = true;
				GC.SuppressFinalize(this);
			}
			_exception.Throw();
			return default;
		}

		void IGdTaskSource.GetResult(short token)
		{
			if (!_calledGet)
			{
				_calledGet = true;
				GC.SuppressFinalize(this);
			}
			_exception.Throw();
		}

		public GdTaskStatus GetStatus(short token)
		{
			return GdTaskStatus.Faulted;
		}

		public GdTaskStatus UnsafeGetStatus()
		{
			return GdTaskStatus.Faulted;
		}

		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			continuation(state);
		}

		~ExceptionResultSource()
		{
			if (!_calledGet)
			{
				GdTaskScheduler.PublishUnobservedTaskException(_exception.SourceException);
			}
		}
	}

	private sealed class CanceledResultSource(CancellationToken cancellationToken) : IGdTaskSource
	{
		public void GetResult(short token)
		{
			throw new OperationCanceledException(cancellationToken);
		}

		public GdTaskStatus GetStatus(short token)
		{
			return GdTaskStatus.Canceled;
		}

		public GdTaskStatus UnsafeGetStatus()
		{
			return GdTaskStatus.Canceled;
		}

		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			continuation(state);
		}
	}

	private sealed class CanceledResultSource<T>(CancellationToken cancellationToken) : IGdTaskSource<T>
	{
		public T GetResult(short token)
		{
			throw new OperationCanceledException(cancellationToken);
		}

		void IGdTaskSource.GetResult(short token)
		{
			throw new OperationCanceledException(cancellationToken);
		}

		public GdTaskStatus GetStatus(short token)
		{
			return GdTaskStatus.Canceled;
		}

		public GdTaskStatus UnsafeGetStatus()
		{
			return GdTaskStatus.Canceled;
		}

		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			continuation(state);
		}
	}

	private sealed class DeferPromise(Func<GdTask> factory) : IGdTaskSource
	{
		private Func<GdTask> _factory = factory;
		private GdTask _task;
		private Awaiter _awaiter;

		public void GetResult(short token)
		{
			_awaiter.GetResult();
		}

		public GdTaskStatus GetStatus(short token)
		{
			var f = Interlocked.Exchange(ref _factory, null);
			if (f != null)
			{
				_task = f();
				_awaiter = _task.GetAwaiter();
			}

			return _task.Status;
		}

		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			_awaiter.SourceOnCompleted(continuation, state);
		}

		public GdTaskStatus UnsafeGetStatus()
		{
			return _task.Status;
		}
	}

	private sealed class DeferPromise<T>(Func<GdTask<T>> factory) : IGdTaskSource<T>
	{
		private Func<GdTask<T>> _factory = factory;
		private GdTask<T> _task;
		private GdTask<T>.Awaiter _awaiter;

		public T GetResult(short token)
		{
			return _awaiter.GetResult();
		}

		void IGdTaskSource.GetResult(short token)
		{
			_awaiter.GetResult();
		}

		public GdTaskStatus GetStatus(short token)
		{
			var f = Interlocked.Exchange(ref _factory, null);
			if (f != null)
			{
				_task = f();
				_awaiter = _task.GetAwaiter();
			}

			return _task.Status;
		}

		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			_awaiter.SourceOnCompleted(continuation, state);
		}

		public GdTaskStatus UnsafeGetStatus()
		{
			return _task.Status;
		}
	}

	private sealed class NeverPromise<T> : IGdTaskSource<T>
	{
		private static readonly Action<object> cancellationCallback = CancellationCallback;

		private CancellationToken _cancellationToken;
		private GdTaskCompletionSourceCore<T> _core;

		public NeverPromise(CancellationToken cancellationToken)
		{
			_cancellationToken = cancellationToken;
			if (_cancellationToken.CanBeCanceled)
			{
				_cancellationToken.RegisterWithoutCaptureExecutionContext(cancellationCallback, this);
			}
		}

		private static void CancellationCallback(object state)
		{
			var self = (NeverPromise<T>)state;
			self._core.TrySetCanceled(self._cancellationToken);
		}

		public T GetResult(short token)
		{
			return _core.GetResult(token);
		}

		public GdTaskStatus GetStatus(short token)
		{
			return _core.GetStatus(token);
		}

		public GdTaskStatus UnsafeGetStatus()
		{
			return _core.UnsafeGetStatus();
		}

		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			_core.OnCompleted(continuation, state, token);
		}

		void IGdTaskSource.GetResult(short token)
		{
			_core.GetResult(token);
		}
	}
}

internal static class CompletedTasks
{
	public static readonly GdTask<AsyncUnit> AsyncUnit = GdTask.FromResult(Fractural.Tasks.AsyncUnit.Default);
	public static readonly GdTask<bool> True = GdTask.FromResult(true);
	public static readonly GdTask<bool> False = GdTask.FromResult(false);
	public static readonly GdTask<int> Zero = GdTask.FromResult(0);
	public static readonly GdTask<int> MinusOne = GdTask.FromResult(-1);
	public static readonly GdTask<int> One = GdTask.FromResult(1);
}