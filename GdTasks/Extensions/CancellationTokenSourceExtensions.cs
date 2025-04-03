using GdTasks.Models.Abstracts;
using GdTasks.Models.Triggers;
using Godot;

namespace GdTasks.Extensions;

public static partial class CancellationTokenSourceExtensions
{
	private static readonly Action<object> CancelCancellationTokenSourceStateDelegate = CancelCancellationTokenSourceState;

	public static IDisposable CancelAfterSlim(this CancellationTokenSource cts, int millisecondsDelay, DelayType delayType = DelayType.DeltaTime, PlayerLoopTiming delayTiming = PlayerLoopTiming.Process)
		=> CancelAfterSlim(cts, TimeSpan.FromMilliseconds(millisecondsDelay), delayType, delayTiming);

	public static IDisposable CancelAfterSlim(this CancellationTokenSource cts, TimeSpan delayTimeSpan, DelayType delayType = DelayType.DeltaTime, PlayerLoopTiming delayTiming = PlayerLoopTiming.Process)
		=> PlayerLoopTimer.StartNew(delayTimeSpan, false, delayType, delayTiming, cts.Token, CancelCancellationTokenSourceStateDelegate, cts);

	public static void RegisterRaiseCancelOnDestroy(this CancellationTokenSource cts, Node node)
	{
		var trigger = node.GetAsyncDestroyTrigger();
		trigger.CancellationToken.RegisterWithoutCaptureExecutionContext(CancelCancellationTokenSourceStateDelegate, cts);
	}

	private static void CancelCancellationTokenSourceState(object state)
	{
		var cts = (CancellationTokenSource)state;
		cts.Cancel();
	}
}