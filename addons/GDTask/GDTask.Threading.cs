using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Fractural.Tasks.Internal;

namespace Fractural.Tasks;

public partial struct GdTask
{

	/// <summary>
	/// If running on mainthread, do nothing. Otherwise, same as GDTask.Yield(PlayerLoopTiming.Update).
	/// </summary>
	public static SwitchToMainThreadAwaitable SwitchToMainThread(CancellationToken cancellationToken = default)
	{
		return new SwitchToMainThreadAwaitable(PlayerLoopTiming.Process, cancellationToken);
	}

	/// <summary>
	/// If running on mainthread, do nothing. Otherwise, same as GDTask.Yield(timing).
	/// </summary>
	public static SwitchToMainThreadAwaitable SwitchToMainThread(PlayerLoopTiming timing, CancellationToken cancellationToken = default)
	{
		return new SwitchToMainThreadAwaitable(timing, cancellationToken);
	}

	/// <summary>
	/// Return to mainthread(same as await SwitchToMainThread) after using scope is closed.
	/// </summary>
	public static ReturnToMainThread ReturnToMainThread(CancellationToken cancellationToken = default)
	{
		return new ReturnToMainThread(PlayerLoopTiming.Process, cancellationToken);
	}

	/// <summary>
	/// Return to mainthread(same as await SwitchToMainThread) after using scope is closed.
	/// </summary>
	public static ReturnToMainThread ReturnToMainThread(PlayerLoopTiming timing, CancellationToken cancellationToken = default)
	{
		return new ReturnToMainThread(timing, cancellationToken);
	}

	/// <summary>
	/// Queue the action to PlayerLoop.
	/// </summary>
	public static void Post(Action action, PlayerLoopTiming timing = PlayerLoopTiming.Process)
	{
		GdTaskPlayerLoopAutoload.AddContinuation(timing, action);
	}


	public static SwitchToThreadPoolAwaitable SwitchToThreadPool()
	{
		return new SwitchToThreadPoolAwaitable();
	}

	/// <summary>
	/// Note: use SwitchToThreadPool is recommended.
	/// </summary>
	public static SwitchToTaskPoolAwaitable SwitchToTaskPool()
	{
		return new SwitchToTaskPoolAwaitable();
	}

	public static SwitchToSynchronizationContextAwaitable SwitchToSynchronizationContext(SynchronizationContext synchronizationContext, CancellationToken cancellationToken = default)
	{
		Error.ThrowArgumentNullException(synchronizationContext, nameof(synchronizationContext));
		return new SwitchToSynchronizationContextAwaitable(synchronizationContext, cancellationToken);
	}

	public static ReturnToSynchronizationContext ReturnToSynchronizationContext(SynchronizationContext synchronizationContext, CancellationToken cancellationToken = default)
	{
		return new ReturnToSynchronizationContext(synchronizationContext, false, cancellationToken);
	}

	public static ReturnToSynchronizationContext ReturnToCurrentSynchronizationContext(bool dontPostWhenSameContext = true, CancellationToken cancellationToken = default)
	{
		return new ReturnToSynchronizationContext(SynchronizationContext.Current, dontPostWhenSameContext, cancellationToken);
	}
}

public struct SwitchToMainThreadAwaitable(PlayerLoopTiming playerLoopTiming, CancellationToken cancellationToken)
{
	public Awaiter GetAwaiter() => new Awaiter(playerLoopTiming, cancellationToken);

	public struct Awaiter(PlayerLoopTiming playerLoopTiming, CancellationToken cancellationToken)
		: ICriticalNotifyCompletion
	{
		public bool IsCompleted
		{
			get
			{
				var currentThreadId = Thread.CurrentThread.ManagedThreadId;
				if (GdTaskPlayerLoopAutoload.MainThreadId == currentThreadId)
				{
					return true; // run immediate.
				}
				else
				{
					return false; // register continuation.
				}
			}
		}

		public void GetResult() { cancellationToken.ThrowIfCancellationRequested(); }

		public void OnCompleted(Action continuation)
		{
			GdTaskPlayerLoopAutoload.AddContinuation(playerLoopTiming, continuation);
		}

		public void UnsafeOnCompleted(Action continuation)
		{
			GdTaskPlayerLoopAutoload.AddContinuation(playerLoopTiming, continuation);
		}
	}
}

