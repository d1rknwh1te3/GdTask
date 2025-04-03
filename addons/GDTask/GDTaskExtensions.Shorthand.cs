﻿using System.Collections.Generic;

namespace Fractural.Tasks;

public static partial class GdTaskExtensions
{
	// shorthand of WhenAll

	public static GdTask.Awaiter GetAwaiter(this GdTask[] tasks)
	{
		return GdTask.WhenAll(tasks).GetAwaiter();
	}

	public static GdTask.Awaiter GetAwaiter(this IEnumerable<GdTask> tasks)
	{
		return GdTask.WhenAll(tasks).GetAwaiter();
	}

	public static GdTask<T[]>.Awaiter GetAwaiter<T>(this GdTask<T>[] tasks)
	{
		return GdTask.WhenAll(tasks).GetAwaiter();
	}

	public static GdTask<T[]>.Awaiter GetAwaiter<T>(this IEnumerable<GdTask<T>> tasks)
	{
		return GdTask.WhenAll(tasks).GetAwaiter();
	}

	public static GdTask<(T1, T2)>.Awaiter GetAwaiter<T1, T2>(this (GdTask<T1> task1, GdTask<T2> task2) tasks)
	{
		return GdTask.WhenAll(tasks.Item1, tasks.Item2).GetAwaiter();
	}

