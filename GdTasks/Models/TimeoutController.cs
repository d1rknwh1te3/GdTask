using GdTasks.Models.Abstracts;

namespace GdTasks.Models;
// CancellationTokenSource itself can not reuse but CancelAfter(Timeout.InfiniteTimeSpan) allows reuse if did not reach timeout.
// Similar discussion:
// https://github.com/dotnet/runtime/issues/4694
// https://github.com/dotnet/runtime/issues/48492
// This TimeoutController emulate similar implementation, using CancelAfterSlim; to achieve zero allocation timeout.

public sealed class TimeoutController : IDisposable
{
	private static readonly Action<object> CancelCancellationTokenSourceStateDelegate = new Action<object>(CancelCancellationTokenSourceState);

	private readonly PlayerLoopTiming _delayTiming;

	private readonly DelayType _delayType;

	private readonly CancellationTokenSource _originalLinkCancellationTokenSource;

	private bool _isDisposed;

	private CancellationTokenSource _linkedSource;

	private CancellationTokenSource _timeoutSource;

	private PlayerLoopTimer _timer;

	public TimeoutController(DelayType delayType = DelayType.DeltaTime, PlayerLoopTiming delayTiming = PlayerLoopTiming.Process)
	{
		_timeoutSource = new CancellationTokenSource();
		_originalLinkCancellationTokenSource = null;
		_linkedSource = null;
		_delayType = delayType;
		_delayTiming = delayTiming;
	}

	public TimeoutController(CancellationTokenSource linkCancellationTokenSource, DelayType delayType = DelayType.DeltaTime, PlayerLoopTiming delayTiming = PlayerLoopTiming.Process)
	{
		_timeoutSource = new CancellationTokenSource();
		_originalLinkCancellationTokenSource = linkCancellationTokenSource;
		_linkedSource = CancellationTokenSource.CreateLinkedTokenSource(_timeoutSource.Token, linkCancellationTokenSource.Token);
		_delayType = delayType;
		_delayTiming = delayTiming;
	}

	public void Dispose()
	{
		if (_isDisposed) return;

		try
		{
			// stop timer.
			_timer?.Dispose();

			// cancel and dispose.
			_timeoutSource.Cancel();
			_timeoutSource.Dispose();

			if (_linkedSource == null)
				return;

			_linkedSource.Cancel();
			_linkedSource.Dispose();
		}
		finally
		{
			_isDisposed = true;
		}
	}

	public bool IsTimeout() => _timeoutSource.IsCancellationRequested;

	public void Reset() => _timer?.Stop();

	public CancellationToken Timeout(int millisecondsTimeout) => Timeout(TimeSpan.FromMilliseconds(millisecondsTimeout));

	public CancellationToken Timeout(TimeSpan timeout)
	{
		if (_originalLinkCancellationTokenSource is { IsCancellationRequested: true })
			return _originalLinkCancellationTokenSource.Token;

		// Timeouted, create new source and timer.
		if (_timeoutSource.IsCancellationRequested)
		{
			_timeoutSource.Dispose();
			_timeoutSource = new CancellationTokenSource();
			if (_linkedSource != null)
			{
				_linkedSource.Cancel();
				_linkedSource.Dispose();
				_linkedSource = CancellationTokenSource.CreateLinkedTokenSource(_timeoutSource.Token, _originalLinkCancellationTokenSource.Token);
			}

			_timer?.Dispose();
			_timer = null;
		}

		var useSource = (_linkedSource != null) ? _linkedSource : _timeoutSource;
		var token = useSource.Token;
		if (_timer == null)
		{
			// Timer complete => timeoutSource.Cancel() -> linkedSource will be canceled.
			// (linked)token is canceled => stop timer
			_timer = PlayerLoopTimer.StartNew(timeout, false, _delayType, _delayTiming, token, CancelCancellationTokenSourceStateDelegate, _timeoutSource);
		}
		else
		{
			_timer.Restart(timeout);
		}

		return token;
	}

	private static void CancelCancellationTokenSourceState(object state)
	{
		var cts = (CancellationTokenSource)state;
		cts.Cancel();
	}
}