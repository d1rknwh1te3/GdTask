using System.Runtime.ExceptionServices;

namespace GdTasks.Models;

internal class ExceptionHolder(ExceptionDispatchInfo exception)
{
	private bool _calledGet;

	~ExceptionHolder()
	{
		if (!_calledGet)
			GdTaskScheduler.PublishUnobservedTaskException(exception.SourceException);
	}

	public ExceptionDispatchInfo GetException()
	{
		if (_calledGet)
			return exception;

		_calledGet = true;
		GC.SuppressFinalize(this);
		return exception;
	}
}