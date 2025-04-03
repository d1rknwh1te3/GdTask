using System.Runtime.CompilerServices;
using GdTaskPlayerLoopAutoload = GdTasks.AutoLoader.GdTaskPlayerLoopAutoload;

namespace GdTasks.Structs;

public readonly struct YieldAwaitable(PlayerLoopTiming timing)
{
	public Awaiter GetAwaiter() => new(timing);

	public GdTask ToGdTask() => GdTask.Yield(timing, CancellationToken.None);

	public readonly struct Awaiter(PlayerLoopTiming timing) : ICriticalNotifyCompletion
	{
		public bool IsCompleted => false;

		public void GetResult()
		{ }

		public void OnCompleted(Action continuation) => GdTaskPlayerLoopAutoload.AddContinuation(timing, continuation);

		public void UnsafeOnCompleted(Action continuation) => GdTaskPlayerLoopAutoload.AddContinuation(timing, continuation);
	}
}