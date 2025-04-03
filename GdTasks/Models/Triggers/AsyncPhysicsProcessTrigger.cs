namespace GdTasks.Models.Triggers;

public sealed partial class AsyncPhysicsProcessTrigger : AsyncTriggerBase<AsyncUnit>
{
	/// <inheritdoc/>
	public override void _PhysicsProcess(double delta) => RaiseEvent(AsyncUnit.Default);

	public IAsyncPhysicsProcessHandler GetPhysicsProcessAsyncHandler()
		=> new AsyncTriggerHandler<AsyncUnit>(this, false);

	public IAsyncPhysicsProcessHandler GetPhysicsProcessAsyncHandler(CancellationToken cancellationToken)
		=> new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);

	public GdTask PhysicsProcessAsync()
		=> ((IAsyncPhysicsProcessHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).PhysicsProcessAsync();

	public GdTask PhysicsProcessAsync(CancellationToken cancellationToken)
		=> ((IAsyncPhysicsProcessHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).PhysicsProcessAsync();
}