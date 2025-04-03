using System.Runtime.CompilerServices;

namespace GdTasks.Structs;

public struct SwitchToThreadPoolAwaitable
{
	public Awaiter GetAwaiter() => new();

	public struct Awaiter : ICriticalNotifyCompletion
	{
		private static readonly WaitCallback SwitchToCallback = Callback;

		public bool IsCompleted => false;

		public void GetResult()
		{ }

		public void OnCompleted(Action continuation) => ThreadPool.QueueUserWorkItem(SwitchToCallback, continuation);

		public void UnsafeOnCompleted(Action continuation) => ThreadPool.UnsafeQueueUserWorkItem(SwitchToCallback, continuation);

		private static void Callback(object state)
		{
			var continuation = (Action)state;
			continuation();
		}
	}
}