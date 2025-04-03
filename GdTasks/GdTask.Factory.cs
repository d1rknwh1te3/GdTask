using GdTasks.Extensions;
using System.Runtime.ExceptionServices;

namespace GdTasks;

public partial struct GdTask
{
	public static readonly GdTask CompletedTask = new();

	private static readonly GdTask CanceledGDTask
			= new Func<GdTask>(() => new GdTask(new CanceledResultSource(CancellationToken.None), 0))();

	/// <summary>
	/// helper of create add GDTaskVoid to delegate.
	/// For example: FooAction = GDTask.Action(async () => { /* */ })
	/// </summary>
	public static Action Action(Func<GdTaskVoid> asyncAction)
		=> () => asyncAction().Forget();

	/// <summary>
	/// helper of create add GDTaskVoid to delegate.
	/// </summary>
	public static Action Action(Func<CancellationToken, GdTaskVoid> asyncAction, CancellationToken cancellationToken)
		=> () => asyncAction(cancellationToken).Forget();

	public static GdTask Create(Func<GdTask> factory) => factory();

	public static GdTask<T> Create<T>(Func<GdTask<T>> factory) => factory();

	/// <summary>
	/// Defer the task creation just before call await.
	/// </summary>
	public static GdTask Defer(Func<GdTask> factory) => new(new DeferPromise(factory), 0);

	/// <summary>
	/// Defer the task creation just before call await.
	/// </summary>
	public static GdTask<T> Defer<T>(Func<GdTask<T>> factory) => new(new DeferPromise<T>(factory), 0);

	public static GdTask FromCanceled(CancellationToken cancellationToken = default)
	{
		return cancellationToken == CancellationToken.None
			? CanceledGDTask
			: new GdTask(new CanceledResultSource(cancellationToken), 0);
	}

	public static GdTask<T> FromCanceled<T>(CancellationToken cancellationToken = default)
	{
		return cancellationToken == CancellationToken.None
			? CanceledGdTaskCache<T>.Task
			: new GdTask<T>(new CanceledResultSource<T>(cancellationToken), 0);
	}

	public static GdTask FromException(Exception ex)
	{
		return ex is OperationCanceledException oce
			? FromCanceled(oce.CancellationToken)
			: new GdTask(new ExceptionResultSource(ex), 0);
	}

	public static GdTask<T> FromException<T>(Exception ex)
	{
		return ex is OperationCanceledException oce
			? FromCanceled<T>(oce.CancellationToken)
			: new GdTask<T>(new ExceptionResultSource<T>(ex), 0);
	}

	public static GdTask<T> FromResult<T>(T value) => new(value);

	public static AsyncLazy Lazy(Func<GdTask> factory) => new(factory);

	public static AsyncLazy<T> Lazy<T>(Func<GdTask<T>> factory) => new(factory);

	/// <summary>
	/// Never complete.
	/// </summary>
	public static GdTask Never(CancellationToken cancellationToken)
		=> new GdTask<AsyncUnit>(new NeverPromise<AsyncUnit>(cancellationToken), 0);

	/// <summary>
	/// Never complete.
	/// </summary>
	public static GdTask<T> Never<T>(CancellationToken cancellationToken)
		=> new(new NeverPromise<T>(cancellationToken), 0);

	/// <summary>
	/// helper of fire and forget void action.
	/// </summary>
	public static void Void(Func<GdTaskVoid> asyncAction) => asyncAction().Forget();

	/// <summary>
	/// helper of fire and forget void action.
	/// </summary>
	public static void Void(Func<CancellationToken, GdTaskVoid> asyncAction, CancellationToken cancellationToken)
		=> asyncAction(cancellationToken).Forget();

	/// <summary>
	/// helper of fire and forget void action.
	/// </summary>
	public static void Void<T>(Func<T, GdTaskVoid> asyncAction, T state) => asyncAction(state).Forget();

	private static class CanceledGdTaskCache<T>
	{
		public static readonly GdTask<T> Task;

		static CanceledGdTaskCache() => Task = new GdTask<T>(new CanceledResultSource<T>(CancellationToken.None), 0);
	}
	private sealed class CanceledResultSource(CancellationToken cancellationToken) : IGdTaskSource
	{
		public void GetResult(short token) => throw new OperationCanceledException(cancellationToken);

		public GdTaskStatus GetStatus(short token) => GdTaskStatus.Canceled;

		public void OnCompleted(Action<object> continuation, object state, short token) => continuation(state);

