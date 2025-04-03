﻿namespace GdTasks;

public partial struct GdTask
{
	public static GdTask<(bool hasResultLeft, T result)> WhenAny<T>(GdTask<T> leftTask, GdTask rightTask)
	{
		return new GdTask<(bool, T)>(new WhenAnyLrPromise<T>(leftTask, rightTask), 0);
	}

	public static GdTask<(int winArgumentIndex, T result)> WhenAny<T>(params GdTask<T>[] tasks)
	{
		return new GdTask<(int, T)>(new WhenAnyPromise<T>(tasks, tasks.Length), 0);
	}

	public static GdTask<(int winArgumentIndex, T result)> WhenAny<T>(IEnumerable<GdTask<T>> tasks)
	{
		using var span = ArrayPoolUtil.Materialize(tasks);
		return new GdTask<(int, T)>(new WhenAnyPromise<T>(span.Array, span.Length), 0);
	}

	/// <summary>Return value is winArgumentIndex</summary>
	public static GdTask<int> WhenAny(params GdTask[] tasks)
	{
		return new GdTask<int>(new WhenAnyPromise(tasks, tasks.Length), 0);
	}

	/// <summary>Return value is winArgumentIndex</summary>
	public static GdTask<int> WhenAny(IEnumerable<GdTask> tasks)
	{
		using var span = ArrayPoolUtil.Materialize(tasks);
		return new GdTask<int>(new WhenAnyPromise(span.Array, span.Length), 0);
	}

	private sealed class WhenAnyLrPromise<T> : IGdTaskSource<(bool, T)>
	{
		private int _completedCount;
		private GdTaskCompletionSourceCore<(bool, T)> _core;

		public WhenAnyLrPromise(GdTask<T> leftTask, GdTask rightTask)
		{
			TaskTracker.TrackActiveTask(this, 3);

			{
				GdTask<T>.Awaiter awaiter;
				try
				{
					awaiter = leftTask.GetAwaiter();
				}
				catch (Exception ex)
				{
					_core.TrySetException(ex);
					goto RIGHT;
				}

				if (awaiter.IsCompleted)
				{
					TryLeftInvokeContinuation(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using var t = (StateTuple<WhenAnyLrPromise<T>, GdTask<T>.Awaiter>)state;
						TryLeftInvokeContinuation(t.Item1, t.Item2);
					}, StateTuple.Create(this, awaiter));
				}
			}
		RIGHT:
			{
				Awaiter awaiter;
				try
				{
					awaiter = rightTask.GetAwaiter();
				}
				catch (Exception ex)
				{
					_core.TrySetException(ex);
					return;
				}

				if (awaiter.IsCompleted)
				{
					TryRightInvokeContinuation(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using var t = (StateTuple<WhenAnyLrPromise<T>, Awaiter>)state;
						TryRightInvokeContinuation(t.Item1, t.Item2);
					}, StateTuple.Create(this, awaiter));
				}
			}
		}

		public (bool, T) GetResult(short token)
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

		private static void TryLeftInvokeContinuation(WhenAnyLrPromise<T> self, in GdTask<T>.Awaiter awaiter)
		{
			T result;
			try
			{
				result = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self._core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self._completedCount) == 1)
			{
				self._core.TrySetResult((true, result));
			}
		}

		private static void TryRightInvokeContinuation(WhenAnyLrPromise<T> self, in Awaiter awaiter)
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

			if (Interlocked.Increment(ref self._completedCount) == 1)
			{
				self._core.TrySetResult((false, default));
			}
		}
	}

	private sealed class WhenAnyPromise<T> : IGdTaskSource<(int, T)>
	{
		private int _completedCount;
		private GdTaskCompletionSourceCore<(int, T)> _core;

		public WhenAnyPromise(GdTask<T>[] tasks, int tasksLength)
		{
			if (tasksLength == 0)
			{
				throw new ArgumentException("The tasks argument contains no tasks.");
			}

			TaskTracker.TrackActiveTask(this, 3);

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
					continue; // consume others.
				}

				if (awaiter.IsCompleted)
				{
					TryInvokeContinuation(this, awaiter, i);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using var t = (StateTuple<WhenAnyPromise<T>, GdTask<T>.Awaiter, int>)state;
						TryInvokeContinuation(t.Item1, t.Item2, t.Item3);
					}, StateTuple.Create(this, awaiter, i));
				}
			}
		}

		public (int, T) GetResult(short token)
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

		private static void TryInvokeContinuation(WhenAnyPromise<T> self, in GdTask<T>.Awaiter awaiter, int i)
		{
			T result;
			try
			{
				result = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self._core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self._completedCount) == 1)
			{
				self._core.TrySetResult((i, result));
			}
		}
	}

	private sealed class WhenAnyPromise : IGdTaskSource<int>
	{
		private int _completedCount;
		private GdTaskCompletionSourceCore<int> _core;

		public WhenAnyPromise(GdTask[] tasks, int tasksLength)
		{
			if (tasksLength == 0)
			{
				throw new ArgumentException("The tasks argument contains no tasks.");
			}

			TaskTracker.TrackActiveTask(this, 3);

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
					continue; // consume others.
				}

				if (awaiter.IsCompleted)
				{
					TryInvokeContinuation(this, awaiter, i);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using var t = (StateTuple<WhenAnyPromise, Awaiter, int>)state;
						TryInvokeContinuation(t.Item1, t.Item2, t.Item3);
					}, StateTuple.Create(this, awaiter, i));
				}
			}
		}

		public int GetResult(short token)
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

		private static void TryInvokeContinuation(WhenAnyPromise self, in Awaiter awaiter, int i)
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

			if (Interlocked.Increment(ref self._completedCount) == 1)
			{
				self._core.TrySetResult(i);
			}
		}
	}
}