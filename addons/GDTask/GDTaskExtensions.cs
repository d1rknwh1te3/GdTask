using System;
using System.Collections;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Fractural.Tasks.Internal;

namespace Fractural.Tasks;

public static partial class GdTaskExtensions
{
	/// <summary>
	/// Convert Task[T] -> GDTask[T].
	/// </summary>
	public static GdTask<T> AsGdTask<T>(this Task<T> task, bool useCurrentSynchronizationContext = true)
	{
		var promise = new GdTaskCompletionSource<T>();

		task.ContinueWith((x, state) =>
		{
			var p = (GdTaskCompletionSource<T>)state;

			switch (x.Status)
			{
				case TaskStatus.Canceled:
					p.TrySetCanceled();
					break;
				case TaskStatus.Faulted:
					p.TrySetException(x.Exception);
					break;
				case TaskStatus.RanToCompletion:
					p.TrySetResult(x.Result);
					break;
				default:
					throw new NotSupportedException();
			}
		}, promise, useCurrentSynchronizationContext ? TaskScheduler.FromCurrentSynchronizationContext() : TaskScheduler.Current);

		return promise.Task;
	}

	/// <summary>
	/// Convert Task -> GDTask.
	/// </summary>
	public static GdTask AsGdTask(this Task task, bool useCurrentSynchronizationContext = true)
	{
		var promise = new GdTaskCompletionSource();

		task.ContinueWith((x, state) =>
		{
			var p = (GdTaskCompletionSource)state;

			switch (x.Status)
			{
				case TaskStatus.Canceled:
					p.TrySetCanceled();
					break;
				case TaskStatus.Faulted:
					p.TrySetException(x.Exception);
					break;
				case TaskStatus.RanToCompletion:
					p.TrySetResult();
					break;
				default:
					throw new NotSupportedException();
			}
		}, promise, useCurrentSynchronizationContext ? TaskScheduler.FromCurrentSynchronizationContext() : TaskScheduler.Current);

		return promise.Task;
	}

	public static Task<T> AsTask<T>(this GdTask<T> task)
	{
		try
		{
			GdTask<T>.Awaiter awaiter;
			try
			{
				awaiter = task.GetAwaiter();
			}
			catch (Exception ex)
			{
				return Task.FromException<T>(ex);
			}

			if (awaiter.IsCompleted)
			{
				try
				{
					var result = awaiter.GetResult();
					return Task.FromResult(result);
				}
				catch (Exception ex)
				{
					return Task.FromException<T>(ex);
				}
			}

			var tcs = new TaskCompletionSource<T>();

			awaiter.SourceOnCompleted(state =>
			{
				using (var tuple = (StateTuple<TaskCompletionSource<T>, GdTask<T>.Awaiter>)state)
				{
					var (inTcs, inAwaiter) = tuple;
					try
					{
						var result = inAwaiter.GetResult();
						inTcs.SetResult(result);
					}
					catch (Exception ex)
					{
						inTcs.SetException(ex);
					}
				}
			}, StateTuple.Create(tcs, awaiter));

			return tcs.Task;
		}
		catch (Exception ex)
		{
			return Task.FromException<T>(ex);
		}
	}

	public static Task AsTask(this GdTask task)
	{
		try
		{
			GdTask.Awaiter awaiter;
			try
			{
				awaiter = task.GetAwaiter();
			}
			catch (Exception ex)
			{
				return Task.FromException(ex);
			}

			if (awaiter.IsCompleted)
			{
				try
				{
					awaiter.GetResult(); // check token valid on Succeeded
					return Task.CompletedTask;
				}
				catch (Exception ex)
				{
					return Task.FromException(ex);
				}
			}

			var tcs = new TaskCompletionSource<object>();

			awaiter.SourceOnCompleted(state =>
			{
				using (var tuple = (StateTuple<TaskCompletionSource<object>, GdTask.Awaiter>)state)
				{
					var (inTcs, inAwaiter) = tuple;
					try
					{
						inAwaiter.GetResult();
						inTcs.SetResult(null);
					}
					catch (Exception ex)
					{
						inTcs.SetException(ex);
					}
				}
			}, StateTuple.Create(tcs, awaiter));

			return tcs.Task;
		}
		catch (Exception ex)
		{
			return Task.FromException(ex);
		}
	}

