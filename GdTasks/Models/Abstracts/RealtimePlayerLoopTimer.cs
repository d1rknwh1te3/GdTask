namespace GdTasks.Models.Abstracts;

internal sealed class RealtimePlayerLoopTimer : PlayerLoopTimer
{
	private long _intervalTicks;
	private ValueStopwatch _stopwatch;
	public RealtimePlayerLoopTimer(TimeSpan interval, bool periodic, PlayerLoopTiming playerLoopTiming, CancellationToken cancellationToken, Action<object> timerCallback, object state)
		: base(periodic, playerLoopTiming, cancellationToken, timerCallback, state)
		=> ResetCore(interval);

	protected override bool MoveNextCore() => _stopwatch.ElapsedTicks < _intervalTicks;

	protected override void ResetCore(TimeSpan? interval)
	{
		_stopwatch = ValueStopwatch.StartNew();

		if (interval != null)
			_intervalTicks = interval.Value.Ticks;
	}
}