using Godot;
using GdTaskPlayerLoopAutoload = GdTasks.AutoLoader.GdTaskPlayerLoopAutoload;

namespace GdTasks.Models.Abstracts;

public abstract class PlayerLoopTimer(
	bool periodic,
	PlayerLoopTiming playerLoopTiming,
	CancellationToken cancellationToken,
	Action<object> timerCallback,
	object state)
	: IDisposable, IPlayerLoopItem
{
	private bool _isRunning;
	private bool _tryStop;
	private bool _isDisposed;

	public static PlayerLoopTimer Create(TimeSpan interval, bool periodic, DelayType delayType, PlayerLoopTiming playerLoopTiming, CancellationToken cancellationToken, Action<object> timerCallback, object state)
	{
#if DEBUG
		// force use Realtime.
		if (GdTaskPlayerLoopAutoload.IsMainThread && Engine.IsEditorHint())
			delayType = DelayType.RealTime;
#endif

		switch (delayType)
		{
			case DelayType.RealTime:
				return new RealtimePlayerLoopTimer(interval, periodic, playerLoopTiming, cancellationToken, timerCallback, state);

			case DelayType.DeltaTime:
			default:
				return new DeltaTimePlayerLoopTimer(interval, periodic, playerLoopTiming, cancellationToken, timerCallback, state);
		}
	}

	public static PlayerLoopTimer StartNew(TimeSpan interval, bool periodic, DelayType delayType, PlayerLoopTiming playerLoopTiming, CancellationToken cancellationToken, Action<object> timerCallback, object state)
	{
		var timer = Create(interval, periodic, delayType, playerLoopTiming, cancellationToken, timerCallback, state);
		timer.Restart();
		return timer;
	}

	/// <summary>
	/// Restart(Reset and Start) timer.
	/// </summary>
	public void Restart()
	{
		if (_isDisposed) throw new ObjectDisposedException(null);

		ResetCore(null); // init state

		if (!_isRunning)
		{
			_isRunning = true;
			GdTaskPlayerLoopAutoload.AddAction(playerLoopTiming, this);
		}
		_tryStop = false;
	}

	/// <summary>
	/// Restart(Reset and Start) and change interval.
	/// </summary>
	public void Restart(TimeSpan interval)
	{
		if (_isDisposed) throw new ObjectDisposedException(null);

		ResetCore(interval); // init state
		if (!_isRunning)
		{
			_isRunning = true;
			GdTaskPlayerLoopAutoload.AddAction(playerLoopTiming, this);
		}
		_tryStop = false;
	}

	/// <summary>
	/// Stop timer.
	/// </summary>
	public void Stop()
	{
		_tryStop = true;
	}

	protected abstract void ResetCore(TimeSpan? newInterval);

	public void Dispose() => _isDisposed = true;

	bool IPlayerLoopItem.MoveNext()
	{
		if (_isDisposed)
		{
			_isRunning = false;
			return false;
		}

		if (_tryStop)
		{
			_isRunning = false;
			return false;
		}

		if (cancellationToken.IsCancellationRequested)
		{
			_isRunning = false;
			return false;
		}

		if (MoveNextCore())
			return true;

		timerCallback(state);

		if (periodic)
		{
			ResetCore(null);
			return true;
		}

		_isRunning = false;
		return false;
	}

	protected abstract bool MoveNextCore();
}