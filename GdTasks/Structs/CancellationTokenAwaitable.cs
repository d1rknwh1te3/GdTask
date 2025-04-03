using GdTasks.Extensions;
using System.Runtime.CompilerServices;

namespace GdTasks.Structs;

public struct CancellationTokenAwaitable(CancellationToken cancellationToken)
{
	public Awaiter GetAwaiter() => new(cancellationToken);

	public struct Awaiter(CancellationToken cancellationToken) : ICriticalNotifyCompletion
	{
		public bool IsCompleted => !cancellationToken.CanBeCanceled || cancellationToken.IsCancellationRequested;

		public void GetResult()
		{ }

		public void OnCompleted(Action continuation) => UnsafeOnCompleted(continuation);

		public void UnsafeOnCompleted(Action continuation) => cancellationToken.RegisterWithoutCaptureExecutionContext(continuation);
	}
}