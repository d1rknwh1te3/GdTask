using System.Runtime.CompilerServices;

namespace GdTasks.CompilerServices;

internal static class StateMachineUtility
{
	// Get AsyncStateMachine internal state to check IL2CPP bug
	public static int GetState(IAsyncStateMachine stateMachine)
	{
		var info = stateMachine.GetType().GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
			.First(x => x.Name.EndsWith("__state"));
		return (int)info.GetValue(stateMachine);
	}
}