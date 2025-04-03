using System.Runtime.CompilerServices;
using GdTaskPlayerLoopAutoload = GdTasks.AutoLoader.GdTaskPlayerLoopAutoload;

namespace GdTasks.Structs;

public struct ReturnToMainThread(PlayerLoopTiming playerLoopTiming, CancellationToken cancellationToken)
{
	public Awaiter DisposeAsync() => new(playerLoopTiming, cancellationToken); // run immediate.

	public readonly struct Awaiter(PlayerLoopTiming timing, CancellationToken cancellationToken)
		: ICriticalNotifyCompletion
	{
		public bool IsCompleted => GdTaskPlayerLoopAutoload.MainThreadId == Thread.CurrentThread.ManagedThreadId;

		public Awaiter GetAwaiter() => this;
		public void GetResult() => cancellationToken.ThrowIfCancellationRequested();

		public void OnCompleted(Action continuation) => GdTaskPlayerLoopAutoload.AddContinuation(timing, continuation);

		public void UnsafeOnCompleted(Action continuation) => GdTaskPlayerLoopAutoload.AddContinuation(timing, continuation);
	}
}