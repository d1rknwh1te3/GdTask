namespace GdTasks.Models.Abstracts;

public abstract class MoveNextSource : IGdTaskSource<bool>
{
	protected GdTaskCompletionSourceCore<bool> CompletionSource;

	public bool GetResult(short token) => CompletionSource.GetResult(token);

	void IGdTaskSource.GetResult(short token) => CompletionSource.GetResult(token);

	public GdTaskStatus GetStatus(short token) => CompletionSource.GetStatus(token);

	public void OnCompleted(Action<object> continuation, object state, short token)
		=> CompletionSource.OnCompleted(continuation, state, token);

	public GdTaskStatus UnsafeGetStatus() => CompletionSource.UnsafeGetStatus();
	protected bool TryGetResult<T>(GdTask<T>.Awaiter awaiter, out T result)
	{
		try
		{
			result = awaiter.GetResult();
			return true;
		}
		catch (Exception ex)
		{
			CompletionSource.TrySetException(ex);
			result = default;
			return false;
		}
	}

	protected bool TryGetResult(GdTask.Awaiter awaiter)
	{
		try
		{
			awaiter.GetResult();
			return true;
		}
		catch (Exception ex)
		{
			CompletionSource.TrySetException(ex);
			return false;
		}
	}
}