	public static AsyncLazy ToAsyncLazy(this GdTask task)
	{
		return new AsyncLazy(task);
	}

	public static AsyncLazy<T> ToAsyncLazy<T>(this GdTask<T> task)
	{
		return new AsyncLazy<T>(task);
	}

	/// <summary>
	/// Ignore task result when cancel raised first.
	/// </summary>
	public static GdTask AttachExternalCancellation(this GdTask task, CancellationToken cancellationToken)
	{
		if (!cancellationToken.CanBeCanceled)
		{
			return task;
		}

		if (cancellationToken.IsCancellationRequested)
		{
			return GdTask.FromCanceled(cancellationToken);
		}

		if (task.Status.IsCompleted())
		{
			return task;
		}

		return new GdTask(new AttachExternalCancellationSource(task, cancellationToken), 0);
	}

	/// <summary>
	/// Ignore task result when cancel raised first.
	/// </summary>
	public static GdTask<T> AttachExternalCancellation<T>(this GdTask<T> task, CancellationToken cancellationToken)
	{
		if (!cancellationToken.CanBeCanceled)
		{
			return task;
		}

		if (cancellationToken.IsCancellationRequested)
		{
			return GdTask.FromCanceled<T>(cancellationToken);
		}

		if (task.Status.IsCompleted())
		{
			return task;
		}

		return new GdTask<T>(new AttachExternalCancellationSource<T>(task, cancellationToken), 0);
	}

	private sealed class AttachExternalCancellationSource : IGdTaskSource
	{
		private static readonly Action<object> CancellationCallbackDelegate = CancellationCallback;

		private CancellationToken _cancellationToken;
		private CancellationTokenRegistration _tokenRegistration;
		private GdTaskCompletionSourceCore<AsyncUnit> _core;

		public AttachExternalCancellationSource(GdTask task, CancellationToken cancellationToken)
		{
			_cancellationToken = cancellationToken;
			_tokenRegistration = cancellationToken.RegisterWithoutCaptureExecutionContext(CancellationCallbackDelegate, this);
			RunTask(task).Forget();
		}

		private async GdTaskVoid RunTask(GdTask task)
		{
			try
			{
				await task;
				_core.TrySetResult(AsyncUnit.Default);
			}
			catch (Exception ex)
			{
				_core.TrySetException(ex);
			}
			finally
			{
				_tokenRegistration.Dispose();
			}
		}

		private static void CancellationCallback(object state)
		{
			var self = (AttachExternalCancellationSource)state;
			self._core.TrySetCanceled(self._cancellationToken);
		}

		public void GetResult(short token)
		{
			_core.GetResult(token);
		}

		public GdTaskStatus GetStatus(short token)
		{
			return _core.GetStatus(token);
		}

		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			_core.OnCompleted(continuation, state, token);
		}

