namespace GdTasks.Models.Triggers;

public sealed partial class AsyncReadyTrigger : AsyncTriggerBase<AsyncUnit>
{
	private bool _called;

	/// <inheritdoc/>
	public override void _Ready()
	{
		base._Ready();
		_called = true;
		RaiseEvent(AsyncUnit.Default);
	}

	public GdTask ReadyAsync()
	{
		return _called
			? GdTask.CompletedTask
			: ((IAsyncOneShotTrigger)new AsyncTriggerHandler<AsyncUnit>(this, true)).OneShotAsync();
	}
}