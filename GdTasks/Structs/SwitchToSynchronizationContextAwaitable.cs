using System.Runtime.CompilerServices;

namespace GdTasks.Structs;

public struct SwitchToSynchronizationContextAwaitable(
	SynchronizationContext synchronizationContext,
	CancellationToken cancellationToken)
{
	public Awaiter GetAwaiter() => new(synchronizationContext, cancellationToken);

	public struct Awaiter(SynchronizationContext synchronizationContext, CancellationToken cancellationToken)
		: ICriticalNotifyCompletion
	{
		private static readonly SendOrPostCallback SwitchToCallback = Callback;

		public bool IsCompleted => false;

		public void GetResult() => cancellationToken.ThrowIfCancellationRequested();

		public void OnCompleted(Action continuation) => synchronizationContext.Post(SwitchToCallback, continuation);

		public void UnsafeOnCompleted(Action continuation) => synchronizationContext.Post(SwitchToCallback, continuation);

		private static void Callback(object state)
		{
			var continuation = (Action)state;
			continuation();
		}
	}
}