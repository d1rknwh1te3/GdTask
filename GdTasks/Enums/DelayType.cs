namespace GdTasks.Enums;

/// <summary>
/// Delay types.
/// </summary>
public enum DelayType
{
	/// <summary>
	/// Use Time.deltaTime.
	/// </summary>
	DeltaTime,

	/// <summary>
	/// Use Stopwatch.GetTimestamp().
	/// </summary>
	RealTime
}