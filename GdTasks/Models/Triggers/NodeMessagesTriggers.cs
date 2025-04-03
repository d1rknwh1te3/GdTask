using Godot;

namespace GdTasks.Models.Triggers;

#region PhysicsProcess

public partial class AsyncTriggerHandler<T> : IAsyncPhysicsProcessHandler
{
	GdTask IAsyncPhysicsProcessHandler.PhysicsProcessAsync()
	{
		_core.Reset();
		return new GdTask((IGdTaskSource)(object)this, _core.Version);
	}
}

public static partial class AsyncTriggerExtensions
{
	public static AsyncPhysicsProcessTrigger GetAsyncPhysicsProcessTrigger(this Node node)
	{
		return node.GetOrAddImmediateChild<AsyncPhysicsProcessTrigger>();
	}
}

#endregion

#region Process

public partial class AsyncTriggerHandler<T> : IAsyncProcessHandler
{
	GdTask IAsyncProcessHandler.ProcessAsync()
	{
		_core.Reset();
		return new GdTask((IGdTaskSource)(object)this, _core.Version);
	}
}

public static partial class AsyncTriggerExtensions
{
	public static AsyncProcessTrigger GetAsyncProcessTrigger(this Node node)
	{
		return node.GetOrAddImmediateChild<AsyncProcessTrigger>();
	}
}

#endregion