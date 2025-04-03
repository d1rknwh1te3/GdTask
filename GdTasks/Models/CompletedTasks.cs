namespace GdTasks.Models;

internal static class CompletedTasks
{
	public static readonly GdTask<AsyncUnit> AsyncUnit = GdTask.FromResult(Structs.AsyncUnit.Default);
	public static readonly GdTask<bool> False = GdTask.FromResult(false);
	public static readonly GdTask<int> MinusOne = GdTask.FromResult(-1);
	public static readonly GdTask<int> One = GdTask.FromResult(1);
	public static readonly GdTask<bool> True = GdTask.FromResult(true);
	public static readonly GdTask<int> Zero = GdTask.FromResult(0);
}