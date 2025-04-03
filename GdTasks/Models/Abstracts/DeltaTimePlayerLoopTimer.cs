using Godot;
using GdTaskPlayerLoopAutoload = GdTasks.AutoLoader.GdTaskPlayerLoopAutoload;

namespace GdTasks.Models.Abstracts;

internal sealed class DeltaTimePlayerLoopTimer : PlayerLoopTimer
{
	private double _elapsed;
	private ulong _initialFrame;
	private double _interval;
	private bool _isMainThread;
	public DeltaTimePlayerLoopTimer(TimeSpan interval, bool periodic, PlayerLoopTiming playerLoopTiming, CancellationToken cancellationToken, Action<object> timerCallback, object state)
		: base(periodic, playerLoopTiming, cancellationToken, timerCallback, state)
	{
		ResetCore(interval);
	}

	protected override bool MoveNextCore()
	{
		if (_elapsed == 0.0)
		{
			if (_isMainThread && _initialFrame == Engine.GetProcessFrames())
			{
				return true;
			}
		}

		_elapsed += GdTaskPlayerLoopAutoload.Global.DeltaTime;

		return !(_elapsed >= _interval);
	}

	protected override void ResetCore(TimeSpan? interval)
	{
		_elapsed = 0.0;
		_isMainThread = GdTaskPlayerLoopAutoload.IsMainThread;

		if (_isMainThread)
			_initialFrame = Engine.GetProcessFrames();

		if (interval != null)
			_interval = (float)interval.Value.TotalSeconds;
	}
}