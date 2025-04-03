namespace GdTasks.Interfaces.Tasks;

// General architecture:
// Each GDTask holds a IGDTaskSource, which determines how the GDTask will run. This is basically a strategy pattern.
// GDTask is a struct, so will be allocated on stack with no garbage collection. All IGDTaskSources will be pooled using
// TaskPool<T>, so again, no garabage will be generated.
//
// Hence we achieve 0 memory allocation, making our tasks run really fast.

/// <summary>
/// GDTaskSource that has a void return (returns nothing).
/// </summary>
public interface IGdTaskSource
{
	GdTaskStatus GetStatus(short token);

	void OnCompleted(Action<object> continuation, object state, short token);

	void GetResult(short token);

	GdTaskStatus UnsafeGetStatus(); // only for debug use.
}

/// <summary>
/// GDTaskSource that has a return value of <see cref="T"/>
/// </summary>
/// <typeparam name="T">Return value of the task source</typeparam>
public interface IGdTaskSource<out T> : IGdTaskSource
{
	// Hide the original void GetResult method
	new T GetResult(short token);
}

// Extensions are all aggressive inlined so all calls are substituted with raw code for greatest performance.