	public static GdTask<(T1, T2, T3)>.Awaiter GetAwaiter<T1, T2, T3>(this (GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3) tasks)
	{
		return GdTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3).GetAwaiter();
	}

	public static GdTask<(T1, T2, T3, T4)>.Awaiter GetAwaiter<T1, T2, T3, T4>(this (GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3, GdTask<T4> task4) tasks)
	{
		return GdTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4).GetAwaiter();
	}

	public static GdTask<(T1, T2, T3, T4, T5)>.Awaiter GetAwaiter<T1, T2, T3, T4, T5>(this (GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3, GdTask<T4> task4, GdTask<T5> task5) tasks)
	{
		return GdTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5).GetAwaiter();
	}

	public static GdTask<(T1, T2, T3, T4, T5, T6)>.Awaiter GetAwaiter<T1, T2, T3, T4, T5, T6>(this (GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3, GdTask<T4> task4, GdTask<T5> task5, GdTask<T6> task6) tasks)
	{
		return GdTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6).GetAwaiter();
	}

	public static GdTask<(T1, T2, T3, T4, T5, T6, T7)>.Awaiter GetAwaiter<T1, T2, T3, T4, T5, T6, T7>(this (GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3, GdTask<T4> task4, GdTask<T5> task5, GdTask<T6> task6, GdTask<T7> task7) tasks)
	{
		return GdTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6, tasks.Item7).GetAwaiter();
	}

	public static GdTask<(T1, T2, T3, T4, T5, T6, T7, T8)>.Awaiter GetAwaiter<T1, T2, T3, T4, T5, T6, T7, T8>(this (GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3, GdTask<T4> task4, GdTask<T5> task5, GdTask<T6> task6, GdTask<T7> task7, GdTask<T8> task8) tasks)
	{
		return GdTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6, tasks.Item7, tasks.Item8).GetAwaiter();
	}

	public static GdTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9)>.Awaiter GetAwaiter<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this (GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3, GdTask<T4> task4, GdTask<T5> task5, GdTask<T6> task6, GdTask<T7> task7, GdTask<T8> task8, GdTask<T9> task9) tasks)
	{
		return GdTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6, tasks.Item7, tasks.Item8, tasks.Item9).GetAwaiter();
	}

	public static GdTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)>.Awaiter GetAwaiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this (GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3, GdTask<T4> task4, GdTask<T5> task5, GdTask<T6> task6, GdTask<T7> task7, GdTask<T8> task8, GdTask<T9> task9, GdTask<T10> task10) tasks)
	{
		return GdTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6, tasks.Item7, tasks.Item8, tasks.Item9, tasks.Item10).GetAwaiter();
	}

	public static GdTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)>.Awaiter GetAwaiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this (GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3, GdTask<T4> task4, GdTask<T5> task5, GdTask<T6> task6, GdTask<T7> task7, GdTask<T8> task8, GdTask<T9> task9, GdTask<T10> task10, GdTask<T11> task11) tasks)
	{
		return GdTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6, tasks.Item7, tasks.Item8, tasks.Item9, tasks.Item10, tasks.Item11).GetAwaiter();
	}

	public static GdTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)>.Awaiter GetAwaiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this (GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3, GdTask<T4> task4, GdTask<T5> task5, GdTask<T6> task6, GdTask<T7> task7, GdTask<T8> task8, GdTask<T9> task9, GdTask<T10> task10, GdTask<T11> task11, GdTask<T12> task12) tasks)
	{
		return GdTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6, tasks.Item7, tasks.Item8, tasks.Item9, tasks.Item10, tasks.Item11, tasks.Item12).GetAwaiter();
	}

	public static GdTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)>.Awaiter GetAwaiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this (GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3, GdTask<T4> task4, GdTask<T5> task5, GdTask<T6> task6, GdTask<T7> task7, GdTask<T8> task8, GdTask<T9> task9, GdTask<T10> task10, GdTask<T11> task11, GdTask<T12> task12, GdTask<T13> task13) tasks)
	{
		return GdTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6, tasks.Item7, tasks.Item8, tasks.Item9, tasks.Item10, tasks.Item11, tasks.Item12, tasks.Item13).GetAwaiter();
	}

	public static GdTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)>.Awaiter GetAwaiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this (GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3, GdTask<T4> task4, GdTask<T5> task5, GdTask<T6> task6, GdTask<T7> task7, GdTask<T8> task8, GdTask<T9> task9, GdTask<T10> task10, GdTask<T11> task11, GdTask<T12> task12, GdTask<T13> task13, GdTask<T14> task14) tasks)
	{
		return GdTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6, tasks.Item7, tasks.Item8, tasks.Item9, tasks.Item10, tasks.Item11, tasks.Item12, tasks.Item13, tasks.Item14).GetAwaiter();
	}

	public static GdTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)>.Awaiter GetAwaiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this (GdTask<T1> task1, GdTask<T2> task2, GdTask<T3> task3, GdTask<T4> task4, GdTask<T5> task5, GdTask<T6> task6, GdTask<T7> task7, GdTask<T8> task8, GdTask<T9> task9, GdTask<T10> task10, GdTask<T11> task11, GdTask<T12> task12, GdTask<T13> task13, GdTask<T14> task14, GdTask<T15> task15) tasks)
	{
		return GdTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6, tasks.Item7, tasks.Item8, tasks.Item9, tasks.Item10, tasks.Item11, tasks.Item12, tasks.Item13, tasks.Item14, tasks.Item15).GetAwaiter();
	}



	public static GdTask.Awaiter GetAwaiter(this (GdTask task1, GdTask task2) tasks)
	{
		return GdTask.WhenAll(tasks.Item1, tasks.Item2).GetAwaiter();
	}


	public static GdTask.Awaiter GetAwaiter(this (GdTask task1, GdTask task2, GdTask task3) tasks)
	{
		return GdTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3).GetAwaiter();
	}


	public static GdTask.Awaiter GetAwaiter(this (GdTask task1, GdTask task2, GdTask task3, GdTask task4) tasks)
	{
		return GdTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4).GetAwaiter();
	}


	public static GdTask.Awaiter GetAwaiter(this (GdTask task1, GdTask task2, GdTask task3, GdTask task4, GdTask task5) tasks)
	{
		return GdTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5).GetAwaiter();
	}


	public static GdTask.Awaiter GetAwaiter(this (GdTask task1, GdTask task2, GdTask task3, GdTask task4, GdTask task5, GdTask task6) tasks)
	{
		return GdTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6).GetAwaiter();
	}


	public static GdTask.Awaiter GetAwaiter(this (GdTask task1, GdTask task2, GdTask task3, GdTask task4, GdTask task5, GdTask task6, GdTask task7) tasks)
	{
		return GdTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6, tasks.Item7).GetAwaiter();
	}


	public static GdTask.Awaiter GetAwaiter(this (GdTask task1, GdTask task2, GdTask task3, GdTask task4, GdTask task5, GdTask task6, GdTask task7, GdTask task8) tasks)
	{
		return GdTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6, tasks.Item7, tasks.Item8).GetAwaiter();
	}


	public static GdTask.Awaiter GetAwaiter(this (GdTask task1, GdTask task2, GdTask task3, GdTask task4, GdTask task5, GdTask task6, GdTask task7, GdTask task8, GdTask task9) tasks)
	{
		return GdTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6, tasks.Item7, tasks.Item8, tasks.Item9).GetAwaiter();
	}


	public static GdTask.Awaiter GetAwaiter(this (GdTask task1, GdTask task2, GdTask task3, GdTask task4, GdTask task5, GdTask task6, GdTask task7, GdTask task8, GdTask task9, GdTask task10) tasks)
	{
		return GdTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6, tasks.Item7, tasks.Item8, tasks.Item9, tasks.Item10).GetAwaiter();
	}


	public static GdTask.Awaiter GetAwaiter(this (GdTask task1, GdTask task2, GdTask task3, GdTask task4, GdTask task5, GdTask task6, GdTask task7, GdTask task8, GdTask task9, GdTask task10, GdTask task11) tasks)
	{
		return GdTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6, tasks.Item7, tasks.Item8, tasks.Item9, tasks.Item10, tasks.Item11).GetAwaiter();
	}


	public static GdTask.Awaiter GetAwaiter(this (GdTask task1, GdTask task2, GdTask task3, GdTask task4, GdTask task5, GdTask task6, GdTask task7, GdTask task8, GdTask task9, GdTask task10, GdTask task11, GdTask task12) tasks)
	{
		return GdTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6, tasks.Item7, tasks.Item8, tasks.Item9, tasks.Item10, tasks.Item11, tasks.Item12).GetAwaiter();
	}


	public static GdTask.Awaiter GetAwaiter(this (GdTask task1, GdTask task2, GdTask task3, GdTask task4, GdTask task5, GdTask task6, GdTask task7, GdTask task8, GdTask task9, GdTask task10, GdTask task11, GdTask task12, GdTask task13) tasks)
	{
		return GdTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6, tasks.Item7, tasks.Item8, tasks.Item9, tasks.Item10, tasks.Item11, tasks.Item12, tasks.Item13).GetAwaiter();
	}


	public static GdTask.Awaiter GetAwaiter(this (GdTask task1, GdTask task2, GdTask task3, GdTask task4, GdTask task5, GdTask task6, GdTask task7, GdTask task8, GdTask task9, GdTask task10, GdTask task11, GdTask task12, GdTask task13, GdTask task14) tasks)
	{
		return GdTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6, tasks.Item7, tasks.Item8, tasks.Item9, tasks.Item10, tasks.Item11, tasks.Item12, tasks.Item13, tasks.Item14).GetAwaiter();
	}


	public static GdTask.Awaiter GetAwaiter(this (GdTask task1, GdTask task2, GdTask task3, GdTask task4, GdTask task5, GdTask task6, GdTask task7, GdTask task8, GdTask task9, GdTask task10, GdTask task11, GdTask task12, GdTask task13, GdTask task14, GdTask task15) tasks)
	{
		return GdTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6, tasks.Item7, tasks.Item8, tasks.Item9, tasks.Item10, tasks.Item11, tasks.Item12, tasks.Item13, tasks.Item14, tasks.Item15).GetAwaiter();
	}


}