		public GdTaskStatus UnsafeGetStatus()
		{
			return _core.UnsafeGetStatus();
		}
	}

	private sealed class AttachExternalCancellationSource<T> : IGdTaskSource<T>
	{
		private static readonly Action<object> CancellationCallbackDelegate = CancellationCallback;

		private CancellationToken _cancellationToken;
		private CancellationTokenRegistration _tokenRegistration;
		private GdTaskCompletionSourceCore<T> _core;

		public AttachExternalCancellationSource(GdTask<T> task, CancellationToken cancellationToken)
		{
			_cancellationToken = cancellationToken;
			_tokenRegistration = cancellationToken.RegisterWithoutCaptureExecutionContext(CancellationCallbackDelegate, this);
			RunTask(task).Forget();
		}

		private async GdTaskVoid RunTask(GdTask<T> task)
		{
			try
			{
				_core.TrySetResult(await task);
			}
			catch (Exception ex)
			{
				_core.TrySetException(ex);
			}
			finally
			{
				_tokenRegistration.Dispose();
			}
		}

		private static void CancellationCallback(object state)
		{
			var self = (AttachExternalCancellationSource<T>)state;
			self._core.TrySetCanceled(self._cancellationToken);
		}

		void IGdTaskSource.GetResult(short token)
		{
			_core.GetResult(token);
		}

		public T GetResult(short token)
		{
			return _core.GetResult(token);
		}

		public GdTaskStatus GetStatus(short token)
		{
			return _core.GetStatus(token);
		}

		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			_core.OnCompleted(continuation, state, token);
		}

		public GdTaskStatus UnsafeGetStatus()
		{
			return _core.UnsafeGetStatus();
		}
	}

	public static async GdTask Timeout(this GdTask task, TimeSpan timeout, DelayType delayType = DelayType.DeltaTime, PlayerLoopTiming timeoutCheckTiming = PlayerLoopTiming.Process, CancellationTokenSource taskCancellationTokenSource = null)
	{
		var delayCancellationTokenSource = new CancellationTokenSource();
		var timeoutTask = GdTask.Delay(timeout, delayType, timeoutCheckTiming, delayCancellationTokenSource.Token).SuppressCancellationThrow();

		int winArgIndex;
		bool taskResultIsCanceled;
		try
		{
			(winArgIndex, taskResultIsCanceled, _) = await GdTask.WhenAny(task.SuppressCancellationThrow(), timeoutTask);
		}
		catch
		{
			delayCancellationTokenSource.Cancel();
			delayCancellationTokenSource.Dispose();
			throw;
		}

		// timeout
		if (winArgIndex == 1)
		{
			if (taskCancellationTokenSource != null)
			{
				taskCancellationTokenSource.Cancel();
				taskCancellationTokenSource.Dispose();
			}

			throw new TimeoutException("Exceed Timeout:" + timeout);
		}
		else
		{
			delayCancellationTokenSource.Cancel();
			delayCancellationTokenSource.Dispose();
		}

		if (taskResultIsCanceled)
		{
			Error.ThrowOperationCanceledException();
		}
	}

	public static async GdTask<T> Timeout<T>(this GdTask<T> task, TimeSpan timeout, DelayType delayType = DelayType.DeltaTime, PlayerLoopTiming timeoutCheckTiming = PlayerLoopTiming.Process, CancellationTokenSource taskCancellationTokenSource = null)
	{
		var delayCancellationTokenSource = new CancellationTokenSource();
		var timeoutTask = GdTask.Delay(timeout, delayType, timeoutCheckTiming, delayCancellationTokenSource.Token).SuppressCancellationThrow();

		int winArgIndex;
		(bool IsCanceled, T Result) taskResult;
		try
		{
			(winArgIndex, taskResult, _) = await GdTask.WhenAny(task.SuppressCancellationThrow(), timeoutTask);
		}
		catch
		{
			delayCancellationTokenSource.Cancel();
			delayCancellationTokenSource.Dispose();
			throw;
		}

		// timeout
		if (winArgIndex == 1)
		{
			if (taskCancellationTokenSource != null)
			{
				taskCancellationTokenSource.Cancel();
				taskCancellationTokenSource.Dispose();
			}

			throw new TimeoutException("Exceed Timeout:" + timeout);
		}
		else
		{
			delayCancellationTokenSource.Cancel();
			delayCancellationTokenSource.Dispose();
		}

		if (taskResult.IsCanceled)
		{
			Error.ThrowOperationCanceledException();
		}

		return taskResult.Result;
	}

	/// <summary>
	/// Timeout with suppress OperationCanceledException. Returns (bool, IsCacneled).
	/// </summary>
	public static async GdTask<bool> TimeoutWithoutException(this GdTask task, TimeSpan timeout, DelayType delayType = DelayType.DeltaTime, PlayerLoopTiming timeoutCheckTiming = PlayerLoopTiming.Process, CancellationTokenSource taskCancellationTokenSource = null)
	{
		var delayCancellationTokenSource = new CancellationTokenSource();
		var timeoutTask = GdTask.Delay(timeout, delayType, timeoutCheckTiming, delayCancellationTokenSource.Token).SuppressCancellationThrow();

		int winArgIndex;
		bool taskResultIsCanceled;
		try
		{
			(winArgIndex, taskResultIsCanceled, _) = await GdTask.WhenAny(task.SuppressCancellationThrow(), timeoutTask);
		}
		catch
		{
			delayCancellationTokenSource.Cancel();
			delayCancellationTokenSource.Dispose();
			return true;
		}

		// timeout
		if (winArgIndex == 1)
		{
			if (taskCancellationTokenSource != null)
			{
				taskCancellationTokenSource.Cancel();
				taskCancellationTokenSource.Dispose();
			}

			return true;
		}
		else
		{
			delayCancellationTokenSource.Cancel();
			delayCancellationTokenSource.Dispose();
		}

		if (taskResultIsCanceled)
		{
			return true;
		}

		return false;
	}

	/// <summary>
	/// Timeout with suppress OperationCanceledException. Returns (bool IsTimeout, T Result).
	/// </summary>
	public static async GdTask<(bool IsTimeout, T Result)> TimeoutWithoutException<T>(this GdTask<T> task, TimeSpan timeout, DelayType delayType = DelayType.DeltaTime, PlayerLoopTiming timeoutCheckTiming = PlayerLoopTiming.Process, CancellationTokenSource taskCancellationTokenSource = null)
	{
		var delayCancellationTokenSource = new CancellationTokenSource();
		var timeoutTask = GdTask.Delay(timeout, delayType, timeoutCheckTiming, delayCancellationTokenSource.Token).SuppressCancellationThrow();

		int winArgIndex;
		(bool IsCanceled, T Result) taskResult;
		try
		{
			(winArgIndex, taskResult, _) = await GdTask.WhenAny(task.SuppressCancellationThrow(), timeoutTask);
		}
		catch
		{
			delayCancellationTokenSource.Cancel();
			delayCancellationTokenSource.Dispose();
			return (true, default);
		}

		// timeout
		if (winArgIndex == 1)
		{
			if (taskCancellationTokenSource != null)
			{
				taskCancellationTokenSource.Cancel();
				taskCancellationTokenSource.Dispose();
			}

			return (true, default);
		}
		else
		{
			delayCancellationTokenSource.Cancel();
			delayCancellationTokenSource.Dispose();
		}

		if (taskResult.IsCanceled)
		{
			return (true, default);
		}

		return (false, taskResult.Result);
	}

	public static void Forget(this GdTask task)
	{
		var awaiter = task.GetAwaiter();
		if (awaiter.IsCompleted)
		{
			try
			{
				awaiter.GetResult();
			}
			catch (Exception ex)
			{
				GdTaskScheduler.PublishUnobservedTaskException(ex);
			}
		}
		else
		{
			awaiter.SourceOnCompleted(state =>
			{
				using (var t = (StateTuple<GdTask.Awaiter>)state)
				{
					try
					{
						t.Item1.GetResult();
					}
					catch (Exception ex)
					{
						GdTaskScheduler.PublishUnobservedTaskException(ex);
					}
				}
			}, StateTuple.Create(awaiter));
		}
	}

	public static void Forget(this GdTask task, Action<Exception> exceptionHandler, bool handleExceptionOnMainThread = true)
	{
		if (exceptionHandler == null)
		{
			Forget(task);
		}
		else
		{
			ForgetCoreWithCatch(task, exceptionHandler, handleExceptionOnMainThread).Forget();
		}
	}

	private static async GdTaskVoid ForgetCoreWithCatch(GdTask task, Action<Exception> exceptionHandler, bool handleExceptionOnMainThread)
	{
		try
		{
			await task;
		}
		catch (Exception ex)
		{
			try
			{
				if (handleExceptionOnMainThread)
				{
					await GdTask.SwitchToMainThread();
				}
				exceptionHandler(ex);
			}
			catch (Exception ex2)
			{
				GdTaskScheduler.PublishUnobservedTaskException(ex2);
			}
		}
	}

	public static void Forget<T>(this GdTask<T> task)
	{
		var awaiter = task.GetAwaiter();
		if (awaiter.IsCompleted)
		{
			try
			{
				awaiter.GetResult();
			}
			catch (Exception ex)
			{
				GdTaskScheduler.PublishUnobservedTaskException(ex);
			}
		}
		else
		{
			awaiter.SourceOnCompleted(state =>
			{
				using (var t = (StateTuple<GdTask<T>.Awaiter>)state)
				{
					try
					{
						t.Item1.GetResult();
					}
					catch (Exception ex)
					{
						GdTaskScheduler.PublishUnobservedTaskException(ex);
					}
				}
			}, StateTuple.Create(awaiter));
		}
	}

	public static void Forget<T>(this GdTask<T> task, Action<Exception> exceptionHandler, bool handleExceptionOnMainThread = true)
	{
		if (exceptionHandler == null)
		{
			task.Forget();
		}
		else
		{
			ForgetCoreWithCatch(task, exceptionHandler, handleExceptionOnMainThread).Forget();
		}
	}

	private static async GdTaskVoid ForgetCoreWithCatch<T>(GdTask<T> task, Action<Exception> exceptionHandler, bool handleExceptionOnMainThread)
	{
		try
		{
			await task;
		}
		catch (Exception ex)
		{
			try
			{
				if (handleExceptionOnMainThread)
				{
					await GdTask.SwitchToMainThread();
				}
				exceptionHandler(ex);
			}
			catch (Exception ex2)
			{
				GdTaskScheduler.PublishUnobservedTaskException(ex2);
			}
		}
	}

	public static async GdTask ContinueWith<T>(this GdTask<T> task, Action<T> continuationFunction)
	{
		continuationFunction(await task);
	}

	public static async GdTask ContinueWith<T>(this GdTask<T> task, Func<T, GdTask> continuationFunction)
	{
		await continuationFunction(await task);
	}

	public static async GdTask<TR> ContinueWith<T, TR>(this GdTask<T> task, Func<T, TR> continuationFunction)
	{
		return continuationFunction(await task);
	}

	public static async GdTask<TR> ContinueWith<T, TR>(this GdTask<T> task, Func<T, GdTask<TR>> continuationFunction)
	{
		return await continuationFunction(await task);
	}

	public static async GdTask ContinueWith(this GdTask task, Action continuationFunction)
	{
		await task;
		continuationFunction();
	}

	public static async GdTask ContinueWith(this GdTask task, Func<GdTask> continuationFunction)
	{
		await task;
		await continuationFunction();
	}

	public static async GdTask<T> ContinueWith<T>(this GdTask task, Func<T> continuationFunction)
	{
		await task;
		return continuationFunction();
	}

	public static async GdTask<T> ContinueWith<T>(this GdTask task, Func<GdTask<T>> continuationFunction)
	{
		await task;
		return await continuationFunction();
	}

	public static async GdTask<T> Unwrap<T>(this GdTask<GdTask<T>> task)
	{
		return await await task;
	}

	public static async GdTask Unwrap(this GdTask<GdTask> task)
	{
		await await task;
	}

	public static async GdTask<T> Unwrap<T>(this Task<GdTask<T>> task)
	{
		return await await task;
	}

	public static async GdTask<T> Unwrap<T>(this Task<GdTask<T>> task, bool continueOnCapturedContext)
	{
		return await await task.ConfigureAwait(continueOnCapturedContext);
	}

	public static async GdTask Unwrap(this Task<GdTask> task)
	{
		await await task;
	}

	public static async GdTask Unwrap(this Task<GdTask> task, bool continueOnCapturedContext)
	{
		await await task.ConfigureAwait(continueOnCapturedContext);
	}

	public static async GdTask<T> Unwrap<T>(this GdTask<Task<T>> task)
	{
		return await await task;
	}

	public static async GdTask<T> Unwrap<T>(this GdTask<Task<T>> task, bool continueOnCapturedContext)
	{
		return await (await task).ConfigureAwait(continueOnCapturedContext);
	}

	public static async GdTask Unwrap(this GdTask<Task> task)
	{
		await await task;
	}

	public static async GdTask Unwrap(this GdTask<Task> task, bool continueOnCapturedContext)
	{
		await (await task).ConfigureAwait(continueOnCapturedContext);
	}
}