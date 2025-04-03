namespace GdTasks.Interfaces.States;

internal interface IStateMachineRunner
{
	Action MoveNext { get; }
	Action ReturnAction { get; }

	void Return();
}