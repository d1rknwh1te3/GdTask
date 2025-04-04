using GdTasks.AutoLoader;
using Godot;

namespace GDTask.Scripts;

public partial class AutoLoadGdTasks : Node
{
	public static GdTaskPlayerLoopAutoload Autoload { get; set; }

	public override void _Ready()
	{
		Autoload = new GdTaskPlayerLoopAutoload();

		base._Ready();
	}
}