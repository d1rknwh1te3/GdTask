namespace GdTasks.Interfaces;

/// <summary>
/// Acts as a linked list for TaskSources.
/// </summary>
/// <typeparam name="T">Same type as the class that implements this</typeparam>
public interface ITaskPoolNode<T>
{
	// Because interfaces cannot have fields, we store a reference to the field as a getter.
	// This is so we can directly set and get the field rather than using a property getter/setter, which might have more overhead.
	//
	// Disgusting, but efficient.
	ref T NextNode { get; }
}