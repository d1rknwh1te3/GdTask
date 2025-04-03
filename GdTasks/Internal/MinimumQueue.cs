using System.Runtime.CompilerServices;

namespace GdTasks.Internal;

// optimized version of Standard Queue<T>.
internal class MinimumQueue<T>
{
	private const int GrowFactor = 200;
	private const int MinimumGrow = 4;
	private T[] _array;
	private int _head;
	private int _size;
	private int _tail;
	public MinimumQueue(int capacity)
	{
		if (capacity < 0) throw new ArgumentOutOfRangeException(nameof(capacity));
		_array = new T[capacity];
		_head = _tail = _size = 0;
	}

	public int Count
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => _size;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public T Dequeue()
	{
		if (_size == 0) ThrowForEmptyQueue();

		var head = _head;
		var array = _array;
		var removed = array[head];
		array[head] = default(T);

		MoveNext(ref _head);

		_size--;

		return removed;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Enqueue(T item)
	{
		if (_size == _array.Length)
			Grow();

		_array[_tail] = item;
		MoveNext(ref _tail);
		_size++;
	}

	public T Peek()
	{
		if (_size == 0)
			ThrowForEmptyQueue();

		return _array[_head];
	}
	private void Grow()
	{
		var newcapacity = (int)((long)_array.Length * (long)GrowFactor / 100);

		if (newcapacity < _array.Length + MinimumGrow)
			newcapacity = _array.Length + MinimumGrow;

		SetCapacity(newcapacity);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void MoveNext(ref int index)
	{
		var tmp = index + 1;

		if (tmp == _array.Length)
			tmp = 0;

		index = tmp;
	}

	private void SetCapacity(int capacity)
	{
		var newarray = new T[capacity];
		if (_size > 0)
		{
			if (_head < _tail)
			{
				Array.Copy(_array, _head, newarray, 0, _size);
			}
			else
			{
				Array.Copy(_array, _head, newarray, 0, _array.Length - _head);
				Array.Copy(_array, 0, newarray, _array.Length - _head, _tail);
			}
		}

		_array = newarray;
		_head = 0;
		_tail = _size == capacity ? 0 : _size;
	}
	private void ThrowForEmptyQueue() => throw new InvalidOperationException("EmptyQueue");
}