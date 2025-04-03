using Godot;

namespace GdTasks.Structs;

// be careful to use, itself is struct.
public struct TriggerEvent<T>
{
	private ITriggerHandler<T> _head; // head.prev is last
	private ITriggerHandler<T> _iteratingHead;

	private ITriggerHandler<T> _iteratingNode;
	private bool _preserveRemoveSelf;
	public void Add(ITriggerHandler<T> handler)
	{
		if (handler == null) throw new ArgumentNullException(nameof(handler));

		// zero node.
		if (_head == null)
		{
			_head = handler;
			return;
		}

		if (_iteratingNode != null)
		{
			if (_iteratingHead == null)
			{
				_iteratingHead = handler;
				return;
			}

			var last = _iteratingHead.Prev;

			_iteratingHead.Prev = handler;

			if (last == null)
			{
				// single node.
				_iteratingHead.Next = handler;
				handler.Prev = _iteratingHead;
			}
			else
			{
				// multi node
				last.Next = handler;
				handler.Prev = last;
			}
		}
		else
		{
			var last = _head.Prev;

			_head.Prev = handler;

			if (last == null)
			{
				// single node.
				_head.Next = handler;
				handler.Prev = _head;
			}
			else
			{
				// multi node
				last.Next = handler;
				handler.Prev = last;
			}
		}
	}

	public void Remove(ITriggerHandler<T> handler)
	{
		if (handler == null) throw new ArgumentNullException(nameof(handler));

		if (_iteratingNode != null && _iteratingNode == handler)
		{
			// if remove self, reserve remove self after invoke completed.
			_preserveRemoveSelf = true;
		}
		else
		{
			var prev = handler.Prev;
			var next = handler.Next;

			if (next != null)
			{
				next.Prev = prev;
			}

			if (handler == _head)
			{
				_head = next;
			}
			else if (handler == _iteratingHead)
			{
				_iteratingHead = next;
			}
			else
			{
				// when handler is head, prev indicate last so don't use it.
				if (prev != null)
				{
					prev.Next = next;
				}
			}

			if (_head != null)
			{
				if (_head.Prev == handler)
				{
					_head.Prev = prev != _head
						? prev
						: null;
				}
			}

			if (_iteratingHead != null)
			{
				if (_iteratingHead.Prev == handler)
				{
					_iteratingHead.Prev = prev != _iteratingHead.Prev
						? prev
						: null;
				}
			}

			handler.Prev = null;
			handler.Next = null;
		}
	}

	public void SetCanceled(CancellationToken cancellationToken)
	{
		if (_iteratingNode != null)
			throw new InvalidOperationException("Can not trigger itself in iterating.");

		var h = _head;

		while (h != null)
		{
			_iteratingNode = h;
			try
			{
				h.OnCanceled(cancellationToken);
			}
			catch (Exception ex)
			{
				LogError(ex);
			}

			_preserveRemoveSelf = false;
			_iteratingNode = null;
			var next = h.Next;
			Remove(h);
			h = next;
		}

		_iteratingNode = null;

		if (_iteratingHead == null)
			return;

		Add(_iteratingHead);
		_iteratingHead = null;
	}

	public void SetCompleted()
	{
		if (_iteratingNode != null)
			throw new InvalidOperationException("Can not trigger itself in iterating.");

		var h = _head;

		while (h != null)
		{
			_iteratingNode = h;
			try
			{
				h.OnCompleted();
			}
			catch (Exception ex)
			{
				LogError(ex);
			}

			_preserveRemoveSelf = false;
			_iteratingNode = null;
			var next = h.Next;
			Remove(h);
			h = next;
		}

		_iteratingNode = null;

		if (_iteratingHead == null)
			return;

		Add(_iteratingHead);
		_iteratingHead = null;
	}

	public void SetError(Exception exception)
	{
		if (_iteratingNode != null)
			throw new InvalidOperationException("Can not trigger itself in iterating.");

		var h = _head;

		while (h != null)
		{
			_iteratingNode = h;
			try
			{
				h.OnError(exception);
			}
			catch (Exception ex)
			{
				LogError(ex);
			}

			_preserveRemoveSelf = false;
			_iteratingNode = null;
			var next = h.Next;
			Remove(h);
			h = next;
		}

		_iteratingNode = null;

		if (_iteratingHead == null)
			return;

		Add(_iteratingHead);
		_iteratingHead = null;
	}

	public void SetResult(T value)
	{
		if (_iteratingNode != null)
			throw new InvalidOperationException("Can not trigger itself in iterating.");

		var h = _head;

		while (h != null)
		{
			_iteratingNode = h;

			try
			{
				h.OnNext(value);
			}
			catch (Exception ex)
			{
				LogError(ex);
				Remove(h);
			}

			if (_preserveRemoveSelf)
			{
				_preserveRemoveSelf = false;
				_iteratingNode = null;
				var next = h.Next;
				Remove(h);
				h = next;
			}
			else
			{
				h = h.Next;
			}
		}

		_iteratingNode = null;

		if (_iteratingHead == null)
			return;

		Add(_iteratingHead);
		_iteratingHead = null;
	}

	private void LogError(Exception ex) => GD.PrintErr(ex);
}