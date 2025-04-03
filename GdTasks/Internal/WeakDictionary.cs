namespace GdTasks.Internal;

// Add, Remove, Enumerate with sweep. All operations are thread safe(in spinlock).
internal class WeakDictionary<TKey, TValue>
	where TKey : class
{
	private readonly IEqualityComparer<TKey> _keyEqualityComparer;
	private readonly float _loadFactor;
	private Entry[] _buckets;
	private SpinLock _gate;
	private int _size;
	// mutable struct (not readonly)
	public WeakDictionary(int capacity = 4, float loadFactor = 0.75f, IEqualityComparer<TKey> keyComparer = null)
	{
		var tableSize = CalculateCapacity(capacity, loadFactor);

		_buckets = new Entry[tableSize];
		_loadFactor = loadFactor;
		_gate = new SpinLock(false);
		_keyEqualityComparer = keyComparer ?? EqualityComparer<TKey>.Default;
	}

	public List<KeyValuePair<TKey, TValue>> ToList()
	{
		var list = new List<KeyValuePair<TKey, TValue>>(_size);
		ToList(ref list, false);
		return list;
	}

	// avoid allocate everytime.
	public int ToList(ref List<KeyValuePair<TKey, TValue>> list, bool clear = true)
	{
		if (clear)
			list.Clear();

		var listIndex = 0;
		var lockTaken = false;

		try
		{
			for (var i = 0; i < _buckets.Length; i++)
			{
				var entry = _buckets[i];

				while (entry != null)
				{
					if (entry.Key.TryGetTarget(out var target))
					{
						var item = new KeyValuePair<TKey, TValue>(target, entry.Value);
						if (listIndex < list.Count)
						{
							list[listIndex++] = item;
						}
						else
						{
							list.Add(item);
							listIndex++;
						}
					}
					else
					{
						// sweap
						Remove(i, entry);
					}

					entry = entry.Next;
				}
			}
		}
		finally
		{
			if (lockTaken) 
				_gate.Exit(false); // TODO: cause lockTaken is always false => code unreachable
		}

		return listIndex;
	}

	public bool TryAdd(TKey key, TValue value)
	{
		var lockTaken = false;

		try
		{
			_gate.Enter(ref lockTaken);
			return TryAddInternal(key, value);
		}
		finally
		{
			if (lockTaken) _gate.Exit(false);
		}
	}

	public bool TryGetValue(TKey key, out TValue? value)
	{
		var lockTaken = false;

		try
		{
			_gate.Enter(ref lockTaken);
			
			if (TryGetEntry(key, out _, out var entry))
			{
				value = entry.Value;
				return true;
			}

			value = default;

			return false;
		}
		finally
		{
			if (lockTaken) 
				_gate.Exit(false);
		}
	}

	public bool TryRemove(TKey key)
	{
		var lockTaken = false;
		try
		{
			_gate.Enter(ref lockTaken);

			if (!TryGetEntry(key, out var hashIndex, out var entry))
				return false;

			Remove(hashIndex, entry);
			return true;
		}
		finally
		{
			if (lockTaken) _gate.Exit(false);
		}
	}

	private static int CalculateCapacity(int collectionSize, float loadFactor)
	{
		var size = (int)(collectionSize / loadFactor);

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

	private bool AddToBuckets(Entry[] targetBuckets, TKey newKey, TValue value, int keyHash)
	{
		var hashIndex = keyHash & (targetBuckets.Length - 1);

	TRY_ADD_AGAIN:

		if (targetBuckets[hashIndex] == null)
		{
			targetBuckets[hashIndex] = new Entry
			{
				Key = new WeakReference<TKey>(newKey, false),
				Value = value,
				Hash = keyHash
			};

			return true;
		}

		// add to last.
		var entry = targetBuckets[hashIndex];
		while (entry != null)
		{
			if (entry.Key.TryGetTarget(out var target))
			{
				if (_keyEqualityComparer.Equals(newKey, target))
				{
					return false; // duplicate
				}
			}
			else
			{
				Remove(hashIndex, entry);
				if (targetBuckets[hashIndex] == null) goto TRY_ADD_AGAIN; // add new entry
			}

			if (entry.Next != null)
			{
				entry = entry.Next;
			}
			else
			{
				// found last
				entry.Next = new Entry
				{
					Key = new WeakReference<TKey>(newKey, false),
					Value = value,
					Hash = keyHash,
					Prev = entry
				};
			}
		}

		return false;
	}

	private void Remove(int hashIndex, Entry entry)
	{
		if (entry.Prev == null && entry.Next == null)
		{
			_buckets[hashIndex] = null;
		}
		else
		{
			if (entry.Prev == null) 
				_buckets[hashIndex] = entry.Next;

			if (entry.Prev != null) 
				entry.Prev.Next = entry.Next;

			if (entry.Next != null) 
				entry.Next.Prev = entry.Prev;
		}
		_size--;
	}

	private bool TryAddInternal(TKey key, TValue value)
	{
		var nextCapacity = CalculateCapacity(_size + 1, _loadFactor);

	TRY_ADD_AGAIN:
		if (_buckets.Length < nextCapacity)
		{
			// rehash
			var nextBucket = new Entry[nextCapacity];

			foreach (var t in _buckets)
			{
				var e = t;

				while (e != null)
				{
					AddToBuckets(nextBucket, key, e.Value, e.Hash);
					e = e.Next;
				}
			}

			_buckets = nextBucket;
			goto TRY_ADD_AGAIN;
		}

		var successAdd = AddToBuckets(_buckets, key, value, _keyEqualityComparer.GetHashCode(key));

		if (successAdd)
			_size++;

		return successAdd;
	}
	private bool TryGetEntry(TKey key, out int hashIndex, out Entry entry)
	{
		var table = _buckets;
		var hash = _keyEqualityComparer.GetHashCode(key);

		hashIndex = hash & table.Length - 1;
		entry = table[hashIndex];

		while (entry != null)
		{
			if (entry.Key.TryGetTarget(out var target))
			{
				if (_keyEqualityComparer.Equals(key, target))
				{
					return true;
				}
			}
			else
			{
				// sweap
				Remove(hashIndex, entry);
			}

			entry = entry.Next;
		}

		return false;
	}
	private class Entry
	{
		public int Hash;
		public WeakReference<TKey> Key;
		public Entry Next;
		public Entry Prev;
		public TValue Value;
		// debug only
		public override string ToString()
		{
			if (Key.TryGetTarget(out var target))
			{
				return $"{target}({Count()})";
			}

			return "(Dead)";
		}

		private int Count()
		{
			var count = 1;
			var n = this;

			while (n.Next != null)
			{
				count++;
				n = n.Next;
			}

			return count;
		}
	}
}