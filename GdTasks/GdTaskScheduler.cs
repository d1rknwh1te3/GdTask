using Godot;

namespace GdTasks;
// GDTask has no scheduler like TaskScheduler.
// Only handle unobserved exception.

public static class GdTaskScheduler
{
	/// <summary>
	/// Propagate OperationCanceledException to UnobservedTaskException when true. Default is false.
	/// </summary>
	public static bool propagateOperationCanceledException = false;

	public static event Action<Exception>? UnobservedTaskException;
	internal static void PublishUnobservedTaskException(Exception? ex)
	{
		if (ex == null)
			return;

		if (!propagateOperationCanceledException && ex is OperationCanceledException)
			return;

		if (UnobservedTaskException != null)
		{
			UnobservedTaskException.Invoke(ex);
		}
		else
		{
			GD.PrintErr($"UnobservedTaskException: {ex}");
		}
	}
}