namespace GdTasks.Models;

public partial class AsyncLazy
{
	private static Action<object> _continuation = SetCompletionSource;

	private GdTask.Awaiter _awaiter;
	private GdTaskCompletionSource _completionSource;
	private bool _initialized;
	private object _syncLock;
	private Func<GdTask> _taskFactory;
	public AsyncLazy(Func<GdTask> taskFactory)
	{
		_taskFactory = taskFactory;
		_completionSource = new GdTaskCompletionSource();
		_syncLock = new object();
		_initialized = false;
	}

	internal AsyncLazy(GdTask task)
	{
		_taskFactory = null;
		_completionSource = new GdTaskCompletionSource();
		_syncLock = null;
		_initialized = true;

		var awaiter = task.GetAwaiter();

		if (awaiter.IsCompleted)
		{
			SetCompletionSource(awaiter);
		}
		else
		{
			_awaiter = awaiter;
			awaiter.SourceOnCompleted(_continuation, this);
		}
	}

	public GdTask Task
	{
		get
		{
			EnsureInitialized();
			return _completionSource.Task;
		}
	}

	public GdTask.Awaiter GetAwaiter() => Task.GetAwaiter();

	private static void SetCompletionSource(object state)
	{
		var self = (AsyncLazy)state;
		try
		{
			self._awaiter.GetResult();
			self._completionSource.TrySetResult();
		}
		catch (Exception ex)
		{
			self._completionSource.TrySetException(ex);
		}
		finally
		{
			self._awaiter = default;
		}
	}

	private void EnsureInitialized()
	{
		if (Volatile.Read(ref _initialized))
			return;

		EnsureInitializedCore();
	}

	private void EnsureInitializedCore()
	{
		lock (_syncLock)
		{
			if (Volatile.Read(ref _initialized))
				return;

			var f = Interlocked.Exchange(ref _taskFactory, null);

			if (f == null)
				return;

			var task = f();
			var awaiter = task.GetAwaiter();

			if (awaiter.IsCompleted)
			{
				SetCompletionSource(awaiter);
			}
			else
			{
				_awaiter = awaiter;
				awaiter.SourceOnCompleted(_continuation, this);
			}

			Volatile.Write(ref _initialized, true);
		}
	}

	private void SetCompletionSource(in GdTask.Awaiter awaiter)
	{
		try
		{
			awaiter.GetResult();
			_completionSource.TrySetResult();
		}
		catch (Exception ex)
		{
			_completionSource.TrySetException(ex);
		}
	}
}

public partial class AsyncLazy<T>
{
	private static Action<object> _continuation = SetCompletionSource;

	private GdTask<T>.Awaiter _awaiter;
	private GdTaskCompletionSource<T> _completionSource;
	private bool _initialized;
	private object _syncLock;
	private Func<GdTask<T>> _taskFactory;
	public AsyncLazy(Func<GdTask<T>> taskFactory)
	{
		_taskFactory = taskFactory;
		_completionSource = new GdTaskCompletionSource<T>();
		_syncLock = new object();
		_initialized = false;
	}

	internal AsyncLazy(GdTask<T> task)
	{
		_taskFactory = null;
		_completionSource = new GdTaskCompletionSource<T>();
		_syncLock = null;
		_initialized = true;

		var awaiter = task.GetAwaiter();
		if (awaiter.IsCompleted)
		{
			SetCompletionSource(awaiter);
		}
		else
		{
			_awaiter = awaiter;
			awaiter.SourceOnCompleted(_continuation, this);
		}
	}

	public GdTask<T> Task
	{
		get
		{
			EnsureInitialized();
			return _completionSource.Task;
		}
	}

	public GdTask<T>.Awaiter GetAwaiter() => Task.GetAwaiter();

	private static void SetCompletionSource(object state)
	{
		var self = (AsyncLazy<T>)state;
		try
		{
			var result = self._awaiter.GetResult();
			self._completionSource.TrySetResult(result);
		}
		catch (Exception ex)
		{
			self._completionSource.TrySetException(ex);
		}
		finally
		{
			self._awaiter = default;
		}
	}

	private void EnsureInitialized()
	{
		if (Volatile.Read(ref _initialized))
			return;

		EnsureInitializedCore();
	}

	private void EnsureInitializedCore()
	{
		lock (_syncLock)
		{
			if (Volatile.Read(ref _initialized))
				return;

			var f = Interlocked.Exchange(ref _taskFactory, null);

			if (f == null)
				return;

			var task = f();
			var awaiter = task.GetAwaiter();
			if (awaiter.IsCompleted)
			{
				SetCompletionSource(awaiter);
			}
			else
			{
				_awaiter = awaiter;
				awaiter.SourceOnCompleted(_continuation, this);
			}

			Volatile.Write(ref _initialized, true);
		}
	}

	private void SetCompletionSource(in GdTask<T>.Awaiter awaiter)
	{
		try
		{
			var result = awaiter.GetResult();
			_completionSource.TrySetResult(result);
		}
		catch (Exception ex)
		{
			_completionSource.TrySetException(ex);
		}
	}
}