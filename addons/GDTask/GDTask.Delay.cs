using Fractural.Tasks.Internal;
using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Fractural.Tasks;

public enum DelayType
{
	/// <summary>use Time.deltaTime.</summary>
	DeltaTime,
	/// <summary>use Stopwatch.GetTimestamp().</summary>
	Realtime
}

public partial struct GDTask
{
	public static YieldAwaitable Yield()
	{
		// optimized for single continuation
		return new YieldAwaitable(PlayerLoopTiming.Process);
	}

	public static YieldAwaitable Yield(PlayerLoopTiming timing)
	{
		// optimized for single continuation
		return new YieldAwaitable(timing);
	}

	public static GDTask Yield(CancellationToken cancellationToken)
	{
		return new GDTask(YieldPromise.Create(PlayerLoopTiming.Process, cancellationToken, out var token), token);
	}

	public static GDTask Yield(PlayerLoopTiming timing, CancellationToken cancellationToken)
	{
		return new GDTask(YieldPromise.Create(timing, cancellationToken, out var token), token);
	}

	/// <summary>
	/// Similar as GDTask.Yield but guaranteed run on next frame.
	/// </summary>
	public static GDTask NextFrame()
	{
		return new GDTask(NextFramePromise.Create(PlayerLoopTiming.Process, CancellationToken.None, out var token), token);
	}

	/// <summary>
	/// Similar as GDTask.Yield but guaranteed run on next frame.
	/// </summary>
	public static GDTask NextFrame(PlayerLoopTiming timing)
	{
		return new GDTask(NextFramePromise.Create(timing, CancellationToken.None, out var token), token);
	}

	/// <summary>
	/// Similar as GDTask.Yield but guaranteed run on next frame.
	/// </summary>
	public static GDTask NextFrame(CancellationToken cancellationToken)
	{
		return new GDTask(NextFramePromise.Create(PlayerLoopTiming.Process, cancellationToken, out var token), token);
	}

	/// <summary>
	/// Similar as GDTask.Yield but guaranteed run on next frame.
	/// </summary>
	public static GDTask NextFrame(PlayerLoopTiming timing, CancellationToken cancellationToken)
	{
		return new GDTask(NextFramePromise.Create(timing, cancellationToken, out var token), token);
	}

	public static YieldAwaitable WaitForEndOfFrame()
	{
		return GDTask.Yield(PlayerLoopTiming.Process);
	}

	public static GDTask WaitForEndOfFrame(CancellationToken cancellationToken)
	{
		return GDTask.Yield(PlayerLoopTiming.Process, cancellationToken);
	}

	/// <summary>
	/// Same as GDTask.Yield(PlayerLoopTiming.PhysicsProcess).
	/// </summary>
	public static YieldAwaitable WaitForPhysicsProcess()
	{
		return GDTask.Yield(PlayerLoopTiming.PhysicsProcess);
	}

	/// <summary>
	/// Same as GDTask.Yield(PlayerLoopTiming.PhysicsProcess, cancellationToken).
	/// </summary>
	public static GDTask WaitForPhysicsProcess(CancellationToken cancellationToken)
	{
		return GDTask.Yield(PlayerLoopTiming.PhysicsProcess, cancellationToken);
	}

	public static GDTask DelayFrame(int delayFrameCount, PlayerLoopTiming delayTiming = PlayerLoopTiming.Process, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (delayFrameCount < 0)
		{
			throw new ArgumentOutOfRangeException("Delay does not allow minus delayFrameCount. delayFrameCount:" + delayFrameCount);
		}

		return new GDTask(DelayFramePromise.Create(delayFrameCount, delayTiming, cancellationToken, out var token), token);
	}

	public static GDTask Delay(int millisecondsDelay, PlayerLoopTiming delayTiming = PlayerLoopTiming.Process, CancellationToken cancellationToken = default(CancellationToken))
	{
		var delayTimeSpan = TimeSpan.FromMilliseconds(millisecondsDelay);
		return Delay(delayTimeSpan, delayTiming, cancellationToken);
	}

