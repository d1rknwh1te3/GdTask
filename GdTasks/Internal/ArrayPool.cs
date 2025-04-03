namespace GdTasks.Internal;
// Same interface as System.Buffers.ArrayPool<T> but only provides Shared.

internal sealed class ArrayPool<T>
{
	public static readonly ArrayPool<T> Shared = new();

	// Same size as System.Buffers.DefaultArrayPool<T>
	private const int DefaultMaxNumberOfArraysPerBucket = 50;

	private static readonly T[] EmptyArray = [];
	private readonly MinimumQueue<T[]>[] _buckets;
	private readonly SpinLock[] _locks;

	private ArrayPool()
	{
		// see: GetQueueIndex
		_buckets = new MinimumQueue<T[]>[18];
		_locks = new SpinLock[18];

		for (var i = 0; i < _buckets.Length; i++)
		{
			_buckets[i] = new MinimumQueue<T[]>(4);
			_locks[i] = new SpinLock(false);
		}
	}

	public T[] Rent(int minimumLength)
	{
		switch (minimumLength)
		{
			case < 0: throw new ArgumentOutOfRangeException(nameof(minimumLength));
			case 0: return EmptyArray;
		}

		var size = CalculateSize(minimumLength);
		var index = GetQueueIndex(size);

		if (index == -1)
			return new T[size];

		var q = _buckets[index];
		var lockTaken = false;

		try
		{
			_locks[index].Enter(ref lockTaken);

			if (q.Count != 0)
			{
				return q.Dequeue();
			}
		}
		finally
		{
			if (lockTaken) _locks[index].Exit(false);
		}

		return new T[size];
	}

	public void Return(T[]? array, bool clearArray = false)
	{
		if (array == null || array.Length == 0)
			return;

		var index = GetQueueIndex(array.Length);

		if (index == -1)
			return;

		if (clearArray)
			Array.Clear(array, 0, array.Length);

		var q = _buckets[index];
		var lockTaken = false;

		try
		{
			_locks[index].Enter(ref lockTaken);

			if (q.Count > DefaultMaxNumberOfArraysPerBucket)
				return;

			q.Enqueue(array);
		}
		finally
		{
			if (lockTaken)
				_locks[index].Exit(false);
		}
	}

	private static int CalculateSize(int size)
	{
		size--;
		size |= size >> 1;
		size |= size >> 2;
		size |= size >> 4;
		size |= size >> 8;
		size |= size >> 16;
		size += 1;

		if (size < 8)
			size = 8;

		return size;
	}

	private static int GetQueueIndex(int size)
	{
		return size switch
		{
			8 => 0,
			16 => 1,
			32 => 2,
			64 => 3,
			128 => 4,
			256 => 5,
			512 => 6,
			1024 => 7,
			2048 => 8,
			4096 => 9,
			8192 => 10,
			16384 => 11,
			32768 => 12,
			65536 => 13,
			131072 => 14,
			262144 => 15,
			524288 => 16,
			1048576 => 17, // Max Array Size
			_ => -1
		};
	}
}