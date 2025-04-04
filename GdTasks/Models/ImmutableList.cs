﻿namespace GdTasks.Models;

internal class ImmutableList<T>(T[] data)
{
	public static readonly ImmutableList<T> Empty = new();

	private ImmutableList() : this([])
	{ }

	public T[] Data => data;
	public ImmutableList<T> Add(T value)
	{
		var newData = new T[data.Length + 1];
		Array.Copy(data, newData, data.Length);
		newData[data.Length] = value;
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
}