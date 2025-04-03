using System;
using System.Threading;

namespace Fractural.Tasks;

public partial struct GdTask
{
	#region OBSOLETE_RUN

	[Obsolete("GDTask.Run is similar as Task.Run, it uses ThreadPool. For equivalent behaviour, use GDTask.RunOnThreadPool instead. If you don't want to use ThreadPool, you can use GDTask.Void(async void) or GDTask.Create(async GDTask) too.")]
	public static GdTask Run(Action action, bool configureAwait = true, CancellationToken cancellationToken = default)
	{
		return RunOnThreadPool(action, configureAwait, cancellationToken);
	}

	[Obsolete("GDTask.Run is similar as Task.Run, it uses ThreadPool. For equivalent behaviour, use GDTask.RunOnThreadPool instead. If you don't want to use ThreadPool, you can use GDTask.Void(async void) or GDTask.Create(async GDTask) too.")]
	public static GdTask Run(Action<object> action, object state, bool configureAwait = true, CancellationToken cancellationToken = default)
	{
		return RunOnThreadPool(action, state, configureAwait, cancellationToken);
	}

	[Obsolete("GDTask.Run is similar as Task.Run, it uses ThreadPool. For equivalent behaviour, use GDTask.RunOnThreadPool instead. If you don't want to use ThreadPool, you can use GDTask.Void(async void) or GDTask.Create(async GDTask) too.")]
	public static GdTask Run(Func<GdTask> action, bool configureAwait = true, CancellationToken cancellationToken = default)
	{
		return RunOnThreadPool(action, configureAwait, cancellationToken);
	}

	[Obsolete("GDTask.Run is similar as Task.Run, it uses ThreadPool. For equivalent behaviour, use GDTask.RunOnThreadPool instead. If you don't want to use ThreadPool, you can use GDTask.Void(async void) or GDTask.Create(async GDTask) too.")]
	public static GdTask Run(Func<object, GdTask> action, object state, bool configureAwait = true, CancellationToken cancellationToken = default)
	{
		return RunOnThreadPool(action, state, configureAwait, cancellationToken);
	}

	[Obsolete("GDTask.Run is similar as Task.Run, it uses ThreadPool. For equivalent behaviour, use GDTask.RunOnThreadPool instead. If you don't want to use ThreadPool, you can use GDTask.Void(async void) or GDTask.Create(async GDTask) too.")]
	public static GdTask<T> Run<T>(Func<T> func, bool configureAwait = true, CancellationToken cancellationToken = default)
	{
		return RunOnThreadPool(func, configureAwait, cancellationToken);
	}

	[Obsolete("GDTask.Run is similar as Task.Run, it uses ThreadPool. For equivalent behaviour, use GDTask.RunOnThreadPool instead. If you don't want to use ThreadPool, you can use GDTask.Void(async void) or GDTask.Create(async GDTask) too.")]
	public static GdTask<T> Run<T>(Func<GdTask<T>> func, bool configureAwait = true, CancellationToken cancellationToken = default)
	{
		return RunOnThreadPool(func, configureAwait, cancellationToken);
	}

	[Obsolete("GDTask.Run is similar as Task.Run, it uses ThreadPool. For equivalent behaviour, use GDTask.RunOnThreadPool instead. If you don't want to use ThreadPool, you can use GDTask.Void(async void) or GDTask.Create(async GDTask) too.")]
	public static GdTask<T> Run<T>(Func<object, T> func, object state, bool configureAwait = true, CancellationToken cancellationToken = default)
	{
		return RunOnThreadPool(func, state, configureAwait, cancellationToken);
	}

	[Obsolete("GDTask.Run is similar as Task.Run, it uses ThreadPool. For equivalent behaviour, use GDTask.RunOnThreadPool instead. If you don't want to use ThreadPool, you can use GDTask.Void(async void) or GDTask.Create(async GDTask) too.")]
	public static GdTask<T> Run<T>(Func<object, GdTask<T>> func, object state, bool configureAwait = true, CancellationToken cancellationToken = default)
	{
		return RunOnThreadPool(func, state, configureAwait, cancellationToken);
	}

	#endregion