	public static GDTask Delay(TimeSpan delayTimeSpan, PlayerLoopTiming delayTiming = PlayerLoopTiming.Process, CancellationToken cancellationToken = default(CancellationToken))
	{
		return Delay(delayTimeSpan, DelayType.DeltaTime, delayTiming, cancellationToken);
	}

	public static GDTask Delay(int millisecondsDelay, DelayType delayType, PlayerLoopTiming delayTiming = PlayerLoopTiming.Process, CancellationToken cancellationToken = default(CancellationToken))
	{
		var delayTimeSpan = TimeSpan.FromMilliseconds(millisecondsDelay);
		return Delay(delayTimeSpan, delayType, delayTiming, cancellationToken);
	}

	public static GDTask Delay(TimeSpan delayTimeSpan, DelayType delayType, PlayerLoopTiming delayTiming = PlayerLoopTiming.Process, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (delayTimeSpan < TimeSpan.Zero)
		{
			throw new ArgumentOutOfRangeException("Delay does not allow minus delayTimeSpan. delayTimeSpan:" + delayTimeSpan);
		}

#if DEBUG
		// force use Realtime.
		if (GDTaskPlayerLoopAutoload.IsMainThread && Engine.IsEditorHint())
		{
			delayType = DelayType.Realtime;
		}
#endif
		switch (delayType)
		{
			case DelayType.Realtime:
			{
				return new GDTask(DelayRealtimePromise.Create(delayTimeSpan, delayTiming, cancellationToken, out var token), token);
			}
			case DelayType.DeltaTime:
			default:
			{
				return new GDTask(DelayPromise.Create(delayTimeSpan, delayTiming, cancellationToken, out var token), token);
			}
		}
	}

	private sealed class YieldPromise : IGDTaskSource, IPlayerLoopItem, ITaskPoolNode<YieldPromise>
	{
		private static TaskPool<YieldPromise> pool;
		private YieldPromise nextNode;
		public ref YieldPromise NextNode => ref nextNode;

		static YieldPromise()
		{
			TaskPool.RegisterSizeGetter(typeof(YieldPromise), () => pool.Size);
		}

		private CancellationToken cancellationToken;
		private GDTaskCompletionSourceCore<object> core;

		private YieldPromise()
		{
		}

		public static IGDTaskSource Create(PlayerLoopTiming timing, CancellationToken cancellationToken, out short token)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return AutoResetGDTaskCompletionSource.CreateFromCanceled(cancellationToken, out token);
			}

			if (!pool.TryPop(out var result))
			{
				result = new YieldPromise();
			}


			result.cancellationToken = cancellationToken;

			TaskTracker.TrackActiveTask(result, 3);

			GDTaskPlayerLoopAutoload.AddAction(timing, result);

