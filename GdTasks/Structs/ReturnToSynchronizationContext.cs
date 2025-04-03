using System.Runtime.CompilerServices;

namespace GdTasks.Structs;

public struct ReturnToSynchronizationContext(
	SynchronizationContext syncContext,
	bool dontPostWhenSameContext,
	CancellationToken cancellationToken)
{
	public Awaiter DisposeAsync()
	{
		return new Awaiter(syncContext, dontPostWhenSameContext, cancellationToken);
	}

	public struct Awaiter(
		SynchronizationContext synchronizationContext,
		bool dontPostWhenSameContext,
		CancellationToken cancellationToken)
		: ICriticalNotifyCompletion
	{
		private static readonly SendOrPostCallback SwitchToCallback = Callback;

		public bool IsCompleted
		{
			get
			{
				if (!dontPostWhenSameContext) return false;

				var current = SynchronizationContext.Current;
				return current == synchronizationContext;
			}
		}

		public Awaiter GetAwaiter() => this;
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