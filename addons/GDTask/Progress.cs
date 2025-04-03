using System;
using System.Collections.Generic;
using Fractural.Tasks.Internal;

namespace Fractural.Tasks;

/// <summary>
/// Lightweight IProgress[T] factory.
/// </summary>
public static class Progress
{
	public static IProgress<T> Create<T>(Action<T> handler)
	{
		if (handler == null) return NullProgress<T>.Instance;
		return new AnonymousProgress<T>(handler);
	}

	public static IProgress<T> CreateOnlyValueChanged<T>(Action<T> handler, IEqualityComparer<T> comparer = null)
	{
		if (handler == null) return NullProgress<T>.Instance;
		return new OnlyValueChangedProgress<T>(handler, comparer ?? GodotEqualityComparer.GetDefault<T>());
	}

	private sealed class NullProgress<T> : IProgress<T>
	{
		public static readonly IProgress<T> Instance = new NullProgress<T>();

		private NullProgress()
		{

		}

		public void Report(T value)
		{
		}
	}

	private sealed class AnonymousProgress<T>(Action<T> action) : IProgress<T>
	{
		public void Report(T value)
		{
			action(value);
		}
	}

	private sealed class OnlyValueChangedProgress<T>(Action<T> action, IEqualityComparer<T> comparer) : IProgress<T>
	{
		private bool _isFirstCall = true;
		private T _latestValue;

		public void Report(T value)
		{
			if (_isFirstCall)
			{
				_isFirstCall = false;
			}
			else if (comparer.Equals(value, _latestValue))
			{
				return;
			}

			_latestValue = value;
			action(value);
		}
	}
}