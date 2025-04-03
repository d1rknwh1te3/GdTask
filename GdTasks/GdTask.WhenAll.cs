namespace GdTasks;

public partial struct GdTask
{
	public static GdTask<T[]> WhenAll<T>(params GdTask<T>[] tasks)
	{
		return tasks.Length == 0
			? FromResult(Array.Empty<T>())
			: new GdTask<T[]>(new WhenAllPromise<T>(tasks, tasks.Length), 0);
	}

	public static GdTask<T[]> WhenAll<T>(IEnumerable<GdTask<T>> tasks)
	{
		using var span = ArrayPoolUtil.Materialize(tasks);
		var promise = new WhenAllPromise<T>(span.Array, span.Length); // consumed array in constructor.

		return new GdTask<T[]>(promise, 0);
	}

	public static GdTask WhenAll(params GdTask[] tasks)
	{
		return tasks.Length == 0
			? CompletedTask
			: new GdTask(new WhenAllPromise(tasks, tasks.Length), 0);
	}

	public static GdTask WhenAll(IEnumerable<GdTask> tasks)
	{
		using var span = ArrayPoolUtil.Materialize(tasks);
		var promise = new WhenAllPromise(span.Array, span.Length); // consumed array in constructor.
		return new GdTask(promise, 0);
	}

	private sealed class WhenAllPromise<T> : IGdTaskSource<T[]>
	{
		private int _completeCount;
		private GdTaskCompletionSourceCore<T[]> _core;
		private T[] _result;
		// don't reset(called after GetResult, will invoke TrySetException.)

		public WhenAllPromise(GdTask<T>[] tasks, int tasksLength)
		{
			TaskTracker.TrackActiveTask(this, 3);

			_completeCount = 0;

			if (tasksLength == 0)
			{
				_result = [];
				_core.TrySetResult(_result);
				return;
			}

			_result = new T[tasksLength];

			for (var i = 0; i < tasksLength; i++)
			{
				GdTask<T>.Awaiter awaiter;
				try
				{
					awaiter = tasks[i].GetAwaiter();
				}
				catch (Exception ex)
				{
					_core.TrySetException(ex);
					continue;
				}

				if (awaiter.IsCompleted)
				{
					TryInvokeContinuation(this, awaiter, i);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using var t = (StateTuple<WhenAllPromise<T>, GdTask<T>.Awaiter, int>)state;
						TryInvokeContinuation(t.Item1, t.Item2, t.Item3);
					}, StateTuple.Create(this, awaiter, i));
				}
			}
		}

		public T[] GetResult(short token)
		{
			TaskTracker.RemoveTracking(this);
			GC.SuppressFinalize(this);
			return _core.GetResult(token);
		}

		void IGdTaskSource.GetResult(short token)
		{
			GetResult(token);
		}

		public GdTaskStatus GetStatus(short token)
		{
			return _core.GetStatus(token);
		}

		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			_core.OnCompleted(continuation, state, token);
		}

		public GdTaskStatus UnsafeGetStatus()
		{
			return _core.UnsafeGetStatus();
		}

		private static void TryInvokeContinuation(WhenAllPromise<T> self, in GdTask<T>.Awaiter awaiter, int i)
		{
			try
			{
				self._result[i] = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self._core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self._completeCount) == self._result.Length)
			{
				self._core.TrySetResult(self._result);
			}
		}
	}

	private sealed class WhenAllPromise : IGdTaskSource
	{
		private int _completeCount;
		private GdTaskCompletionSourceCore<AsyncUnit> _core;
		private int _tasksLength;
		// don't reset(called after GetResult, will invoke TrySetException.)

		public WhenAllPromise(GdTask[] tasks, int tasksLength)
		{
			TaskTracker.TrackActiveTask(this, 3);

			_tasksLength = tasksLength;
			_completeCount = 0;

			if (tasksLength == 0)
			{
				_core.TrySetResult(AsyncUnit.Default);
				return;
			}

			for (var i = 0; i < tasksLength; i++)
			{
				Awaiter awaiter;
				try
				{
					awaiter = tasks[i].GetAwaiter();
				}
				catch (Exception ex)
				{
					_core.TrySetException(ex);
					continue;
				}

				if (awaiter.IsCompleted)
				{
					TryInvokeContinuation(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using var t = (StateTuple<WhenAllPromise, Awaiter>)state;
						TryInvokeContinuation(t.Item1, t.Item2);
					}, StateTuple.Create(this, awaiter));
				}
			}
		}

		public void GetResult(short token)
		{
			TaskTracker.RemoveTracking(this);
			GC.SuppressFinalize(this);
			_core.GetResult(token);
		}

		public GdTaskStatus GetStatus(short token)
		{
			return _core.GetStatus(token);
		}

		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			_core.OnCompleted(continuation, state, token);
		}

		public GdTaskStatus UnsafeGetStatus()
		{
			return _core.UnsafeGetStatus();
		}

		private static void TryInvokeContinuation(WhenAllPromise self, in Awaiter awaiter)
		{
			try
			{
				awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self._core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self._completeCount) == self._tasksLength)
			{
				self._core.TrySetResult(AsyncUnit.Default);
			}
		}
	}
}