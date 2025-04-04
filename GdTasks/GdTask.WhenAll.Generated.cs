﻿using GdTasks.Enums;
using GdTasks.Interfaces.Tasks;
using GdTasks.Internal;

namespace GdTasks;

public partial struct GdTask
{
	public static GdTask<(T1, T2)> WhenAll<T1, T2>(GdTask<T1> task1, GdTask<T2> task2)
	{
		return task1.Status.IsCompletedSuccessfully() && task2.Status.IsCompletedSuccessfully()
			? new GdTask<(T1, T2)>((task1.GetAwaiter().GetResult(), task2.GetAwaiter().GetResult()))
			: new GdTask<(T1, T2)>(new WhenAllPromise<T1, T2>(task1, task2), 0);
	}

	public static GdTask<(T1, T2, T3)> WhenAll<T1, T2, T3>(GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3)
	{
		return task1.Status.IsCompletedSuccessfully() && task2.Status.IsCompletedSuccessfully() && task3.Status.IsCompletedSuccessfully()
			? new GdTask<(T1, T2, T3)>((task1.GetAwaiter().GetResult(), task2.GetAwaiter().GetResult(), task3.GetAwaiter().GetResult()))
			: new GdTask<(T1, T2, T3)>(new WhenAllPromise<T1, T2, T3>(task1, task2, task3), 0);
	}

	public static GdTask<(T1, T2, T3, T4)> WhenAll<T1, T2, T3, T4>(GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3, GdTask<T4> task4)
	{
		return task1.Status.IsCompletedSuccessfully() && task2.Status.IsCompletedSuccessfully() && task3.Status.IsCompletedSuccessfully() && task4.Status.IsCompletedSuccessfully()
			? new GdTask<(T1, T2, T3, T4)>((task1.GetAwaiter().GetResult(), task2.GetAwaiter().GetResult(), task3.GetAwaiter().GetResult(), task4.GetAwaiter().GetResult()))
			: new GdTask<(T1, T2, T3, T4)>(new WhenAllPromise<T1, T2, T3, T4>(task1, task2, task3, task4), 0);
	}

	public static GdTask<(T1, T2, T3, T4, T5)> WhenAll<T1, T2, T3, T4, T5>(GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3, GdTask<T4> task4, GdTask<T5> task5)
	{
		return task1.Status.IsCompletedSuccessfully() && task2.Status.IsCompletedSuccessfully() && task3.Status.IsCompletedSuccessfully() && task4.Status.IsCompletedSuccessfully() && task5.Status.IsCompletedSuccessfully()
			? new GdTask<(T1, T2, T3, T4, T5)>((task1.GetAwaiter().GetResult(), task2.GetAwaiter().GetResult(), task3.GetAwaiter().GetResult(), task4.GetAwaiter().GetResult(), task5.GetAwaiter().GetResult()))
			: new GdTask<(T1, T2, T3, T4, T5)>(new WhenAllPromise<T1, T2, T3, T4, T5>(task1, task2, task3, task4, task5), 0);
	}

	public static GdTask<(T1, T2, T3, T4, T5, T6)> WhenAll<T1, T2, T3, T4, T5, T6>(GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3, GdTask<T4> task4, GdTask<T5> task5, GdTask<T6> task6)
	{
		if (task1.Status.IsCompletedSuccessfully() && task2.Status.IsCompletedSuccessfully() && task3.Status.IsCompletedSuccessfully() && task4.Status.IsCompletedSuccessfully() && task5.Status.IsCompletedSuccessfully() && task6.Status.IsCompletedSuccessfully())
		{
			return new GdTask<(T1, T2, T3, T4, T5, T6)>((task1.GetAwaiter().GetResult(), task2.GetAwaiter().GetResult(), task3.GetAwaiter().GetResult(), task4.GetAwaiter().GetResult(), task5.GetAwaiter().GetResult(), task6.GetAwaiter().GetResult()));
		}

		return new GdTask<(T1, T2, T3, T4, T5, T6)>(new WhenAllPromise<T1, T2, T3, T4, T5, T6>(task1, task2, task3, task4, task5, task6), 0);
	}

	public static GdTask<(T1, T2, T3, T4, T5, T6, T7)> WhenAll<T1, T2, T3, T4, T5, T6, T7>(GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3, GdTask<T4> task4, GdTask<T5> task5, GdTask<T6> task6, GdTask<T7> task7)
	{
		if (task1.Status.IsCompletedSuccessfully() && task2.Status.IsCompletedSuccessfully() && task3.Status.IsCompletedSuccessfully() && task4.Status.IsCompletedSuccessfully() && task5.Status.IsCompletedSuccessfully() && task6.Status.IsCompletedSuccessfully() && task7.Status.IsCompletedSuccessfully())
		{
			return new GdTask<(T1, T2, T3, T4, T5, T6, T7)>((task1.GetAwaiter().GetResult(), task2.GetAwaiter().GetResult(), task3.GetAwaiter().GetResult(), task4.GetAwaiter().GetResult(), task5.GetAwaiter().GetResult(), task6.GetAwaiter().GetResult(), task7.GetAwaiter().GetResult()));
		}

		return new GdTask<(T1, T2, T3, T4, T5, T6, T7)>(new WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>(task1, task2, task3, task4, task5, task6, task7), 0);
	}

	public static GdTask<(T1, T2, T3, T4, T5, T6, T7, T8)> WhenAll<T1, T2, T3, T4, T5, T6, T7, T8>(GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3, GdTask<T4> task4, GdTask<T5> task5, GdTask<T6> task6, GdTask<T7> task7, GdTask<T8> task8)
	{
		if (task1.Status.IsCompletedSuccessfully() && task2.Status.IsCompletedSuccessfully() && task3.Status.IsCompletedSuccessfully() && task4.Status.IsCompletedSuccessfully() && task5.Status.IsCompletedSuccessfully() && task6.Status.IsCompletedSuccessfully() && task7.Status.IsCompletedSuccessfully() && task8.Status.IsCompletedSuccessfully())
		{
			return new GdTask<(T1, T2, T3, T4, T5, T6, T7, T8)>((task1.GetAwaiter().GetResult(), task2.GetAwaiter().GetResult(), task3.GetAwaiter().GetResult(), task4.GetAwaiter().GetResult(), task5.GetAwaiter().GetResult(), task6.GetAwaiter().GetResult(), task7.GetAwaiter().GetResult(), task8.GetAwaiter().GetResult()));
		}

		return new GdTask<(T1, T2, T3, T4, T5, T6, T7, T8)>(new WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>(task1, task2, task3, task4, task5, task6, task7, task8), 0);
	}

	public static GdTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> WhenAll<T1, T2, T3, T4, T5, T6, T7, T8, T9>(GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3, GdTask<T4> task4, GdTask<T5> task5, GdTask<T6> task6, GdTask<T7> task7, GdTask<T8> task8, GdTask<T9> task9)
	{
		if (task1.Status.IsCompletedSuccessfully() && task2.Status.IsCompletedSuccessfully() && task3.Status.IsCompletedSuccessfully() && task4.Status.IsCompletedSuccessfully() && task5.Status.IsCompletedSuccessfully() && task6.Status.IsCompletedSuccessfully() && task7.Status.IsCompletedSuccessfully() && task8.Status.IsCompletedSuccessfully() && task9.Status.IsCompletedSuccessfully())
		{
			return new GdTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9)>((task1.GetAwaiter().GetResult(), task2.GetAwaiter().GetResult(), task3.GetAwaiter().GetResult(), task4.GetAwaiter().GetResult(), task5.GetAwaiter().GetResult(), task6.GetAwaiter().GetResult(), task7.GetAwaiter().GetResult(), task8.GetAwaiter().GetResult(), task9.GetAwaiter().GetResult()));
		}

