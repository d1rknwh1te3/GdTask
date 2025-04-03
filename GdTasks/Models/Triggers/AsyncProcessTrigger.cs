namespace GdTasks.Models.Triggers;

public sealed partial class AsyncProcessTrigger : AsyncTriggerBase<AsyncUnit>
{
	/// <inheritdoc/>
	public override void _Process(double delta) => RaiseEvent(AsyncUnit.Default);

	public IAsyncProcessHandler GetProcessAsyncHandler()
		=> new AsyncTriggerHandler<AsyncUnit>(this, false);

	public IAsyncProcessHandler GetProcessAsyncHandler(CancellationToken cancellationToken)
		=> new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);

	public GdTask ProcessAsync()
		=> ((IAsyncProcessHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).ProcessAsync();

	public GdTask ProcessAsync(CancellationToken cancellationToken)
		=> ((IAsyncProcessHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).ProcessAsync();
}