public struct ReturnToMainThread(PlayerLoopTiming playerLoopTiming, CancellationToken cancellationToken)
{
	public Awaiter DisposeAsync()
	{
		return new Awaiter(playerLoopTiming, cancellationToken); // run immediate.
	}

	public readonly struct Awaiter(PlayerLoopTiming timing, CancellationToken cancellationToken)
		: ICriticalNotifyCompletion
	{
		public Awaiter GetAwaiter() => this;

		public bool IsCompleted => GdTaskPlayerLoopAutoload.MainThreadId == Thread.CurrentThread.ManagedThreadId;

		public void GetResult() { cancellationToken.ThrowIfCancellationRequested(); }

		public void OnCompleted(Action continuation)
		{
			GdTaskPlayerLoopAutoload.AddContinuation(timing, continuation);
		}

		public void UnsafeOnCompleted(Action continuation)
		{
			GdTaskPlayerLoopAutoload.AddContinuation(timing, continuation);
		}
	}
}


public struct SwitchToThreadPoolAwaitable
{
	public Awaiter GetAwaiter() => new Awaiter();

	public struct Awaiter : ICriticalNotifyCompletion
	{
		private static readonly WaitCallback SwitchToCallback = Callback;

		public bool IsCompleted => false;
		public void GetResult() { }

		public void OnCompleted(Action continuation)
		{
			ThreadPool.QueueUserWorkItem(SwitchToCallback, continuation);
		}

		public void UnsafeOnCompleted(Action continuation)
		{
			ThreadPool.UnsafeQueueUserWorkItem(SwitchToCallback, continuation);
		}

		private static void Callback(object state)
		{
			var continuation = (Action)state;
			continuation();
		}
	}
}

public struct SwitchToTaskPoolAwaitable
{
	public Awaiter GetAwaiter() => new Awaiter();

	public struct Awaiter : ICriticalNotifyCompletion
	{
		private static readonly Action<object> SwitchToCallback = Callback;

		public bool IsCompleted => false;
		public void GetResult() { }

		public void OnCompleted(Action continuation)
		{
			Task.Factory.StartNew(SwitchToCallback, continuation, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
		}

		public void UnsafeOnCompleted(Action continuation)
		{
			Task.Factory.StartNew(SwitchToCallback, continuation, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
		}

		private static void Callback(object state)
		{
			var continuation = (Action)state;
			continuation();
		}
	}
}

public struct SwitchToSynchronizationContextAwaitable(
	SynchronizationContext synchronizationContext,
	CancellationToken cancellationToken)
{
	public Awaiter GetAwaiter() => new Awaiter(synchronizationContext, cancellationToken);

	public struct Awaiter(SynchronizationContext synchronizationContext, CancellationToken cancellationToken)
		: ICriticalNotifyCompletion
	{
		private static readonly SendOrPostCallback SwitchToCallback = Callback;

		public bool IsCompleted => false;
		public void GetResult() { cancellationToken.ThrowIfCancellationRequested(); }

		public void OnCompleted(Action continuation)
		{
			synchronizationContext.Post(SwitchToCallback, continuation);
		}

		public void UnsafeOnCompleted(Action continuation)
		{
			synchronizationContext.Post(SwitchToCallback, continuation);
		}

		private static void Callback(object state)
		{
			var continuation = (Action)state;
			continuation();
		}
	}
}

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

		public Awaiter GetAwaiter() => this;

		public bool IsCompleted
		{
			get
			{
				if (!dontPostWhenSameContext) return false;

				var current = SynchronizationContext.Current;
				if (current == synchronizationContext)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		public void GetResult() { cancellationToken.ThrowIfCancellationRequested(); }

		public void OnCompleted(Action continuation)
		{
			synchronizationContext.Post(SwitchToCallback, continuation);
		}

		public void UnsafeOnCompleted(Action continuation)
		{
			synchronizationContext.Post(SwitchToCallback, continuation);
		}

		private static void Callback(object state)
		{
			var continuation = (Action)state;
			continuation();
		}
	}
}