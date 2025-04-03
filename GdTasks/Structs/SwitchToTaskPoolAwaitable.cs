using System.Runtime.CompilerServices;

namespace GdTasks.Structs;

public struct SwitchToTaskPoolAwaitable
{
	public Awaiter GetAwaiter() => new();

	public struct Awaiter : ICriticalNotifyCompletion
	{
		private static readonly Action<object> SwitchToCallback = Callback;

		public bool IsCompleted => false;

		public void GetResult()
		{ }

		public void OnCompleted(Action continuation) => Task.Factory.StartNew(SwitchToCallback, continuation, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);

		public void UnsafeOnCompleted(Action continuation) => Task.Factory.StartNew(SwitchToCallback, continuation, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);

		private static void Callback(object state)
		{
			var continuation = (Action)state;
			continuation();
		}
	}
}