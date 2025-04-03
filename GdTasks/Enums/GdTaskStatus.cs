namespace GdTasks.Enums;

/// <summary>
/// The status of a Godot Task.
/// </summary>
public enum GdTaskStatus
{
	/// <summary>
	/// The operation has not yet completed.
	/// </summary>
	Pending = 0,

	/// <summary>
	/// The operation completed successfully.
	/// </summary>
	Succeeded = 1,

	/// <summary>
	/// The operation completed with an error.
	/// </summary>
	Faulted = 2,

	/// <summary>
	/// The operation completed due to cancellation.
	/// </summary>
	Canceled = 3
}