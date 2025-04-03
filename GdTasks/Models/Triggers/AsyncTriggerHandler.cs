namespace GdTasks.Models.Triggers;

public partial class AsyncTriggerHandler<T> : IAsyncOneShotTrigger
{
	GdTask IAsyncOneShotTrigger.OneShotAsync()
	{
		_core.Reset();
		return new GdTask(this, _core.Version);
	}
}