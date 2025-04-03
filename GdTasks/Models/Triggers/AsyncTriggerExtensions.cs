using Godot;

namespace GdTasks.Models.Triggers;

public static partial class AsyncTriggerExtensions
{
	public static AsyncReadyTrigger GetAsyncReadyTrigger(this Node node)
		=> node.GetOrAddImmediateChild<AsyncReadyTrigger>();
}

public static partial class AsyncTriggerExtensions
{
	public static T AddImmediateChild<T>(this Node node) where T : Node, new()
	{
		var child = new T();
		node.AddChild(child);
		return child;
	}

	public static GdTask EnterTreeAsync(this Node node) => node.GetAsyncEnterTreeTrigger().EnterTreeAsync();

	// Special for single operation.
	public static T? GetImmediateChild<T>(this Node node, bool includeRoot = true)
	{
		if (node == null) 
			throw new ArgumentNullException(nameof(node));
			
		if (includeRoot && node is T castedRoot)
			return castedRoot;
			
		foreach (var child in node.GetChildren())
			if (child is T castedChild)
				return castedChild;

		return default(T);
	}
	public static T GetOrAddImmediateChild<T>(this Node node) where T : Node, new()
	{
		var child = GetImmediateChild<T>(node) ?? AddImmediateChild<T>(node);
			
		return child;
	}

	/// <summary>
	/// This function is called when the Node will be destroyed.
	/// </summary>
	public static GdTask OnDestroyAsync(this Node node) => node.GetAsyncDestroyTrigger().OnDestroyAsync();

	public static GdTask ReadyAsync(this Node node) => node.GetAsyncReadyTrigger().ReadyAsync();
}