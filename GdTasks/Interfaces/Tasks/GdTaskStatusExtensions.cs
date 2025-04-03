using System.Runtime.CompilerServices;

namespace GdTasks.Interfaces.Tasks;

public static class GdTaskStatusExtensions
{
	/// <summary>status != Pending.</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsCompleted(this GdTaskStatus status)
	{
		return status != GdTaskStatus.Pending;
	}

	/// <summary>status == Succeeded.</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsCompletedSuccessfully(this GdTaskStatus status)
	{
		return status == GdTaskStatus.Succeeded;
	}

	/// <summary>status == Canceled.</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsCanceled(this GdTaskStatus status)
	{
		return status == GdTaskStatus.Canceled;
	}

	/// <summary>status == Faulted.</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsFaulted(this GdTaskStatus status)
	{
		return status == GdTaskStatus.Faulted;
	}
}