namespace GdTasks;

internal static class GdTaskCompletionSourceCoreShared // separated out of generic to avoid unnecessary duplication
{
	internal static readonly Action<object> SSentinel = CompletionSentinel;

	private static void CompletionSentinel(object _) // named method to aid debugging
		=> throw new InvalidOperationException("The sentinel delegate should never be invoked.");
}