using Godot;

namespace GdTasks.AutoLoader;

public partial class ProcessListener : Node
{
	public event Action<double>? OnProcess;

	public event Action<double>? OnPhysicsProcess;

	/// <inheritdoc/>
	public override void _Process(double delta) => OnProcess?.Invoke(delta);

	/// <inheritdoc/>
	public override void _PhysicsProcess(double delta) => OnPhysicsProcess?.Invoke(delta);
}