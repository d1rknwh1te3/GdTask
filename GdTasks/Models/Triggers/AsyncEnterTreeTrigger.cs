using Godot;

namespace GdTasks.Models.Triggers;

public static partial class AsyncTriggerExtensions
{
	public static AsyncEnterTreeTrigger GetAsyncEnterTreeTrigger(this Node node)
		=> node.GetOrAddImmediateChild<AsyncEnterTreeTrigger>();
}

public sealed partial class AsyncEnterTreeTrigger : AsyncTriggerBase<AsyncUnit>
{
	/// <inheritdoc/>
	public override void _EnterTree()
	{
		base._EnterTree();
		RaiseEvent(AsyncUnit.Default);
	}

	public GdTask EnterTreeAsync()
	{
		return CalledEnterTree
			? GdTask.CompletedTask
			: ((IAsyncOneShotTrigger)new AsyncTriggerHandler<AsyncUnit>(this, true)).OneShotAsync();
	}
}