			token = result.core.Version;
			return result;
		}

		public void GetResult(short token)
		{
			try
			{
				core.GetResult(token);
			}
			finally
			{
				TryReturn();
			}
		}

		public GDTaskStatus GetStatus(short token)
		{
			return core.GetStatus(token);
		}

		public GDTaskStatus UnsafeGetStatus()
		{
			return core.UnsafeGetStatus();
		}

		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			core.OnCompleted(continuation, state, token);
		}

		public bool MoveNext()
		{
			if (cancellationToken.IsCancellationRequested)
			{
				core.TrySetCanceled(cancellationToken);
				return false;
			}

			core.TrySetResult(null);
			return false;
		}

		private bool TryReturn()
		{
			TaskTracker.RemoveTracking(this);
			core.Reset();
			cancellationToken = default;
			return pool.TryPush(this);
		}
	}

	private sealed class NextFramePromise : IGDTaskSource, IPlayerLoopItem, ITaskPoolNode<NextFramePromise>
	{
		private static TaskPool<NextFramePromise> pool;
		private NextFramePromise nextNode;
		public ref NextFramePromise NextNode => ref nextNode;

		static NextFramePromise()
		{
			TaskPool.RegisterSizeGetter(typeof(NextFramePromise), () => pool.Size);
		}

		private bool isMainThread;
		private ulong frameCount;
		private CancellationToken cancellationToken;
		private GDTaskCompletionSourceCore<AsyncUnit> core;

		private NextFramePromise()
		{
		}

		public static IGDTaskSource Create(PlayerLoopTiming timing, CancellationToken cancellationToken, out short token)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return AutoResetGDTaskCompletionSource.CreateFromCanceled(cancellationToken, out token);
			}

			if (!pool.TryPop(out var result))
			{
				result = new NextFramePromise();
			}

			result.isMainThread = GDTaskPlayerLoopAutoload.IsMainThread;
			if (result.isMainThread)
				result.frameCount = Engine.GetProcessFrames();
			result.cancellationToken = cancellationToken;

			TaskTracker.TrackActiveTask(result, 3);

			GDTaskPlayerLoopAutoload.AddAction(timing, result);

			token = result.core.Version;
			return result;
		}

		public void GetResult(short token)
		{
			try
			{
				core.GetResult(token);
			}
			finally
			{
				TryReturn();
			}
		}

		public GDTaskStatus GetStatus(short token)
		{
			return core.GetStatus(token);
		}

		public GDTaskStatus UnsafeGetStatus()
		{
			return core.UnsafeGetStatus();
		}

		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			core.OnCompleted(continuation, state, token);
		}

		public bool MoveNext()
		{
			if (cancellationToken.IsCancellationRequested)
			{
				core.TrySetCanceled(cancellationToken);
				return false;
			}

			if (isMainThread && frameCount == Engine.GetProcessFrames())
			{
				return true;
			}

			core.TrySetResult(AsyncUnit.Default);
			return false;
		}

		private bool TryReturn()
		{
			TaskTracker.RemoveTracking(this);
			core.Reset();
			cancellationToken = default;
			return pool.TryPush(this);
		}
	}

	private sealed class DelayFramePromise : IGDTaskSource, IPlayerLoopItem, ITaskPoolNode<DelayFramePromise>
	{
		private static TaskPool<DelayFramePromise> pool;
		private DelayFramePromise nextNode;
		public ref DelayFramePromise NextNode => ref nextNode;

		static DelayFramePromise()
		{
			TaskPool.RegisterSizeGetter(typeof(DelayFramePromise), () => pool.Size);
		}

		private bool isMainThread;
		private ulong initialFrame;
		private int delayFrameCount;
		private CancellationToken cancellationToken;

		private int currentFrameCount;
		private GDTaskCompletionSourceCore<AsyncUnit> core;

		private DelayFramePromise()
		{
		}

		public static IGDTaskSource Create(int delayFrameCount, PlayerLoopTiming timing, CancellationToken cancellationToken, out short token)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return AutoResetGDTaskCompletionSource.CreateFromCanceled(cancellationToken, out token);
			}

			if (!pool.TryPop(out var result))
			{
				result = new DelayFramePromise();
			}

			result.delayFrameCount = delayFrameCount;
			result.cancellationToken = cancellationToken;
			result.isMainThread = GDTaskPlayerLoopAutoload.IsMainThread;
			if (result.isMainThread)
				result.initialFrame = Engine.GetProcessFrames();

			TaskTracker.TrackActiveTask(result, 3);

			GDTaskPlayerLoopAutoload.AddAction(timing, result);

			token = result.core.Version;
			return result;
		}

		public void GetResult(short token)
		{
			try
			{
				core.GetResult(token);
			}
			finally
			{
				TryReturn();
			}
		}

		public GDTaskStatus GetStatus(short token)
		{
			return core.GetStatus(token);
		}

		public GDTaskStatus UnsafeGetStatus()
		{
			return core.UnsafeGetStatus();
		}

		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			core.OnCompleted(continuation, state, token);
		}

		public bool MoveNext()
		{
			if (cancellationToken.IsCancellationRequested)
			{
				core.TrySetCanceled(cancellationToken);
				return false;
			}

			if (currentFrameCount == 0)
			{
				if (delayFrameCount == 0) // same as Yield
				{
					core.TrySetResult(AsyncUnit.Default);
					return false;
				}

				// skip in initial frame.
				if (isMainThread && initialFrame == Engine.GetProcessFrames())
				{
#if DEBUG
					// force use Realtime.
					if (GDTaskPlayerLoopAutoload.IsMainThread && Engine.IsEditorHint())
					{
						//goto ++currentFrameCount
					}
					else
					{
						return true;
					}
#else
                        return true;
#endif
				}
			}

			if (++currentFrameCount >= delayFrameCount)
			{
				core.TrySetResult(AsyncUnit.Default);
				return false;
			}

			return true;
		}

		private bool TryReturn()
		{
			TaskTracker.RemoveTracking(this);
			core.Reset();
			currentFrameCount = default;
			delayFrameCount = default;
			cancellationToken = default;
			return pool.TryPush(this);
		}
	}

	private sealed class DelayPromise : IGDTaskSource, IPlayerLoopItem, ITaskPoolNode<DelayPromise>
	{
		private static TaskPool<DelayPromise> pool;
		private DelayPromise nextNode;
		public ref DelayPromise NextNode => ref nextNode;

		static DelayPromise()
		{
			TaskPool.RegisterSizeGetter(typeof(DelayPromise), () => pool.Size);
		}

		private bool isMainThread;
		private ulong initialFrame;
		private double delayTimeSpan;
		private double elapsed;
		private PlayerLoopTiming timing;
		private CancellationToken cancellationToken;

		private GDTaskCompletionSourceCore<object> core;

		private DelayPromise()
		{
		}

		public static IGDTaskSource Create(TimeSpan delayTimeSpan, PlayerLoopTiming timing, CancellationToken cancellationToken, out short token)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return AutoResetGDTaskCompletionSource.CreateFromCanceled(cancellationToken, out token);
			}

			if (!pool.TryPop(out var result))
			{
				result = new DelayPromise();
			}

			result.elapsed = 0.0f;
			result.delayTimeSpan = (float)delayTimeSpan.TotalSeconds;
			result.cancellationToken = cancellationToken;
			result.isMainThread = GDTaskPlayerLoopAutoload.IsMainThread;
			result.timing = timing;
			if (result.isMainThread)
				result.initialFrame = Engine.GetProcessFrames();

			TaskTracker.TrackActiveTask(result, 3);

			GDTaskPlayerLoopAutoload.AddAction(timing, result);

			token = result.core.Version;
			return result;
		}

		public void GetResult(short token)
		{
			try
			{
				core.GetResult(token);
			}
			finally
			{
				TryReturn();
			}
		}

		public GDTaskStatus GetStatus(short token)
		{
			return core.GetStatus(token);
		}

		public GDTaskStatus UnsafeGetStatus()
		{
			return core.UnsafeGetStatus();
		}

		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			core.OnCompleted(continuation, state, token);
		}

		public bool MoveNext()
		{
			if (cancellationToken.IsCancellationRequested)
			{
				core.TrySetCanceled(cancellationToken);
				return false;
			}

			if (elapsed == 0.0f)
			{
				if (isMainThread && initialFrame == Engine.GetProcessFrames())
				{
					return true;
				}
			}

			if (timing == PlayerLoopTiming.Process || timing == PlayerLoopTiming.PauseProcess)
				elapsed += GDTaskPlayerLoopAutoload.Global.DeltaTime;
			else
				elapsed += GDTaskPlayerLoopAutoload.Global.PhysicsDeltaTime;

			if (elapsed >= delayTimeSpan)
			{
				core.TrySetResult(null);
				return false;
			}

			return true;
		}

		private bool TryReturn()
		{
			TaskTracker.RemoveTracking(this);
			core.Reset();
			delayTimeSpan = default;
			elapsed = default;
			cancellationToken = default;
			return pool.TryPush(this);
		}
	}

	private sealed class DelayRealtimePromise : IGDTaskSource, IPlayerLoopItem, ITaskPoolNode<DelayRealtimePromise>
	{
		private static TaskPool<DelayRealtimePromise> pool;
		private DelayRealtimePromise nextNode;
		public ref DelayRealtimePromise NextNode => ref nextNode;

		static DelayRealtimePromise()
		{
			TaskPool.RegisterSizeGetter(typeof(DelayRealtimePromise), () => pool.Size);
		}

		private long delayTimeSpanTicks;
		private ValueStopwatch stopwatch;
		private CancellationToken cancellationToken;

		private GDTaskCompletionSourceCore<AsyncUnit> core;

		private DelayRealtimePromise()
		{
		}

		public static IGDTaskSource Create(TimeSpan delayTimeSpan, PlayerLoopTiming timing, CancellationToken cancellationToken, out short token)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return AutoResetGDTaskCompletionSource.CreateFromCanceled(cancellationToken, out token);
			}

			if (!pool.TryPop(out var result))
			{
				result = new DelayRealtimePromise();
			}

			result.stopwatch = ValueStopwatch.StartNew();
			result.delayTimeSpanTicks = delayTimeSpan.Ticks;
			result.cancellationToken = cancellationToken;

			TaskTracker.TrackActiveTask(result, 3);

			GDTaskPlayerLoopAutoload.AddAction(timing, result);

			token = result.core.Version;
			return result;
		}

		public void GetResult(short token)
		{
			try
			{
				core.GetResult(token);
			}
			finally
			{
				TryReturn();
			}
		}

		public GDTaskStatus GetStatus(short token)
		{
			return core.GetStatus(token);
		}

		public GDTaskStatus UnsafeGetStatus()
		{
			return core.UnsafeGetStatus();
		}

		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			core.OnCompleted(continuation, state, token);
		}

		public bool MoveNext()
		{
			if (cancellationToken.IsCancellationRequested)
			{
				core.TrySetCanceled(cancellationToken);
				return false;
			}

			if (stopwatch.IsInvalid)
			{
				core.TrySetResult(AsyncUnit.Default);
				return false;
			}

			if (stopwatch.ElapsedTicks >= delayTimeSpanTicks)
			{
				core.TrySetResult(AsyncUnit.Default);
				return false;
			}

			return true;
		}

		private bool TryReturn()
		{
			TaskTracker.RemoveTracking(this);
			core.Reset();
			stopwatch = default;
			cancellationToken = default;
			return pool.TryPush(this);
		}
	}
}

public readonly struct YieldAwaitable
{
	private readonly PlayerLoopTiming timing;

	public YieldAwaitable(PlayerLoopTiming timing)
	{
		this.timing = timing;
	}

	public Awaiter GetAwaiter()
	{
		return new Awaiter(timing);
	}

	public GDTask ToGDTask()
	{
		return GDTask.Yield(timing, CancellationToken.None);
	}

	public readonly struct Awaiter : ICriticalNotifyCompletion
	{
		private readonly PlayerLoopTiming timing;

		public Awaiter(PlayerLoopTiming timing)
		{
			this.timing = timing;
		}

		public bool IsCompleted => false;

		public void GetResult() { }

		public void OnCompleted(Action continuation)
		{
			GDTaskPlayerLoopAutoload.AddContinuation(timing, continuation);
		}

		public void UnsafeOnCompleted(Action continuation)
		{
			GDTaskPlayerLoopAutoload.AddContinuation(timing, continuation);
		}
	}
}