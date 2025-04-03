namespace GdTasks.Extensions;

public static class CancellationTokenExtensions
{
	private static readonly Action<object> CancellationTokenCallback = Callback;
	private static readonly Action<object> disposeCallback = DisposeCallback;

	public static CancellationTokenRegistration AddTo(this IDisposable disposable, CancellationToken cancellationToken)
		=> cancellationToken.RegisterWithoutCaptureExecutionContext(disposeCallback, disposable);

	public static CancellationTokenRegistration RegisterWithoutCaptureExecutionContext(this CancellationToken cancellationToken, Action callback)
	{
		var restoreFlow = false;
		if (!ExecutionContext.IsFlowSuppressed())
		{
			ExecutionContext.SuppressFlow();
			restoreFlow = true;
		}

		try
		{
			return cancellationToken.Register(callback, false);
		}
		finally
		{
			if (restoreFlow)
			{
				ExecutionContext.RestoreFlow();
			}
		}
	}

	public static CancellationTokenRegistration RegisterWithoutCaptureExecutionContext(this CancellationToken cancellationToken, Action<object> callback, object state)
	{
		var restoreFlow = false;
		if (!ExecutionContext.IsFlowSuppressed())
		{
			ExecutionContext.SuppressFlow();
			restoreFlow = true;
		}

		try
		{
			return cancellationToken.Register(callback, state, false);
		}
		finally
		{
			if (restoreFlow)
			{
				ExecutionContext.RestoreFlow();
			}
		}
	}

	public static CancellationToken ToCancellationToken(this GdTask task)
	{
		var cts = new CancellationTokenSource();
		ToCancellationTokenCore(task, cts).Forget();
		return cts.Token;
	}

	public static CancellationToken ToCancellationToken(this GdTask task, CancellationToken linkToken)
	{
		if (linkToken.IsCancellationRequested)
			return linkToken;

		if (!linkToken.CanBeCanceled)
			return ToCancellationToken(task);

		var cts = CancellationTokenSource.CreateLinkedTokenSource(linkToken);
		ToCancellationTokenCore(task, cts).Forget();

		return cts.Token;
	}

	public static CancellationToken ToCancellationToken<T>(this GdTask<T> task)
		=> ToCancellationToken(task.AsGdTask());

	public static CancellationToken ToCancellationToken<T>(this GdTask<T> task, CancellationToken linkToken)
		=> ToCancellationToken(task.AsGdTask(), linkToken);

	public static (GdTask, CancellationTokenRegistration) ToGdTask(this CancellationToken cancellationToken)
	{
		if (cancellationToken.IsCancellationRequested)
			return (GdTask.FromCanceled(cancellationToken), default(CancellationTokenRegistration));

		var promise = new GdTaskCompletionSource();
		return (promise.Task, cancellationToken.RegisterWithoutCaptureExecutionContext(CancellationTokenCallback, promise));
	}

	public static CancellationTokenAwaitable WaitUntilCanceled(this CancellationToken cancellationToken)
	{
		return new CancellationTokenAwaitable(cancellationToken);
	}

	private static void Callback(object state)
	{
		var promise = (GdTaskCompletionSource)state;
		promise.TrySetResult();
	}

	private static void DisposeCallback(object state)
	{
		var d = (IDisposable)state;
		d.Dispose();
	}

	private static async GdTaskVoid ToCancellationTokenCore(GdTask task, CancellationTokenSource cts)
	{
		try
		{
			await task;
		}
		catch (Exception ex)
		{
			GdTaskScheduler.PublishUnobservedTaskException(ex);
		}

		await cts.CancelAsync();
		cts.Dispose();
	}
}