		return new GdTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9)>(new WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>(task1, task2, task3, task4, task5, task6, task7, task8, task9), 0);
	}

	public static GdTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> WhenAll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3, GdTask<T4> task4, GdTask<T5> task5, GdTask<T6> task6, GdTask<T7> task7, GdTask<T8> task8, GdTask<T9> task9, GdTask<T10> task10)
	{
		if (task1.Status.IsCompletedSuccessfully() && task2.Status.IsCompletedSuccessfully() && task3.Status.IsCompletedSuccessfully() && task4.Status.IsCompletedSuccessfully() && task5.Status.IsCompletedSuccessfully() && task6.Status.IsCompletedSuccessfully() && task7.Status.IsCompletedSuccessfully() && task8.Status.IsCompletedSuccessfully() && task9.Status.IsCompletedSuccessfully() && task10.Status.IsCompletedSuccessfully())
		{
			return new GdTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)>((task1.GetAwaiter().GetResult(), task2.GetAwaiter().GetResult(), task3.GetAwaiter().GetResult(), task4.GetAwaiter().GetResult(), task5.GetAwaiter().GetResult(), task6.GetAwaiter().GetResult(), task7.GetAwaiter().GetResult(), task8.GetAwaiter().GetResult(), task9.GetAwaiter().GetResult(), task10.GetAwaiter().GetResult()));
		}

		return new GdTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)>(new WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(task1, task2, task3, task4, task5, task6, task7, task8, task9, task10), 0);
	}

	public static GdTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> WhenAll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3, GdTask<T4> task4, GdTask<T5> task5, GdTask<T6> task6, GdTask<T7> task7, GdTask<T8> task8, GdTask<T9> task9, GdTask<T10> task10, GdTask<T11> task11)
	{
		if (task1.Status.IsCompletedSuccessfully() && task2.Status.IsCompletedSuccessfully() && task3.Status.IsCompletedSuccessfully() && task4.Status.IsCompletedSuccessfully() && task5.Status.IsCompletedSuccessfully() && task6.Status.IsCompletedSuccessfully() && task7.Status.IsCompletedSuccessfully() && task8.Status.IsCompletedSuccessfully() && task9.Status.IsCompletedSuccessfully() && task10.Status.IsCompletedSuccessfully() && task11.Status.IsCompletedSuccessfully())
		{
			return new GdTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)>((task1.GetAwaiter().GetResult(), task2.GetAwaiter().GetResult(), task3.GetAwaiter().GetResult(), task4.GetAwaiter().GetResult(), task5.GetAwaiter().GetResult(), task6.GetAwaiter().GetResult(), task7.GetAwaiter().GetResult(), task8.GetAwaiter().GetResult(), task9.GetAwaiter().GetResult(), task10.GetAwaiter().GetResult(), task11.GetAwaiter().GetResult()));
		}

		return new GdTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)>(new WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(task1, task2, task3, task4, task5, task6, task7, task8, task9, task10, task11), 0);
	}

	public static GdTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> WhenAll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3, GdTask<T4> task4, GdTask<T5> task5, GdTask<T6> task6, GdTask<T7> task7, GdTask<T8> task8, GdTask<T9> task9, GdTask<T10> task10, GdTask<T11> task11, GdTask<T12> task12)
	{
		if (task1.Status.IsCompletedSuccessfully() && task2.Status.IsCompletedSuccessfully() && task3.Status.IsCompletedSuccessfully() && task4.Status.IsCompletedSuccessfully() && task5.Status.IsCompletedSuccessfully() && task6.Status.IsCompletedSuccessfully() && task7.Status.IsCompletedSuccessfully() && task8.Status.IsCompletedSuccessfully() && task9.Status.IsCompletedSuccessfully() && task10.Status.IsCompletedSuccessfully() && task11.Status.IsCompletedSuccessfully() && task12.Status.IsCompletedSuccessfully())
		{
			return new GdTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)>((task1.GetAwaiter().GetResult(), task2.GetAwaiter().GetResult(), task3.GetAwaiter().GetResult(), task4.GetAwaiter().GetResult(), task5.GetAwaiter().GetResult(), task6.GetAwaiter().GetResult(), task7.GetAwaiter().GetResult(), task8.GetAwaiter().GetResult(), task9.GetAwaiter().GetResult(), task10.GetAwaiter().GetResult(), task11.GetAwaiter().GetResult(), task12.GetAwaiter().GetResult()));
		}

		return new GdTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)>(new WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(task1, task2, task3, task4, task5, task6, task7, task8, task9, task10, task11, task12), 0);
	}

	public static GdTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)> WhenAll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3, GdTask<T4> task4, GdTask<T5> task5, GdTask<T6> task6, GdTask<T7> task7, GdTask<T8> task8, GdTask<T9> task9, GdTask<T10> task10, GdTask<T11> task11, GdTask<T12> task12, GdTask<T13> task13)
	{
		if (task1.Status.IsCompletedSuccessfully() && task2.Status.IsCompletedSuccessfully() && task3.Status.IsCompletedSuccessfully() && task4.Status.IsCompletedSuccessfully() && task5.Status.IsCompletedSuccessfully() && task6.Status.IsCompletedSuccessfully() && task7.Status.IsCompletedSuccessfully() && task8.Status.IsCompletedSuccessfully() && task9.Status.IsCompletedSuccessfully() && task10.Status.IsCompletedSuccessfully() && task11.Status.IsCompletedSuccessfully() && task12.Status.IsCompletedSuccessfully() && task13.Status.IsCompletedSuccessfully())
		{
			return new GdTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)>((task1.GetAwaiter().GetResult(), task2.GetAwaiter().GetResult(), task3.GetAwaiter().GetResult(), task4.GetAwaiter().GetResult(), task5.GetAwaiter().GetResult(), task6.GetAwaiter().GetResult(), task7.GetAwaiter().GetResult(), task8.GetAwaiter().GetResult(), task9.GetAwaiter().GetResult(), task10.GetAwaiter().GetResult(), task11.GetAwaiter().GetResult(), task12.GetAwaiter().GetResult(), task13.GetAwaiter().GetResult()));
		}

		return new GdTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)>(new WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(task1, task2, task3, task4, task5, task6, task7, task8, task9, task10, task11, task12, task13), 0);
	}

	public static GdTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)> WhenAll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3, GdTask<T4> task4, GdTask<T5> task5, GdTask<T6> task6, GdTask<T7> task7, GdTask<T8> task8, GdTask<T9> task9, GdTask<T10> task10, GdTask<T11> task11, GdTask<T12> task12, GdTask<T13> task13, GdTask<T14> task14)
	{
		if (task1.Status.IsCompletedSuccessfully() && task2.Status.IsCompletedSuccessfully() && task3.Status.IsCompletedSuccessfully() && task4.Status.IsCompletedSuccessfully() && task5.Status.IsCompletedSuccessfully() && task6.Status.IsCompletedSuccessfully() && task7.Status.IsCompletedSuccessfully() && task8.Status.IsCompletedSuccessfully() && task9.Status.IsCompletedSuccessfully() && task10.Status.IsCompletedSuccessfully() && task11.Status.IsCompletedSuccessfully() && task12.Status.IsCompletedSuccessfully() && task13.Status.IsCompletedSuccessfully() && task14.Status.IsCompletedSuccessfully())
		{
			return new GdTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)>((task1.GetAwaiter().GetResult(), task2.GetAwaiter().GetResult(), task3.GetAwaiter().GetResult(), task4.GetAwaiter().GetResult(), task5.GetAwaiter().GetResult(), task6.GetAwaiter().GetResult(), task7.GetAwaiter().GetResult(), task8.GetAwaiter().GetResult(), task9.GetAwaiter().GetResult(), task10.GetAwaiter().GetResult(), task11.GetAwaiter().GetResult(), task12.GetAwaiter().GetResult(), task13.GetAwaiter().GetResult(), task14.GetAwaiter().GetResult()));
		}

		return new GdTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)>(new WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(task1, task2, task3, task4, task5, task6, task7, task8, task9, task10, task11, task12, task13, task14), 0);
	}

	public static GdTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)> WhenAll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3, GdTask<T4> task4, GdTask<T5> task5, GdTask<T6> task6, GdTask<T7> task7, GdTask<T8> task8, GdTask<T9> task9, GdTask<T10> task10, GdTask<T11> task11, GdTask<T12> task12, GdTask<T13> task13, GdTask<T14> task14, GdTask<T15> task15)
	{
		if (task1.Status.IsCompletedSuccessfully() && task2.Status.IsCompletedSuccessfully() && task3.Status.IsCompletedSuccessfully() && task4.Status.IsCompletedSuccessfully() && task5.Status.IsCompletedSuccessfully() && task6.Status.IsCompletedSuccessfully() && task7.Status.IsCompletedSuccessfully() && task8.Status.IsCompletedSuccessfully() && task9.Status.IsCompletedSuccessfully() && task10.Status.IsCompletedSuccessfully() && task11.Status.IsCompletedSuccessfully() && task12.Status.IsCompletedSuccessfully() && task13.Status.IsCompletedSuccessfully() && task14.Status.IsCompletedSuccessfully() && task15.Status.IsCompletedSuccessfully())
		{
			return new GdTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)>((task1.GetAwaiter().GetResult(), task2.GetAwaiter().GetResult(), task3.GetAwaiter().GetResult(), task4.GetAwaiter().GetResult(), task5.GetAwaiter().GetResult(), task6.GetAwaiter().GetResult(), task7.GetAwaiter().GetResult(), task8.GetAwaiter().GetResult(), task9.GetAwaiter().GetResult(), task10.GetAwaiter().GetResult(), task11.GetAwaiter().GetResult(), task12.GetAwaiter().GetResult(), task13.GetAwaiter().GetResult(), task14.GetAwaiter().GetResult(), task15.GetAwaiter().GetResult()));
		}

		return new GdTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)>(new WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(task1, task2, task3, task4, task5, task6, task7, task8, task9, task10, task11, task12, task13, task14, task15), 0);
	}

	private sealed class WhenAllPromise<T1, T2> : IGdTaskSource<(T1, T2)>
	{
		private int completedCount;
		private GdTaskCompletionSourceCore<(T1, T2)> core;
		private T1 t1 = default;
		private T2 t2 = default;
		public WhenAllPromise(GdTask<T1> task1, GdTask<T2> task2)
		{
			TaskTracker.TrackActiveTask(this, 3);

			this.completedCount = 0;
			{
				var awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2>, GdTask<T1>.Awaiter>)state)
						{
							TryInvokeContinuationT1(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task2.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT2(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2>, GdTask<T2>.Awaiter>)state)
						{
							TryInvokeContinuationT2(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
		}

		public (T1, T2) GetResult(short token)
		{
			TaskTracker.RemoveTracking(this);
			GC.SuppressFinalize(this);
			return core.GetResult(token);
		}

		void IGdTaskSource.GetResult(short token) => GetResult(token);

		public GdTaskStatus GetStatus(short token) => core.GetStatus(token);

		public void OnCompleted(Action<object> continuation, object state, short token) => core.OnCompleted(continuation, state, token);

		public GdTaskStatus UnsafeGetStatus() => core.UnsafeGetStatus();

		private static void TryInvokeContinuationT1(WhenAllPromise<T1, T2> self, in GdTask<T1>.Awaiter awaiter)
		{
			try
			{
				self.t1 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 2)
			{
				self.core.TrySetResult((self.t1, self.t2));
			}
		}

		private static void TryInvokeContinuationT2(WhenAllPromise<T1, T2> self, in GdTask<T2>.Awaiter awaiter)
		{
			try
			{
				self.t2 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 2)
			{
				self.core.TrySetResult((self.t1, self.t2));
			}
		}
	}
	private sealed class WhenAllPromise<T1, T2, T3> : IGdTaskSource<(T1, T2, T3)>
	{
		private int completedCount;
		private GdTaskCompletionSourceCore<(T1, T2, T3)> core;
		private T1 t1 = default;
		private T2 t2 = default;
		private T3 t3 = default;
		public WhenAllPromise(GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3)
		{
			TaskTracker.TrackActiveTask(this, 3);

			this.completedCount = 0;
			{
				var awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3>, GdTask<T1>.Awaiter>)state)
						{
							TryInvokeContinuationT1(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task2.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT2(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3>, GdTask<T2>.Awaiter>)state)
						{
							TryInvokeContinuationT2(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task3.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT3(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3>, GdTask<T3>.Awaiter>)state)
						{
							TryInvokeContinuationT3(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
		}

		public (T1, T2, T3) GetResult(short token)
		{
			TaskTracker.RemoveTracking(this);
			GC.SuppressFinalize(this);
			return core.GetResult(token);
		}

		void IGdTaskSource.GetResult(short token)
		{
			GetResult(token);
		}

		public GdTaskStatus GetStatus(short token)
		{
			return core.GetStatus(token);
		}

		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			core.OnCompleted(continuation, state, token);
		}

		public GdTaskStatus UnsafeGetStatus()
		{
			return core.UnsafeGetStatus();
		}

		private static void TryInvokeContinuationT1(WhenAllPromise<T1, T2, T3> self, in GdTask<T1>.Awaiter awaiter)
		{
			try
			{
				self.t1 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 3)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3));
			}
		}

		private static void TryInvokeContinuationT2(WhenAllPromise<T1, T2, T3> self, in GdTask<T2>.Awaiter awaiter)
		{
			try
			{
				self.t2 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 3)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3));
			}
		}

		private static void TryInvokeContinuationT3(WhenAllPromise<T1, T2, T3> self, in GdTask<T3>.Awaiter awaiter)
		{
			try
			{
				self.t3 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 3)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3));
			}
		}
	}
	private sealed class WhenAllPromise<T1, T2, T3, T4> : IGdTaskSource<(T1, T2, T3, T4)>
	{
		private int completedCount;
		private GdTaskCompletionSourceCore<(T1, T2, T3, T4)> core;
		private T1 t1 = default;
		private T2 t2 = default;
		private T3 t3 = default;
		private T4 t4 = default;
		public WhenAllPromise(GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3, GdTask<T4> task4)
		{
			TaskTracker.TrackActiveTask(this, 3);

			this.completedCount = 0;
			{
				var awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4>, GdTask<T1>.Awaiter>)state)
						{
							TryInvokeContinuationT1(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task2.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT2(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4>, GdTask<T2>.Awaiter>)state)
						{
							TryInvokeContinuationT2(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task3.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT3(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4>, GdTask<T3>.Awaiter>)state)
						{
							TryInvokeContinuationT3(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task4.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT4(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4>, GdTask<T4>.Awaiter>)state)
						{
							TryInvokeContinuationT4(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
		}

		public (T1, T2, T3, T4) GetResult(short token)
		{
			TaskTracker.RemoveTracking(this);
			GC.SuppressFinalize(this);
			return core.GetResult(token);
		}

		void IGdTaskSource.GetResult(short token)
		{
			GetResult(token);
		}

		public GdTaskStatus GetStatus(short token)
		{
			return core.GetStatus(token);
		}

		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			core.OnCompleted(continuation, state, token);
		}

		public GdTaskStatus UnsafeGetStatus()
		{
			return core.UnsafeGetStatus();
		}

		private static void TryInvokeContinuationT1(WhenAllPromise<T1, T2, T3, T4> self, in GdTask<T1>.Awaiter awaiter)
		{
			try
			{
				self.t1 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 4)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4));
			}
		}

		private static void TryInvokeContinuationT2(WhenAllPromise<T1, T2, T3, T4> self, in GdTask<T2>.Awaiter awaiter)
		{
			try
			{
				self.t2 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 4)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4));
			}
		}

		private static void TryInvokeContinuationT3(WhenAllPromise<T1, T2, T3, T4> self, in GdTask<T3>.Awaiter awaiter)
		{
			try
			{
				self.t3 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 4)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4));
			}
		}

		private static void TryInvokeContinuationT4(WhenAllPromise<T1, T2, T3, T4> self, in GdTask<T4>.Awaiter awaiter)
		{
			try
			{
				self.t4 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 4)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4));
			}
		}
	}
	private sealed class WhenAllPromise<T1, T2, T3, T4, T5> : IGdTaskSource<(T1, T2, T3, T4, T5)>
	{
		private int completedCount;
		private GdTaskCompletionSourceCore<(T1, T2, T3, T4, T5)> core;
		private T1 t1 = default;
		private T2 t2 = default;
		private T3 t3 = default;
		private T4 t4 = default;
		private T5 t5 = default;
		public WhenAllPromise(GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3, GdTask<T4> task4, GdTask<T5> task5)
		{
			TaskTracker.TrackActiveTask(this, 3);

			this.completedCount = 0;
			{
				var awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5>, GdTask<T1>.Awaiter>)state)
						{
							TryInvokeContinuationT1(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task2.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT2(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5>, GdTask<T2>.Awaiter>)state)
						{
							TryInvokeContinuationT2(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task3.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT3(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5>, GdTask<T3>.Awaiter>)state)
						{
							TryInvokeContinuationT3(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task4.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT4(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5>, GdTask<T4>.Awaiter>)state)
						{
							TryInvokeContinuationT4(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task5.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT5(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5>, GdTask<T5>.Awaiter>)state)
						{
							TryInvokeContinuationT5(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
		}

		public (T1, T2, T3, T4, T5) GetResult(short token)
		{
			TaskTracker.RemoveTracking(this);
			GC.SuppressFinalize(this);
			return core.GetResult(token);
		}

		void IGdTaskSource.GetResult(short token)
		{
			GetResult(token);
		}

		public GdTaskStatus GetStatus(short token)
		{
			return core.GetStatus(token);
		}

		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			core.OnCompleted(continuation, state, token);
		}

		public GdTaskStatus UnsafeGetStatus()
		{
			return core.UnsafeGetStatus();
		}

		private static void TryInvokeContinuationT1(WhenAllPromise<T1, T2, T3, T4, T5> self, in GdTask<T1>.Awaiter awaiter)
		{
			try
			{
				self.t1 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 5)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5));
			}
		}

		private static void TryInvokeContinuationT2(WhenAllPromise<T1, T2, T3, T4, T5> self, in GdTask<T2>.Awaiter awaiter)
		{
			try
			{
				self.t2 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 5)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5));
			}
		}

		private static void TryInvokeContinuationT3(WhenAllPromise<T1, T2, T3, T4, T5> self, in GdTask<T3>.Awaiter awaiter)
		{
			try
			{
				self.t3 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 5)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5));
			}
		}

		private static void TryInvokeContinuationT4(WhenAllPromise<T1, T2, T3, T4, T5> self, in GdTask<T4>.Awaiter awaiter)
		{
			try
			{
				self.t4 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 5)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5));
			}
		}

		private static void TryInvokeContinuationT5(WhenAllPromise<T1, T2, T3, T4, T5> self, in GdTask<T5>.Awaiter awaiter)
		{
			try
			{
				self.t5 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 5)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5));
			}
		}
	}
	private sealed class WhenAllPromise<T1, T2, T3, T4, T5, T6> : IGdTaskSource<(T1, T2, T3, T4, T5, T6)>
	{
		private int completedCount;
		private GdTaskCompletionSourceCore<(T1, T2, T3, T4, T5, T6)> core;
		private T1 t1 = default;
		private T2 t2 = default;
		private T3 t3 = default;
		private T4 t4 = default;
		private T5 t5 = default;
		private T6 t6 = default;
		public WhenAllPromise(GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3, GdTask<T4> task4, GdTask<T5> task5, GdTask<T6> task6)
		{
			TaskTracker.TrackActiveTask(this, 3);

			this.completedCount = 0;
			{
				var awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6>, GdTask<T1>.Awaiter>)state)
						{
							TryInvokeContinuationT1(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task2.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT2(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6>, GdTask<T2>.Awaiter>)state)
						{
							TryInvokeContinuationT2(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task3.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT3(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6>, GdTask<T3>.Awaiter>)state)
						{
							TryInvokeContinuationT3(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task4.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT4(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6>, GdTask<T4>.Awaiter>)state)
						{
							TryInvokeContinuationT4(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task5.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT5(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6>, GdTask<T5>.Awaiter>)state)
						{
							TryInvokeContinuationT5(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task6.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT6(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6>, GdTask<T6>.Awaiter>)state)
						{
							TryInvokeContinuationT6(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
		}

		public (T1, T2, T3, T4, T5, T6) GetResult(short token)
		{
			TaskTracker.RemoveTracking(this);
			GC.SuppressFinalize(this);
			return core.GetResult(token);
		}

		void IGdTaskSource.GetResult(short token)
		{
			GetResult(token);
		}

		public GdTaskStatus GetStatus(short token)
		{
			return core.GetStatus(token);
		}

		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			core.OnCompleted(continuation, state, token);
		}

		public GdTaskStatus UnsafeGetStatus()
		{
			return core.UnsafeGetStatus();
		}

		private static void TryInvokeContinuationT1(WhenAllPromise<T1, T2, T3, T4, T5, T6> self, in GdTask<T1>.Awaiter awaiter)
		{
			try
			{
				self.t1 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 6)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6));
			}
		}

		private static void TryInvokeContinuationT2(WhenAllPromise<T1, T2, T3, T4, T5, T6> self, in GdTask<T2>.Awaiter awaiter)
		{
			try
			{
				self.t2 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 6)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6));
			}
		}

		private static void TryInvokeContinuationT3(WhenAllPromise<T1, T2, T3, T4, T5, T6> self, in GdTask<T3>.Awaiter awaiter)
		{
			try
			{
				self.t3 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 6)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6));
			}
		}

		private static void TryInvokeContinuationT4(WhenAllPromise<T1, T2, T3, T4, T5, T6> self, in GdTask<T4>.Awaiter awaiter)
		{
			try
			{
				self.t4 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 6)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6));
			}
		}

		private static void TryInvokeContinuationT5(WhenAllPromise<T1, T2, T3, T4, T5, T6> self, in GdTask<T5>.Awaiter awaiter)
		{
			try
			{
				self.t5 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 6)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6));
			}
		}

		private static void TryInvokeContinuationT6(WhenAllPromise<T1, T2, T3, T4, T5, T6> self, in GdTask<T6>.Awaiter awaiter)
		{
			try
			{
				self.t6 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 6)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6));
			}
		}
	}
	private sealed class WhenAllPromise<T1, T2, T3, T4, T5, T6, T7> : IGdTaskSource<(T1, T2, T3, T4, T5, T6, T7)>
	{
		private int completedCount;
		private GdTaskCompletionSourceCore<(T1, T2, T3, T4, T5, T6, T7)> core;
		private T1 t1 = default;
		private T2 t2 = default;
		private T3 t3 = default;
		private T4 t4 = default;
		private T5 t5 = default;
		private T6 t6 = default;
		private T7 t7 = default;
		public WhenAllPromise(GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3, GdTask<T4> task4, GdTask<T5> task5, GdTask<T6> task6, GdTask<T7> task7)
		{
			TaskTracker.TrackActiveTask(this, 3);

			this.completedCount = 0;
			{
				var awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>, GdTask<T1>.Awaiter>)state)
						{
							TryInvokeContinuationT1(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task2.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT2(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>, GdTask<T2>.Awaiter>)state)
						{
							TryInvokeContinuationT2(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task3.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT3(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>, GdTask<T3>.Awaiter>)state)
						{
							TryInvokeContinuationT3(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task4.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT4(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>, GdTask<T4>.Awaiter>)state)
						{
							TryInvokeContinuationT4(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task5.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT5(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>, GdTask<T5>.Awaiter>)state)
						{
							TryInvokeContinuationT5(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task6.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT6(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>, GdTask<T6>.Awaiter>)state)
						{
							TryInvokeContinuationT6(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task7.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT7(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>, GdTask<T7>.Awaiter>)state)
						{
							TryInvokeContinuationT7(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
		}

		public (T1, T2, T3, T4, T5, T6, T7) GetResult(short token)
		{
			TaskTracker.RemoveTracking(this);
			GC.SuppressFinalize(this);
			return core.GetResult(token);
		}

		void IGdTaskSource.GetResult(short token)
		{
			GetResult(token);
		}

		public GdTaskStatus GetStatus(short token)
		{
			return core.GetStatus(token);
		}

		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			core.OnCompleted(continuation, state, token);
		}

		public GdTaskStatus UnsafeGetStatus()
		{
			return core.UnsafeGetStatus();
		}

		private static void TryInvokeContinuationT1(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7> self, in GdTask<T1>.Awaiter awaiter)
		{
			try
			{
				self.t1 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 7)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7));
			}
		}

		private static void TryInvokeContinuationT2(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7> self, in GdTask<T2>.Awaiter awaiter)
		{
			try
			{
				self.t2 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 7)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7));
			}
		}

		private static void TryInvokeContinuationT3(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7> self, in GdTask<T3>.Awaiter awaiter)
		{
			try
			{
				self.t3 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 7)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7));
			}
		}

		private static void TryInvokeContinuationT4(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7> self, in GdTask<T4>.Awaiter awaiter)
		{
			try
			{
				self.t4 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 7)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7));
			}
		}

		private static void TryInvokeContinuationT5(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7> self, in GdTask<T5>.Awaiter awaiter)
		{
			try
			{
				self.t5 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 7)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7));
			}
		}

		private static void TryInvokeContinuationT6(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7> self, in GdTask<T6>.Awaiter awaiter)
		{
			try
			{
				self.t6 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 7)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7));
			}
		}

		private static void TryInvokeContinuationT7(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7> self, in GdTask<T7>.Awaiter awaiter)
		{
			try
			{
				self.t7 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 7)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7));
			}
		}
	}
	private sealed class WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8> : IGdTaskSource<(T1, T2, T3, T4, T5, T6, T7, T8)>
	{
		private int completedCount;
		private GdTaskCompletionSourceCore<(T1, T2, T3, T4, T5, T6, T7, T8)> core;
		private T1 t1 = default;
		private T2 t2 = default;
		private T3 t3 = default;
		private T4 t4 = default;
		private T5 t5 = default;
		private T6 t6 = default;
		private T7 t7 = default;
		private T8 t8 = default;
		public WhenAllPromise(GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3, GdTask<T4> task4, GdTask<T5> task5, GdTask<T6> task6, GdTask<T7> task7, GdTask<T8> task8)
		{
			TaskTracker.TrackActiveTask(this, 3);

			this.completedCount = 0;
			{
				var awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>, GdTask<T1>.Awaiter>)state)
						{
							TryInvokeContinuationT1(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task2.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT2(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>, GdTask<T2>.Awaiter>)state)
						{
							TryInvokeContinuationT2(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task3.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT3(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>, GdTask<T3>.Awaiter>)state)
						{
							TryInvokeContinuationT3(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task4.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT4(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>, GdTask<T4>.Awaiter>)state)
						{
							TryInvokeContinuationT4(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task5.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT5(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>, GdTask<T5>.Awaiter>)state)
						{
							TryInvokeContinuationT5(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task6.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT6(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>, GdTask<T6>.Awaiter>)state)
						{
							TryInvokeContinuationT6(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task7.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT7(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>, GdTask<T7>.Awaiter>)state)
						{
							TryInvokeContinuationT7(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task8.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT8(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>, GdTask<T8>.Awaiter>)state)
						{
							TryInvokeContinuationT8(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
		}

		public (T1, T2, T3, T4, T5, T6, T7, T8) GetResult(short token)
		{
			TaskTracker.RemoveTracking(this);
			GC.SuppressFinalize(this);
			return core.GetResult(token);
		}

		void IGdTaskSource.GetResult(short token)
		{
			GetResult(token);
		}

		public GdTaskStatus GetStatus(short token)
		{
			return core.GetStatus(token);
		}

		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			core.OnCompleted(continuation, state, token);
		}

		public GdTaskStatus UnsafeGetStatus()
		{
			return core.UnsafeGetStatus();
		}

		private static void TryInvokeContinuationT1(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8> self, in GdTask<T1>.Awaiter awaiter)
		{
			try
			{
				self.t1 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 8)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8));
			}
		}

		private static void TryInvokeContinuationT2(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8> self, in GdTask<T2>.Awaiter awaiter)
		{
			try
			{
				self.t2 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 8)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8));
			}
		}

		private static void TryInvokeContinuationT3(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8> self, in GdTask<T3>.Awaiter awaiter)
		{
			try
			{
				self.t3 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 8)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8));
			}
		}

		private static void TryInvokeContinuationT4(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8> self, in GdTask<T4>.Awaiter awaiter)
		{
			try
			{
				self.t4 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 8)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8));
			}
		}

		private static void TryInvokeContinuationT5(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8> self, in GdTask<T5>.Awaiter awaiter)
		{
			try
			{
				self.t5 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 8)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8));
			}
		}

		private static void TryInvokeContinuationT6(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8> self, in GdTask<T6>.Awaiter awaiter)
		{
			try
			{
				self.t6 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 8)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8));
			}
		}

		private static void TryInvokeContinuationT7(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8> self, in GdTask<T7>.Awaiter awaiter)
		{
			try
			{
				self.t7 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 8)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8));
			}
		}

		private static void TryInvokeContinuationT8(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8> self, in GdTask<T8>.Awaiter awaiter)
		{
			try
			{
				self.t8 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 8)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8));
			}
		}
	}
	private sealed class WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9> : IGdTaskSource<(T1, T2, T3, T4, T5, T6, T7, T8, T9)>
	{
		private int completedCount;
		private GdTaskCompletionSourceCore<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> core;
		private T1 t1 = default;
		private T2 t2 = default;
		private T3 t3 = default;
		private T4 t4 = default;
		private T5 t5 = default;
		private T6 t6 = default;
		private T7 t7 = default;
		private T8 t8 = default;
		private T9 t9 = default;
		public WhenAllPromise(GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3, GdTask<T4> task4, GdTask<T5> task5, GdTask<T6> task6, GdTask<T7> task7, GdTask<T8> task8, GdTask<T9> task9)
		{
			TaskTracker.TrackActiveTask(this, 3);

			this.completedCount = 0;
			{
				var awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, GdTask<T1>.Awaiter>)state)
						{
							TryInvokeContinuationT1(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task2.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT2(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, GdTask<T2>.Awaiter>)state)
						{
							TryInvokeContinuationT2(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task3.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT3(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, GdTask<T3>.Awaiter>)state)
						{
							TryInvokeContinuationT3(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task4.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT4(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, GdTask<T4>.Awaiter>)state)
						{
							TryInvokeContinuationT4(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task5.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT5(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, GdTask<T5>.Awaiter>)state)
						{
							TryInvokeContinuationT5(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task6.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT6(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, GdTask<T6>.Awaiter>)state)
						{
							TryInvokeContinuationT6(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task7.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT7(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, GdTask<T7>.Awaiter>)state)
						{
							TryInvokeContinuationT7(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task8.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT8(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, GdTask<T8>.Awaiter>)state)
						{
							TryInvokeContinuationT8(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task9.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT9(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, GdTask<T9>.Awaiter>)state)
						{
							TryInvokeContinuationT9(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
		}

		public (T1, T2, T3, T4, T5, T6, T7, T8, T9) GetResult(short token)
		{
			TaskTracker.RemoveTracking(this);
			GC.SuppressFinalize(this);
			return core.GetResult(token);
		}

		void IGdTaskSource.GetResult(short token)
		{
			GetResult(token);
		}

		public GdTaskStatus GetStatus(short token)
		{
			return core.GetStatus(token);
		}

		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			core.OnCompleted(continuation, state, token);
		}

		public GdTaskStatus UnsafeGetStatus()
		{
			return core.UnsafeGetStatus();
		}

		private static void TryInvokeContinuationT1(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9> self, in GdTask<T1>.Awaiter awaiter)
		{
			try
			{
				self.t1 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 9)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9));
			}
		}

		private static void TryInvokeContinuationT2(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9> self, in GdTask<T2>.Awaiter awaiter)
		{
			try
			{
				self.t2 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 9)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9));
			}
		}

		private static void TryInvokeContinuationT3(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9> self, in GdTask<T3>.Awaiter awaiter)
		{
			try
			{
				self.t3 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 9)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9));
			}
		}

		private static void TryInvokeContinuationT4(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9> self, in GdTask<T4>.Awaiter awaiter)
		{
			try
			{
				self.t4 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 9)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9));
			}
		}

		private static void TryInvokeContinuationT5(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9> self, in GdTask<T5>.Awaiter awaiter)
		{
			try
			{
				self.t5 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 9)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9));
			}
		}

		private static void TryInvokeContinuationT6(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9> self, in GdTask<T6>.Awaiter awaiter)
		{
			try
			{
				self.t6 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 9)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9));
			}
		}

		private static void TryInvokeContinuationT7(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9> self, in GdTask<T7>.Awaiter awaiter)
		{
			try
			{
				self.t7 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 9)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9));
			}
		}

		private static void TryInvokeContinuationT8(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9> self, in GdTask<T8>.Awaiter awaiter)
		{
			try
			{
				self.t8 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 9)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9));
			}
		}

		private static void TryInvokeContinuationT9(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9> self, in GdTask<T9>.Awaiter awaiter)
		{
			try
			{
				self.t9 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 9)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9));
			}
		}
	}
	private sealed class WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : IGdTaskSource<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)>
	{
		private int completedCount;
		private GdTaskCompletionSourceCore<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> core;
		private T1 t1 = default;
		private T10 t10 = default;
		private T2 t2 = default;
		private T3 t3 = default;
		private T4 t4 = default;
		private T5 t5 = default;
		private T6 t6 = default;
		private T7 t7 = default;
		private T8 t8 = default;
		private T9 t9 = default;
		public WhenAllPromise(GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3, GdTask<T4> task4, GdTask<T5> task5, GdTask<T6> task6, GdTask<T7> task7, GdTask<T8> task8, GdTask<T9> task9, GdTask<T10> task10)
		{
			TaskTracker.TrackActiveTask(this, 3);

			this.completedCount = 0;
			{
				var awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, GdTask<T1>.Awaiter>)state)
						{
							TryInvokeContinuationT1(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task2.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT2(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, GdTask<T2>.Awaiter>)state)
						{
							TryInvokeContinuationT2(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task3.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT3(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, GdTask<T3>.Awaiter>)state)
						{
							TryInvokeContinuationT3(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task4.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT4(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, GdTask<T4>.Awaiter>)state)
						{
							TryInvokeContinuationT4(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task5.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT5(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, GdTask<T5>.Awaiter>)state)
						{
							TryInvokeContinuationT5(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task6.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT6(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, GdTask<T6>.Awaiter>)state)
						{
							TryInvokeContinuationT6(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task7.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT7(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, GdTask<T7>.Awaiter>)state)
						{
							TryInvokeContinuationT7(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task8.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT8(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, GdTask<T8>.Awaiter>)state)
						{
							TryInvokeContinuationT8(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task9.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT9(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, GdTask<T9>.Awaiter>)state)
						{
							TryInvokeContinuationT9(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task10.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT10(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, GdTask<T10>.Awaiter>)state)
						{
							TryInvokeContinuationT10(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
		}

		public (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) GetResult(short token)
		{
			TaskTracker.RemoveTracking(this);
			GC.SuppressFinalize(this);
			return core.GetResult(token);
		}

		void IGdTaskSource.GetResult(short token)
		{
			GetResult(token);
		}

		public GdTaskStatus GetStatus(short token)
		{
			return core.GetStatus(token);
		}

		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			core.OnCompleted(continuation, state, token);
		}

		public GdTaskStatus UnsafeGetStatus()
		{
			return core.UnsafeGetStatus();
		}

		private static void TryInvokeContinuationT1(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> self, in GdTask<T1>.Awaiter awaiter)
		{
			try
			{
				self.t1 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 10)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10));
			}
		}

		private static void TryInvokeContinuationT10(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> self, in GdTask<T10>.Awaiter awaiter)
		{
			try
			{
				self.t10 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 10)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10));
			}
		}

		private static void TryInvokeContinuationT2(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> self, in GdTask<T2>.Awaiter awaiter)
		{
			try
			{
				self.t2 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 10)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10));
			}
		}

		private static void TryInvokeContinuationT3(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> self, in GdTask<T3>.Awaiter awaiter)
		{
			try
			{
				self.t3 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 10)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10));
			}
		}

		private static void TryInvokeContinuationT4(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> self, in GdTask<T4>.Awaiter awaiter)
		{
			try
			{
				self.t4 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 10)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10));
			}
		}

		private static void TryInvokeContinuationT5(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> self, in GdTask<T5>.Awaiter awaiter)
		{
			try
			{
				self.t5 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 10)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10));
			}
		}

		private static void TryInvokeContinuationT6(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> self, in GdTask<T6>.Awaiter awaiter)
		{
			try
			{
				self.t6 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 10)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10));
			}
		}

		private static void TryInvokeContinuationT7(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> self, in GdTask<T7>.Awaiter awaiter)
		{
			try
			{
				self.t7 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 10)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10));
			}
		}

		private static void TryInvokeContinuationT8(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> self, in GdTask<T8>.Awaiter awaiter)
		{
			try
			{
				self.t8 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 10)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10));
			}
		}

		private static void TryInvokeContinuationT9(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> self, in GdTask<T9>.Awaiter awaiter)
		{
			try
			{
				self.t9 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 10)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10));
			}
		}
	}
	private sealed class WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : IGdTaskSource<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)>
	{
		private int completedCount;
		private GdTaskCompletionSourceCore<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> core;
		private T1 t1 = default;
		private T10 t10 = default;
		private T11 t11 = default;
		private T2 t2 = default;
		private T3 t3 = default;
		private T4 t4 = default;
		private T5 t5 = default;
		private T6 t6 = default;
		private T7 t7 = default;
		private T8 t8 = default;
		private T9 t9 = default;
		public WhenAllPromise(GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3, GdTask<T4> task4, GdTask<T5> task5, GdTask<T6> task6, GdTask<T7> task7, GdTask<T8> task8, GdTask<T9> task9, GdTask<T10> task10, GdTask<T11> task11)
		{
			TaskTracker.TrackActiveTask(this, 3);

			this.completedCount = 0;
			{
				var awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, GdTask<T1>.Awaiter>)state)
						{
							TryInvokeContinuationT1(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task2.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT2(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, GdTask<T2>.Awaiter>)state)
						{
							TryInvokeContinuationT2(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task3.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT3(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, GdTask<T3>.Awaiter>)state)
						{
							TryInvokeContinuationT3(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task4.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT4(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, GdTask<T4>.Awaiter>)state)
						{
							TryInvokeContinuationT4(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task5.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT5(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, GdTask<T5>.Awaiter>)state)
						{
							TryInvokeContinuationT5(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task6.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT6(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, GdTask<T6>.Awaiter>)state)
						{
							TryInvokeContinuationT6(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task7.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT7(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, GdTask<T7>.Awaiter>)state)
						{
							TryInvokeContinuationT7(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task8.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT8(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, GdTask<T8>.Awaiter>)state)
						{
							TryInvokeContinuationT8(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task9.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT9(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, GdTask<T9>.Awaiter>)state)
						{
							TryInvokeContinuationT9(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task10.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT10(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, GdTask<T10>.Awaiter>)state)
						{
							TryInvokeContinuationT10(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task11.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT11(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, GdTask<T11>.Awaiter>)state)
						{
							TryInvokeContinuationT11(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
		}

		public (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) GetResult(short token)
		{
			TaskTracker.RemoveTracking(this);
			GC.SuppressFinalize(this);
			return core.GetResult(token);
		}

		void IGdTaskSource.GetResult(short token)
		{
			GetResult(token);
		}

		public GdTaskStatus GetStatus(short token)
		{
			return core.GetStatus(token);
		}

		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			core.OnCompleted(continuation, state, token);
		}

		public GdTaskStatus UnsafeGetStatus()
		{
			return core.UnsafeGetStatus();
		}

		private static void TryInvokeContinuationT1(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, in GdTask<T1>.Awaiter awaiter)
		{
			try
			{
				self.t1 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 11)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11));
			}
		}

		private static void TryInvokeContinuationT10(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, in GdTask<T10>.Awaiter awaiter)
		{
			try
			{
				self.t10 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 11)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11));
			}
		}

		private static void TryInvokeContinuationT11(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, in GdTask<T11>.Awaiter awaiter)
		{
			try
			{
				self.t11 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 11)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11));
			}
		}

		private static void TryInvokeContinuationT2(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, in GdTask<T2>.Awaiter awaiter)
		{
			try
			{
				self.t2 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 11)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11));
			}
		}

		private static void TryInvokeContinuationT3(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, in GdTask<T3>.Awaiter awaiter)
		{
			try
			{
				self.t3 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 11)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11));
			}
		}

		private static void TryInvokeContinuationT4(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, in GdTask<T4>.Awaiter awaiter)
		{
			try
			{
				self.t4 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 11)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11));
			}
		}

		private static void TryInvokeContinuationT5(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, in GdTask<T5>.Awaiter awaiter)
		{
			try
			{
				self.t5 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 11)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11));
			}
		}

		private static void TryInvokeContinuationT6(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, in GdTask<T6>.Awaiter awaiter)
		{
			try
			{
				self.t6 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 11)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11));
			}
		}

		private static void TryInvokeContinuationT7(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, in GdTask<T7>.Awaiter awaiter)
		{
			try
			{
				self.t7 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 11)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11));
			}
		}

		private static void TryInvokeContinuationT8(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, in GdTask<T8>.Awaiter awaiter)
		{
			try
			{
				self.t8 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 11)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11));
			}
		}

		private static void TryInvokeContinuationT9(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, in GdTask<T9>.Awaiter awaiter)
		{
			try
			{
				self.t9 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 11)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11));
			}
		}
	}
	private sealed class WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : IGdTaskSource<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)>
	{
		private int completedCount;
		private GdTaskCompletionSourceCore<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> core;
		private T1 t1 = default;
		private T10 t10 = default;
		private T11 t11 = default;
		private T12 t12 = default;
		private T2 t2 = default;
		private T3 t3 = default;
		private T4 t4 = default;
		private T5 t5 = default;
		private T6 t6 = default;
		private T7 t7 = default;
		private T8 t8 = default;
		private T9 t9 = default;
		public WhenAllPromise(GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3, GdTask<T4> task4, GdTask<T5> task5, GdTask<T6> task6, GdTask<T7> task7, GdTask<T8> task8, GdTask<T9> task9, GdTask<T10> task10, GdTask<T11> task11, GdTask<T12> task12)
		{
			TaskTracker.TrackActiveTask(this, 3);

			this.completedCount = 0;
			{
				var awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, GdTask<T1>.Awaiter>)state)
						{
							TryInvokeContinuationT1(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task2.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT2(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, GdTask<T2>.Awaiter>)state)
						{
							TryInvokeContinuationT2(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task3.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT3(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, GdTask<T3>.Awaiter>)state)
						{
							TryInvokeContinuationT3(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task4.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT4(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, GdTask<T4>.Awaiter>)state)
						{
							TryInvokeContinuationT4(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task5.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT5(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, GdTask<T5>.Awaiter>)state)
						{
							TryInvokeContinuationT5(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task6.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT6(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, GdTask<T6>.Awaiter>)state)
						{
							TryInvokeContinuationT6(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task7.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT7(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, GdTask<T7>.Awaiter>)state)
						{
							TryInvokeContinuationT7(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task8.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT8(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, GdTask<T8>.Awaiter>)state)
						{
							TryInvokeContinuationT8(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task9.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT9(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, GdTask<T9>.Awaiter>)state)
						{
							TryInvokeContinuationT9(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task10.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT10(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, GdTask<T10>.Awaiter>)state)
						{
							TryInvokeContinuationT10(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task11.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT11(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, GdTask<T11>.Awaiter>)state)
						{
							TryInvokeContinuationT11(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task12.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT12(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, GdTask<T12>.Awaiter>)state)
						{
							TryInvokeContinuationT12(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
		}

		public (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) GetResult(short token)
		{
			TaskTracker.RemoveTracking(this);
			GC.SuppressFinalize(this);
			return core.GetResult(token);
		}

		void IGdTaskSource.GetResult(short token)
		{
			GetResult(token);
		}

		public GdTaskStatus GetStatus(short token)
		{
			return core.GetStatus(token);
		}

		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			core.OnCompleted(continuation, state, token);
		}

		public GdTaskStatus UnsafeGetStatus()
		{
			return core.UnsafeGetStatus();
		}

		private static void TryInvokeContinuationT1(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, in GdTask<T1>.Awaiter awaiter)
		{
			try
			{
				self.t1 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 12)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12));
			}
		}

		private static void TryInvokeContinuationT10(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, in GdTask<T10>.Awaiter awaiter)
		{
			try
			{
				self.t10 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 12)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12));
			}
		}

		private static void TryInvokeContinuationT11(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, in GdTask<T11>.Awaiter awaiter)
		{
			try
			{
				self.t11 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 12)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12));
			}
		}

		private static void TryInvokeContinuationT12(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, in GdTask<T12>.Awaiter awaiter)
		{
			try
			{
				self.t12 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 12)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12));
			}
		}

		private static void TryInvokeContinuationT2(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, in GdTask<T2>.Awaiter awaiter)
		{
			try
			{
				self.t2 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 12)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12));
			}
		}

		private static void TryInvokeContinuationT3(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, in GdTask<T3>.Awaiter awaiter)
		{
			try
			{
				self.t3 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 12)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12));
			}
		}

		private static void TryInvokeContinuationT4(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, in GdTask<T4>.Awaiter awaiter)
		{
			try
			{
				self.t4 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 12)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12));
			}
		}

		private static void TryInvokeContinuationT5(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, in GdTask<T5>.Awaiter awaiter)
		{
			try
			{
				self.t5 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 12)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12));
			}
		}

		private static void TryInvokeContinuationT6(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, in GdTask<T6>.Awaiter awaiter)
		{
			try
			{
				self.t6 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 12)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12));
			}
		}

		private static void TryInvokeContinuationT7(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, in GdTask<T7>.Awaiter awaiter)
		{
			try
			{
				self.t7 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 12)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12));
			}
		}

		private static void TryInvokeContinuationT8(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, in GdTask<T8>.Awaiter awaiter)
		{
			try
			{
				self.t8 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 12)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12));
			}
		}

		private static void TryInvokeContinuationT9(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, in GdTask<T9>.Awaiter awaiter)
		{
			try
			{
				self.t9 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 12)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12));
			}
		}
	}
	private sealed class WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : IGdTaskSource<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)>
	{
		private int completedCount;
		private GdTaskCompletionSourceCore<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)> core;
		private T1 t1 = default;
		private T10 t10 = default;
		private T11 t11 = default;
		private T12 t12 = default;
		private T13 t13 = default;
		private T2 t2 = default;
		private T3 t3 = default;
		private T4 t4 = default;
		private T5 t5 = default;
		private T6 t6 = default;
		private T7 t7 = default;
		private T8 t8 = default;
		private T9 t9 = default;
		public WhenAllPromise(GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3, GdTask<T4> task4, GdTask<T5> task5, GdTask<T6> task6, GdTask<T7> task7, GdTask<T8> task8, GdTask<T9> task9, GdTask<T10> task10, GdTask<T11> task11, GdTask<T12> task12, GdTask<T13> task13)
		{
			TaskTracker.TrackActiveTask(this, 3);

			this.completedCount = 0;
			{
				var awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, GdTask<T1>.Awaiter>)state)
						{
							TryInvokeContinuationT1(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task2.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT2(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, GdTask<T2>.Awaiter>)state)
						{
							TryInvokeContinuationT2(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task3.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT3(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, GdTask<T3>.Awaiter>)state)
						{
							TryInvokeContinuationT3(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task4.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT4(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, GdTask<T4>.Awaiter>)state)
						{
							TryInvokeContinuationT4(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task5.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT5(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, GdTask<T5>.Awaiter>)state)
						{
							TryInvokeContinuationT5(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task6.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT6(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, GdTask<T6>.Awaiter>)state)
						{
							TryInvokeContinuationT6(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task7.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT7(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, GdTask<T7>.Awaiter>)state)
						{
							TryInvokeContinuationT7(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task8.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT8(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, GdTask<T8>.Awaiter>)state)
						{
							TryInvokeContinuationT8(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task9.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT9(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, GdTask<T9>.Awaiter>)state)
						{
							TryInvokeContinuationT9(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task10.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT10(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, GdTask<T10>.Awaiter>)state)
						{
							TryInvokeContinuationT10(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task11.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT11(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, GdTask<T11>.Awaiter>)state)
						{
							TryInvokeContinuationT11(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task12.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT12(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, GdTask<T12>.Awaiter>)state)
						{
							TryInvokeContinuationT12(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task13.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT13(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, GdTask<T13>.Awaiter>)state)
						{
							TryInvokeContinuationT13(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
		}

		public (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13) GetResult(short token)
		{
			TaskTracker.RemoveTracking(this);
			GC.SuppressFinalize(this);
			return core.GetResult(token);
		}

		void IGdTaskSource.GetResult(short token)
		{
			GetResult(token);
		}

		public GdTaskStatus GetStatus(short token)
		{
			return core.GetStatus(token);
		}

		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			core.OnCompleted(continuation, state, token);
		}

		public GdTaskStatus UnsafeGetStatus()
		{
			return core.UnsafeGetStatus();
		}

		private static void TryInvokeContinuationT1(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in GdTask<T1>.Awaiter awaiter)
		{
			try
			{
				self.t1 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 13)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13));
			}
		}

		private static void TryInvokeContinuationT10(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in GdTask<T10>.Awaiter awaiter)
		{
			try
			{
				self.t10 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 13)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13));
			}
		}

		private static void TryInvokeContinuationT11(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in GdTask<T11>.Awaiter awaiter)
		{
			try
			{
				self.t11 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 13)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13));
			}
		}

		private static void TryInvokeContinuationT12(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in GdTask<T12>.Awaiter awaiter)
		{
			try
			{
				self.t12 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 13)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13));
			}
		}

		private static void TryInvokeContinuationT13(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in GdTask<T13>.Awaiter awaiter)
		{
			try
			{
				self.t13 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 13)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13));
			}
		}

		private static void TryInvokeContinuationT2(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in GdTask<T2>.Awaiter awaiter)
		{
			try
			{
				self.t2 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 13)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13));
			}
		}

		private static void TryInvokeContinuationT3(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in GdTask<T3>.Awaiter awaiter)
		{
			try
			{
				self.t3 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 13)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13));
			}
		}

		private static void TryInvokeContinuationT4(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in GdTask<T4>.Awaiter awaiter)
		{
			try
			{
				self.t4 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 13)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13));
			}
		}

		private static void TryInvokeContinuationT5(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in GdTask<T5>.Awaiter awaiter)
		{
			try
			{
				self.t5 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 13)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13));
			}
		}

		private static void TryInvokeContinuationT6(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in GdTask<T6>.Awaiter awaiter)
		{
			try
			{
				self.t6 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 13)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13));
			}
		}

		private static void TryInvokeContinuationT7(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in GdTask<T7>.Awaiter awaiter)
		{
			try
			{
				self.t7 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 13)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13));
			}
		}

		private static void TryInvokeContinuationT8(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in GdTask<T8>.Awaiter awaiter)
		{
			try
			{
				self.t8 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 13)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13));
			}
		}

		private static void TryInvokeContinuationT9(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in GdTask<T9>.Awaiter awaiter)
		{
			try
			{
				self.t9 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 13)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13));
			}
		}
	}
	private sealed class WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : IGdTaskSource<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)>
	{
		private int completedCount;
		private GdTaskCompletionSourceCore<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)> core;
		private T1 t1 = default;
		private T10 t10 = default;
		private T11 t11 = default;
		private T12 t12 = default;
		private T13 t13 = default;
		private T14 t14 = default;
		private T2 t2 = default;
		private T3 t3 = default;
		private T4 t4 = default;
		private T5 t5 = default;
		private T6 t6 = default;
		private T7 t7 = default;
		private T8 t8 = default;
		private T9 t9 = default;
		public WhenAllPromise(GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3, GdTask<T4> task4, GdTask<T5> task5, GdTask<T6> task6, GdTask<T7> task7, GdTask<T8> task8, GdTask<T9> task9, GdTask<T10> task10, GdTask<T11> task11, GdTask<T12> task12, GdTask<T13> task13, GdTask<T14> task14)
		{
			TaskTracker.TrackActiveTask(this, 3);

			this.completedCount = 0;
			{
				var awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, GdTask<T1>.Awaiter>)state)
						{
							TryInvokeContinuationT1(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task2.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT2(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, GdTask<T2>.Awaiter>)state)
						{
							TryInvokeContinuationT2(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task3.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT3(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, GdTask<T3>.Awaiter>)state)
						{
							TryInvokeContinuationT3(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task4.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT4(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, GdTask<T4>.Awaiter>)state)
						{
							TryInvokeContinuationT4(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task5.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT5(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, GdTask<T5>.Awaiter>)state)
						{
							TryInvokeContinuationT5(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task6.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT6(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, GdTask<T6>.Awaiter>)state)
						{
							TryInvokeContinuationT6(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task7.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT7(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, GdTask<T7>.Awaiter>)state)
						{
							TryInvokeContinuationT7(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task8.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT8(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, GdTask<T8>.Awaiter>)state)
						{
							TryInvokeContinuationT8(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task9.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT9(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, GdTask<T9>.Awaiter>)state)
						{
							TryInvokeContinuationT9(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task10.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT10(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, GdTask<T10>.Awaiter>)state)
						{
							TryInvokeContinuationT10(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task11.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT11(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, GdTask<T11>.Awaiter>)state)
						{
							TryInvokeContinuationT11(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task12.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT12(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, GdTask<T12>.Awaiter>)state)
						{
							TryInvokeContinuationT12(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task13.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT13(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, GdTask<T13>.Awaiter>)state)
						{
							TryInvokeContinuationT13(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task14.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT14(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, GdTask<T14>.Awaiter>)state)
						{
							TryInvokeContinuationT14(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
		}

		public (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14) GetResult(short token)
		{
			TaskTracker.RemoveTracking(this);
			GC.SuppressFinalize(this);
			return core.GetResult(token);
		}

		void IGdTaskSource.GetResult(short token)
		{
			GetResult(token);
		}

		public GdTaskStatus GetStatus(short token)
		{
			return core.GetStatus(token);
		}

		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			core.OnCompleted(continuation, state, token);
		}

		public GdTaskStatus UnsafeGetStatus()
		{
			return core.UnsafeGetStatus();
		}

		private static void TryInvokeContinuationT1(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in GdTask<T1>.Awaiter awaiter)
		{
			try
			{
				self.t1 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 14)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14));
			}
		}

		private static void TryInvokeContinuationT10(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in GdTask<T10>.Awaiter awaiter)
		{
			try
			{
				self.t10 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 14)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14));
			}
		}

		private static void TryInvokeContinuationT11(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in GdTask<T11>.Awaiter awaiter)
		{
			try
			{
				self.t11 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 14)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14));
			}
		}

		private static void TryInvokeContinuationT12(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in GdTask<T12>.Awaiter awaiter)
		{
			try
			{
				self.t12 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 14)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14));
			}
		}

		private static void TryInvokeContinuationT13(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in GdTask<T13>.Awaiter awaiter)
		{
			try
			{
				self.t13 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 14)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14));
			}
		}

		private static void TryInvokeContinuationT14(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in GdTask<T14>.Awaiter awaiter)
		{
			try
			{
				self.t14 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 14)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14));
			}
		}

		private static void TryInvokeContinuationT2(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in GdTask<T2>.Awaiter awaiter)
		{
			try
			{
				self.t2 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 14)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14));
			}
		}

		private static void TryInvokeContinuationT3(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in GdTask<T3>.Awaiter awaiter)
		{
			try
			{
				self.t3 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 14)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14));
			}
		}

		private static void TryInvokeContinuationT4(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in GdTask<T4>.Awaiter awaiter)
		{
			try
			{
				self.t4 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 14)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14));
			}
		}

		private static void TryInvokeContinuationT5(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in GdTask<T5>.Awaiter awaiter)
		{
			try
			{
				self.t5 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 14)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14));
			}
		}

		private static void TryInvokeContinuationT6(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in GdTask<T6>.Awaiter awaiter)
		{
			try
			{
				self.t6 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 14)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14));
			}
		}

		private static void TryInvokeContinuationT7(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in GdTask<T7>.Awaiter awaiter)
		{
			try
			{
				self.t7 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 14)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14));
			}
		}

		private static void TryInvokeContinuationT8(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in GdTask<T8>.Awaiter awaiter)
		{
			try
			{
				self.t8 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 14)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14));
			}
		}

		private static void TryInvokeContinuationT9(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in GdTask<T9>.Awaiter awaiter)
		{
			try
			{
				self.t9 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 14)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14));
			}
		}
	}
	private sealed class WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : IGdTaskSource<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)>
	{
		private int completedCount;
		private GdTaskCompletionSourceCore<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)> core;
		private T1 t1 = default;
		private T10 t10 = default;
		private T11 t11 = default;
		private T12 t12 = default;
		private T13 t13 = default;
		private T14 t14 = default;
		private T15 t15 = default;
		private T2 t2 = default;
		private T3 t3 = default;
		private T4 t4 = default;
		private T5 t5 = default;
		private T6 t6 = default;
		private T7 t7 = default;
		private T8 t8 = default;
		private T9 t9 = default;
		public WhenAllPromise(GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3, GdTask<T4> task4, GdTask<T5> task5, GdTask<T6> task6, GdTask<T7> task7, GdTask<T8> task8, GdTask<T9> task9, GdTask<T10> task10, GdTask<T11> task11, GdTask<T12> task12, GdTask<T13> task13, GdTask<T14> task14, GdTask<T15> task15)
		{
			TaskTracker.TrackActiveTask(this, 3);

			this.completedCount = 0;
			{
				var awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, GdTask<T1>.Awaiter>)state)
						{
							TryInvokeContinuationT1(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task2.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT2(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, GdTask<T2>.Awaiter>)state)
						{
							TryInvokeContinuationT2(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task3.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT3(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, GdTask<T3>.Awaiter>)state)
						{
							TryInvokeContinuationT3(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task4.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT4(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, GdTask<T4>.Awaiter>)state)
						{
							TryInvokeContinuationT4(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task5.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT5(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, GdTask<T5>.Awaiter>)state)
						{
							TryInvokeContinuationT5(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task6.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT6(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, GdTask<T6>.Awaiter>)state)
						{
							TryInvokeContinuationT6(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task7.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT7(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, GdTask<T7>.Awaiter>)state)
						{
							TryInvokeContinuationT7(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task8.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT8(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, GdTask<T8>.Awaiter>)state)
						{
							TryInvokeContinuationT8(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task9.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT9(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, GdTask<T9>.Awaiter>)state)
						{
							TryInvokeContinuationT9(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task10.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT10(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, GdTask<T10>.Awaiter>)state)
						{
							TryInvokeContinuationT10(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task11.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT11(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, GdTask<T11>.Awaiter>)state)
						{
							TryInvokeContinuationT11(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task12.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT12(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, GdTask<T12>.Awaiter>)state)
						{
							TryInvokeContinuationT12(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task13.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT13(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, GdTask<T13>.Awaiter>)state)
						{
							TryInvokeContinuationT13(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task14.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT14(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, GdTask<T14>.Awaiter>)state)
						{
							TryInvokeContinuationT14(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
			{
				var awaiter = task15.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					TryInvokeContinuationT15(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(state =>
					{
						using (var t = (StateTuple<WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, GdTask<T15>.Awaiter>)state)
						{
							TryInvokeContinuationT15(t.Item1, t.Item2);
						}
					}, StateTuple.Create(this, awaiter));
				}
			}
		}

		public (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15) GetResult(short token)
		{
			TaskTracker.RemoveTracking(this);
			GC.SuppressFinalize(this);
			return core.GetResult(token);
		}

		void IGdTaskSource.GetResult(short token)
		{
			GetResult(token);
		}

		public GdTaskStatus GetStatus(short token)
		{
			return core.GetStatus(token);
		}

		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			core.OnCompleted(continuation, state, token);
		}

		public GdTaskStatus UnsafeGetStatus()
		{
			return core.UnsafeGetStatus();
		}

		private static void TryInvokeContinuationT1(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in GdTask<T1>.Awaiter awaiter)
		{
			try
			{
				self.t1 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 15)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14, self.t15));
			}
		}

		private static void TryInvokeContinuationT10(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in GdTask<T10>.Awaiter awaiter)
		{
			try
			{
				self.t10 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 15)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14, self.t15));
			}
		}

		private static void TryInvokeContinuationT11(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in GdTask<T11>.Awaiter awaiter)
		{
			try
			{
				self.t11 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 15)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14, self.t15));
			}
		}

		private static void TryInvokeContinuationT12(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in GdTask<T12>.Awaiter awaiter)
		{
			try
			{
				self.t12 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 15)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14, self.t15));
			}
		}

		private static void TryInvokeContinuationT13(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in GdTask<T13>.Awaiter awaiter)
		{
			try
			{
				self.t13 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 15)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14, self.t15));
			}
		}

		private static void TryInvokeContinuationT14(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in GdTask<T14>.Awaiter awaiter)
		{
			try
			{
				self.t14 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 15)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14, self.t15));
			}
		}

		private static void TryInvokeContinuationT15(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in GdTask<T15>.Awaiter awaiter)
		{
			try
			{
				self.t15 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 15)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14, self.t15));
			}
		}

		private static void TryInvokeContinuationT2(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in GdTask<T2>.Awaiter awaiter)
		{
			try
			{
				self.t2 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 15)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14, self.t15));
			}
		}

		private static void TryInvokeContinuationT3(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in GdTask<T3>.Awaiter awaiter)
		{
			try
			{
				self.t3 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 15)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14, self.t15));
			}
		}

		private static void TryInvokeContinuationT4(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in GdTask<T4>.Awaiter awaiter)
		{
			try
			{
				self.t4 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 15)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14, self.t15));
			}
		}

		private static void TryInvokeContinuationT5(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in GdTask<T5>.Awaiter awaiter)
		{
			try
			{
				self.t5 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 15)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14, self.t15));
			}
		}

		private static void TryInvokeContinuationT6(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in GdTask<T6>.Awaiter awaiter)
		{
			try
			{
				self.t6 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 15)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14, self.t15));
			}
		}

		private static void TryInvokeContinuationT7(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in GdTask<T7>.Awaiter awaiter)
		{
			try
			{
				self.t7 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 15)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14, self.t15));
			}
		}

		private static void TryInvokeContinuationT8(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in GdTask<T8>.Awaiter awaiter)
		{
			try
			{
				self.t8 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 15)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14, self.t15));
			}
		}

		private static void TryInvokeContinuationT9(WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in GdTask<T9>.Awaiter awaiter)
		{
			try
			{
				self.t9 = awaiter.GetResult();
			}
			catch (Exception ex)
			{
				self.core.TrySetException(ex);
				return;
			}

			if (Interlocked.Increment(ref self.completedCount) == 15)
			{
				self.core.TrySetResult((self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14, self.t15));
			}
		}
	}
}