	/// <summary>Run action on the threadPool and return to main thread if configureAwait = true.</summary>
	public static async GdTask RunOnThreadPool(Action action, bool configureAwait = true, CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();

		await SwitchToThreadPool();

		cancellationToken.ThrowIfCancellationRequested();

		if (configureAwait)
		{
			try
			{
				action();
			}
			finally
			{
				await Yield();
			}
		}
		else
		{
			action();
		}

		cancellationToken.ThrowIfCancellationRequested();
	}

	/// <summary>Run action on the threadPool and return to main thread if configureAwait = true.</summary>
	public static async GdTask RunOnThreadPool(Action<object> action, object state, bool configureAwait = true, CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();

		await SwitchToThreadPool();

		cancellationToken.ThrowIfCancellationRequested();

		if (configureAwait)
		{
			try
			{
				action(state);
			}
			finally
			{
				await Yield();
			}
		}
		else
		{
			action(state);
		}

		cancellationToken.ThrowIfCancellationRequested();
	}

	/// <summary>Run action on the threadPool and return to main thread if configureAwait = true.</summary>
	public static async GdTask RunOnThreadPool(Func<GdTask> action, bool configureAwait = true, CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();

		await SwitchToThreadPool();

		cancellationToken.ThrowIfCancellationRequested();

		if (configureAwait)
		{
			try
			{
				await action();
			}
			finally
			{
				await Yield();
			}
		}
		else
		{
			await action();
		}

		cancellationToken.ThrowIfCancellationRequested();
	}

	/// <summary>Run action on the threadPool and return to main thread if configureAwait = true.</summary>
	public static async GdTask RunOnThreadPool(Func<object, GdTask> action, object state, bool configureAwait = true, CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();

		await SwitchToThreadPool();

		cancellationToken.ThrowIfCancellationRequested();

		if (configureAwait)
		{
			try
			{
				await action(state);
			}
			finally
			{
				await Yield();
			}
		}
		else
		{
			await action(state);
		}

		cancellationToken.ThrowIfCancellationRequested();
	}

	/// <summary>Run action on the threadPool and return to main thread if configureAwait = true.</summary>
	public static async GdTask<T> RunOnThreadPool<T>(Func<T> func, bool configureAwait = true, CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();

		await SwitchToThreadPool();

		cancellationToken.ThrowIfCancellationRequested();

		if (configureAwait)
		{
			try
			{
				return func();
			}
			finally
			{
				await Yield();
				cancellationToken.ThrowIfCancellationRequested();
			}
		}
		else
		{
			return func();
		}
	}

	/// <summary>Run action on the threadPool and return to main thread if configureAwait = true.</summary>
	public static async GdTask<T> RunOnThreadPool<T>(Func<GdTask<T>> func, bool configureAwait = true, CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();

		await SwitchToThreadPool();

		cancellationToken.ThrowIfCancellationRequested();

		if (configureAwait)
		{
			try
			{
				return await func();
			}
			finally
			{
				cancellationToken.ThrowIfCancellationRequested();
				await Yield();
				cancellationToken.ThrowIfCancellationRequested();
			}
		}
		else
		{
			var result = await func();
			cancellationToken.ThrowIfCancellationRequested();
			return result;
		}
	}

	/// <summary>Run action on the threadPool and return to main thread if configureAwait = true.</summary>
	public static async GdTask<T> RunOnThreadPool<T>(Func<object, T> func, object state, bool configureAwait = true, CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();

		await SwitchToThreadPool();

		cancellationToken.ThrowIfCancellationRequested();

		if (configureAwait)
		{
			try
			{
				return func(state);
			}
			finally
			{
				await Yield();
				cancellationToken.ThrowIfCancellationRequested();
			}
		}
		else
		{
			return func(state);
		}
	}

	/// <summary>Run action on the threadPool and return to main thread if configureAwait = true.</summary>
	public static async GdTask<T> RunOnThreadPool<T>(Func<object, GdTask<T>> func, object state, bool configureAwait = true, CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();

		await SwitchToThreadPool();

		cancellationToken.ThrowIfCancellationRequested();

		if (configureAwait)
		{
			try
			{
				return await func(state);
			}
			finally
			{
				cancellationToken.ThrowIfCancellationRequested();
				await Yield();
				cancellationToken.ThrowIfCancellationRequested();
			}
		}
		else
		{
			var result = await func(state);
			cancellationToken.ThrowIfCancellationRequested();
			return result;
		}
	}
}