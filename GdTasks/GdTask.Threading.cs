namespace GdTasks;

public partial struct GdTask
{
	/// <summary>
	/// Queue the action to PlayerLoop.
	/// </summary>
	public static void Post(Action action, PlayerLoopTiming timing = PlayerLoopTiming.Process)
		=> GdTaskPlayerLoopAutoload.AddContinuation(timing, action);

	public static ReturnToSynchronizationContext ReturnToCurrentSynchronizationContext(bool dontPostWhenSameContext = true, CancellationToken cancellationToken = default)
			=> new(SynchronizationContext.Current, dontPostWhenSameContext, cancellationToken);

	/// <summary>
	/// Return to mainthread(same as await SwitchToMainThread) after using scope is closed.
	/// </summary>
	public static ReturnToMainThread ReturnToMainThread(CancellationToken cancellationToken = default)
		=> new(PlayerLoopTiming.Process, cancellationToken);

	/// <summary>
	/// Return to mainthread(same as await SwitchToMainThread) after using scope is closed.
	/// </summary>
	public static ReturnToMainThread ReturnToMainThread(PlayerLoopTiming timing, CancellationToken cancellationToken = default)
		=> new(timing, cancellationToken);

	public static ReturnToSynchronizationContext ReturnToSynchronizationContext(SynchronizationContext synchronizationContext, CancellationToken cancellationToken = default)
			=> new(synchronizationContext, false, cancellationToken);

	/// <summary>
	/// If running on mainthread, do nothing. Otherwise, same as GDTask.Yield(PlayerLoopTiming.Update).
	/// </summary>
	public static SwitchToMainThreadAwaitable SwitchToMainThread(CancellationToken cancellationToken = default)
		=> new(PlayerLoopTiming.Process, cancellationToken);

	/// <summary>
	/// If running on mainthread, do nothing. Otherwise, same as GDTask.Yield(timing).
	/// </summary>
	public static SwitchToMainThreadAwaitable SwitchToMainThread(PlayerLoopTiming timing, CancellationToken cancellationToken = default)
		=> new(timing, cancellationToken);
	public static SwitchToSynchronizationContextAwaitable SwitchToSynchronizationContext(SynchronizationContext synchronizationContext, CancellationToken cancellationToken = default)
	{
		Error.ThrowArgumentNullException(synchronizationContext, nameof(synchronizationContext));
		return new SwitchToSynchronizationContextAwaitable(synchronizationContext, cancellationToken);
	}

	/// <summary>
	/// Note: use SwitchToThreadPool is recommended.
	/// </summary>
	public static SwitchToTaskPoolAwaitable SwitchToTaskPool() => new();

	public static SwitchToThreadPoolAwaitable SwitchToThreadPool() => new();
}