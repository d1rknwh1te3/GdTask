using GdTasks.Models.Triggers;
using Godot;

namespace GdTasks.Extensions;

/// <summary>
/// Extensions for Godot Tasks
/// </summary>
public static class GdTaskCancellationExtensions
{
	/// <summary>
	/// This CancellationToken is canceled when the Node will be destroyed.
	/// </summary>
	public static CancellationToken GetCancellationTokenOnDestroy(this Node node)
		=> node.GetAsyncDestroyTrigger().CancellationToken;
}