namespace GdTasks.Models.Disposable;

internal sealed class SingleAssignmentDisposable : IDisposable
{
	private readonly object _gate = new();
	private IDisposable? _current;
	private bool _disposed;

	public IDisposable? Disposable
	{
		get => _current;
		set
		{
			IDisposable? old;
			bool alreadyDisposed;

			lock (_gate)
			{
				alreadyDisposed = _disposed;
				old = _current;
				
				if (!alreadyDisposed)
				{
					if (value == null) 
						return;

					_current = value;
				}
			}

			if (alreadyDisposed && value != null)
			{
				value.Dispose();
				return;
			}

			if (old != null) 
				throw new InvalidOperationException("Disposable is already set");
		}
	}

	public bool IsDisposed { get { lock (_gate) { return _disposed; } } }
	public void Dispose()
	{
		IDisposable? old = null;

		lock (_gate)
		{
			if (!_disposed)
			{
				_disposed = true;
				old = _current;
				_current = null;
			}
		}

		old?.Dispose();
	}
}