		public GdTaskStatus UnsafeGetStatus() => GdTaskStatus.Canceled;
	}

	private sealed class CanceledResultSource<T>(CancellationToken cancellationToken) : IGdTaskSource<T>
	{
		public T GetResult(short token) => throw new OperationCanceledException(cancellationToken);

		void IGdTaskSource.GetResult(short token) => throw new OperationCanceledException(cancellationToken);

		public GdTaskStatus GetStatus(short token) => GdTaskStatus.Canceled;

		public void OnCompleted(Action<object> continuation, object state, short token) => continuation(state);

		public GdTaskStatus UnsafeGetStatus() => GdTaskStatus.Canceled;
	}

	private sealed class DeferPromise(Func<GdTask> factory) : IGdTaskSource
	{
		private Awaiter _awaiter;
		private Func<GdTask> _factory = factory;
		private GdTask _task;
		public void GetResult(short token) => _awaiter.GetResult();

		public GdTaskStatus GetStatus(short token)
		{
			var f = Interlocked.Exchange(ref _factory, null);

			if (f == null)
				return _task.Status;

			_task = f();
			_awaiter = _task.GetAwaiter();

			return _task.Status;
		}

		public void OnCompleted(Action<object> continuation, object state, short token)
			=> _awaiter.SourceOnCompleted(continuation, state);

		public GdTaskStatus UnsafeGetStatus() => _task.Status;
	}

	private sealed class DeferPromise<T>(Func<GdTask<T>> factory) : IGdTaskSource<T>
	{
		private GdTask<T>.Awaiter _awaiter;
		private Func<GdTask<T>> _factory = factory;
		private GdTask<T> _task;
		public T GetResult(short token) => _awaiter.GetResult();

		void IGdTaskSource.GetResult(short token) => _awaiter.GetResult();

		public GdTaskStatus GetStatus(short token)
		{
			var f = Interlocked.Exchange(ref _factory, null);

			if (f == null)
				return _task.Status;

			_task = f();
			_awaiter = _task.GetAwaiter();

			return _task.Status;
		}

		public void OnCompleted(Action<object> continuation, object state, short token) => _awaiter.SourceOnCompleted(continuation, state);

		public GdTaskStatus UnsafeGetStatus() => _task.Status;
	}

	private sealed class ExceptionResultSource(Exception exception) : IGdTaskSource
	{
		private readonly ExceptionDispatchInfo _exception = ExceptionDispatchInfo.Capture(exception);
		private bool _calledGet;

		~ExceptionResultSource()
		{
			if (!_calledGet)
				GdTaskScheduler.PublishUnobservedTaskException(_exception.SourceException);
		}

		public void GetResult(short token)
		{
			if (!_calledGet)
			{
				_calledGet = true;
				GC.SuppressFinalize(this);
			}
			_exception.Throw();
		}

		public GdTaskStatus GetStatus(short token) => GdTaskStatus.Faulted;

		public void OnCompleted(Action<object> continuation, object state, short token) => continuation(state);

		public GdTaskStatus UnsafeGetStatus() => GdTaskStatus.Faulted;
	}

	private sealed class ExceptionResultSource<T>(Exception exception) : IGdTaskSource<T>
	{
		private readonly ExceptionDispatchInfo _exception = ExceptionDispatchInfo.Capture(exception);
		private bool _calledGet;

		~ExceptionResultSource()
		{
			if (!_calledGet)
				GdTaskScheduler.PublishUnobservedTaskException(_exception.SourceException);
		}

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

		public GdTaskStatus GetStatus(short token) => GdTaskStatus.Faulted;

		public void OnCompleted(Action<object> continuation, object state, short token) => continuation(state);

		public GdTaskStatus UnsafeGetStatus() => GdTaskStatus.Faulted;
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
				_cancellationToken.RegisterWithoutCaptureExecutionContext(cancellationCallback, this);
		}

		public T GetResult(short token) => _core.GetResult(token);

		void IGdTaskSource.GetResult(short token) => _core.GetResult(token);

		public GdTaskStatus GetStatus(short token) => _core.GetStatus(token);

		public void OnCompleted(Action<object> continuation, object state, short token) => _core.OnCompleted(continuation, state, token);

		public GdTaskStatus UnsafeGetStatus() => _core.UnsafeGetStatus();

		private static void CancellationCallback(object state)
		{
			var self = (NeverPromise<T>)state;
			self._core.TrySetCanceled(self._cancellationToken);
		}
	}
}