using GdTasks.CompilerServices;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;

namespace GdTasks;

/// <summary>
/// Lightweight Godot specific task-like object with a void return value.
/// </summary>
[AsyncMethodBuilder(typeof(AsyncGdTaskMethodBuilder))]
[StructLayout(LayoutKind.Auto)]
[method: DebuggerHidden]
[method: MethodImpl(MethodImplOptions.AggressiveInlining)]
public readonly partial struct GdTask(IGdTaskSource source, short token)
{
	private readonly IGdTaskSource source = source;
	private readonly short token = token;

	public GdTaskStatus Status
	{
		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => source?.GetStatus(token) ?? GdTaskStatus.Succeeded;
	}

	[DebuggerHidden]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Awaiter GetAwaiter() => new(this);

	/// <summary>
	/// returns (bool IsCanceled) instead of throws OperationCanceledException.
	/// </summary>
	public GdTask<bool> SuppressCancellationThrow()
	{
		var status = Status;

		return status switch
		{
			GdTaskStatus.Succeeded => CompletedTasks.False,
			GdTaskStatus.Canceled => CompletedTasks.True,
			_ => new GdTask<bool>(new IsCanceledSource(source), token)
		};
	}

	public override string ToString()
	{
		return source == null
			? "()"
			: $"({source.UnsafeGetStatus()})";
	}

	/// <summary>
	/// Memoizing inner IValueTaskSource. The result GDTask can await multiple.
	/// </summary>
	public GdTask Preserve()
	{
		return source == null
			? this
			: new GdTask(new MemoizeSource(source), token);
	}

	public GdTask<AsyncUnit> AsAsyncUnitGdTask()
	{
		if (source == null)
			return CompletedTasks.AsyncUnit;

		var status = source.GetStatus(token);

		if (status.IsCompletedSuccessfully())
		{
			source.GetResult(token);
			return CompletedTasks.AsyncUnit;
		}

		return source is IGdTaskSource<AsyncUnit> asyncUnitSource
			? new GdTask<AsyncUnit>(asyncUnitSource, token)
			: new GdTask<AsyncUnit>(new AsyncUnitSource(source), token);
	}

	private sealed class AsyncUnitSource(IGdTaskSource source) : IGdTaskSource<AsyncUnit>
	{
		public AsyncUnit GetResult(short token)
		{
			source.GetResult(token);
			return AsyncUnit.Default;
		}

		public GdTaskStatus GetStatus(short token) => source.GetStatus(token);

		public GdTaskStatus UnsafeGetStatus() => source.UnsafeGetStatus();

		public void OnCompleted(Action<object> continuation, object state, short token) => source.OnCompleted(continuation, state, token);

		void IGdTaskSource.GetResult(short token) => GetResult(token);
	}

	private sealed class IsCanceledSource(IGdTaskSource source) : IGdTaskSource<bool>
	{
		public bool GetResult(short token)
		{
			if (source.GetStatus(token) == GdTaskStatus.Canceled)
				return true;

			source.GetResult(token);
			return false;
		}

		void IGdTaskSource.GetResult(short token) => GetResult(token);

		public GdTaskStatus GetStatus(short token) => source.GetStatus(token);

		public GdTaskStatus UnsafeGetStatus() => source.UnsafeGetStatus();

		public void OnCompleted(Action<object> continuation, object state, short token) => source.OnCompleted(continuation, state, token);
	}

	private sealed class MemoizeSource(IGdTaskSource? source) : IGdTaskSource
	{
		private IGdTaskSource? _source = source;
		private ExceptionDispatchInfo? _exception;
		private GdTaskStatus _status;

		public void GetResult(short token)
		{
			if (_source == null)
			{
				_exception?.Throw();
			}
			else
			{
				try
				{
					_source.GetResult(token);
					_status = GdTaskStatus.Succeeded;
				}
				catch (Exception ex)
				{
					_exception = ExceptionDispatchInfo.Capture(ex);
					_status = ex is OperationCanceledException ? GdTaskStatus.Canceled : GdTaskStatus.Faulted;

					throw;
				}
				finally
				{
					_source = null;
				}
			}
		}

		public GdTaskStatus GetStatus(short token) => _source?.GetStatus(token) ?? _status;

		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			if (_source == null)
			{
				continuation(state);
			}
			else
			{
				_source.OnCompleted(continuation, state, token);
			}
		}

		public GdTaskStatus UnsafeGetStatus() => _source?.UnsafeGetStatus() ?? _status;
	}

	[method: DebuggerHidden]
	[method: MethodImpl(MethodImplOptions.AggressiveInlining)]
	public readonly struct Awaiter(in GdTask task) : ICriticalNotifyCompletion
	{
		private readonly GdTask _task = task;

		public bool IsCompleted
		{
			[DebuggerHidden]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _task.Status.IsCompleted();
		}

		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetResult() => _task.source?.GetResult(_task.token);

		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void OnCompleted(Action continuation)
		{
			if (_task.source == null)
			{
				continuation();
			}
			else
			{
				_task.source.OnCompleted(AwaiterActions.InvokeContinuationDelegate, continuation, _task.token);
			}
		}

		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UnsafeOnCompleted(Action continuation)
		{
			if (_task.source == null)
			{
				continuation();
			}
			else
			{
				_task.source.OnCompleted(AwaiterActions.InvokeContinuationDelegate, continuation, _task.token);
			}
		}

		/// <summary>
		/// If register manually continuation, you can use it instead of for compiler OnCompleted methods.
		/// </summary>
		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SourceOnCompleted(Action<object> continuation, object state)
		{
			if (_task.source == null)
			{
				continuation(state);
			}
			else
			{
				_task.source.OnCompleted(continuation, state, _task.token);
			}
		}
	}
}

/// <summary>
/// Lightweight Godot specified task-like object with a return value.
/// </summary>
/// <typeparam name="T">Return value of the task</typeparam>
[AsyncMethodBuilder(typeof(AsyncGdTaskMethodBuilder<>))]
[StructLayout(LayoutKind.Auto)]
public readonly struct GdTask<T>
{
	private readonly IGdTaskSource<T> source;
	private readonly T? result;
	private readonly short token;

	[DebuggerHidden]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public GdTask(T result)
	{
		source = null;
		token = 0;
		this.result = result;
	}

	[DebuggerHidden]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public GdTask(IGdTaskSource<T> source, short token)
	{
		this.source = source;
		this.token = token;
		result = default;
	}

	public GdTaskStatus Status
	{
		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => source?.GetStatus(token) ?? GdTaskStatus.Succeeded;
	}

	[DebuggerHidden]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Awaiter GetAwaiter() => new(this);

	/// <summary>
	/// Memoizing inner IValueTaskSource. The result GDTask can await multiple.
	/// </summary>
	public GdTask<T> Preserve()
	{
		return source == null
			? this
			: new GdTask<T>(new MemoizeSource(source), token);
	}

	public GdTask AsGdTask()
	{
		if (source == null)
			return GdTask.CompletedTask;

		var status = source.GetStatus(token);

		if (!status.IsCompletedSuccessfully())
			return new GdTask(source, token);

		source.GetResult(token);
		return GdTask.CompletedTask;

		// Converting GDTask<T> -> GDTask is zero overhead.
	}

	public static implicit operator GdTask(GdTask<T> self)
	{
		return self.AsGdTask();
	}

	/// <summary>
	/// returns (bool IsCanceled, T Result) instead of throws OperationCanceledException.
	/// </summary>
	public GdTask<(bool IsCanceled, T Result)> SuppressCancellationThrow()
	{
		return source == null
			? new GdTask<(bool IsCanceled, T Result)>((false, result))
			: new GdTask<(bool, T)>(new IsCanceledSource(source), token);
	}

	/// <inheritdoc/>
	public override string ToString()
	{
		return source == null
			? result?.ToString()
			: $"({source.UnsafeGetStatus()})";
	}

	[method: DebuggerHidden]
	[method: MethodImpl(MethodImplOptions.AggressiveInlining)]
	private sealed class IsCanceledSource(IGdTaskSource<T> source) : IGdTaskSource<(bool, T)>
	{
		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public (bool, T) GetResult(short token)
		{
			if (source.GetStatus(token) == GdTaskStatus.Canceled)
				return (true, default);

			var result = source.GetResult(token);
			return (false, result);
		}

		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void IGdTaskSource.GetResult(short token) => GetResult(token);

		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public GdTaskStatus GetStatus(short token) => source.GetStatus(token);

		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public GdTaskStatus UnsafeGetStatus() => source.UnsafeGetStatus();

		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			source.OnCompleted(continuation, state, token);
		}
	}

	private sealed class MemoizeSource(IGdTaskSource<T> source) : IGdTaskSource<T>
	{
		private IGdTaskSource<T> _source = source;
		private T _result;
		private ExceptionDispatchInfo _exception;
		private GdTaskStatus _status;

		public T GetResult(short token)
		{
			if (_source == null)
			{
				if (_exception != null)
				{
					_exception.Throw();
				}
				return _result;
			}
			else
			{
				try
				{
					_result = _source.GetResult(token);
					_status = GdTaskStatus.Succeeded;
					return _result;
				}
				catch (Exception ex)
				{
					_exception = ExceptionDispatchInfo.Capture(ex);
					_status = ex is OperationCanceledException ? GdTaskStatus.Canceled : GdTaskStatus.Faulted;
					throw;
				}
				finally
				{
					_source = null;
				}
			}
		}

		void IGdTaskSource.GetResult(short token)
		{
			GetResult(token);
		}

		public GdTaskStatus GetStatus(short token)
		{
			return _source == null ? _status : _source.GetStatus(token);
		}

		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			if (_source == null)
			{
				continuation(state);
			}
			else
			{
				_source.OnCompleted(continuation, state, token);
			}
		}

		public GdTaskStatus UnsafeGetStatus()
		{
			return _source == null ? _status : _source.UnsafeGetStatus();
		}
	}

	[method: DebuggerHidden]
	[method: MethodImpl(MethodImplOptions.AggressiveInlining)]
	public readonly struct Awaiter(in GdTask<T> task) : ICriticalNotifyCompletion
	{
		private readonly GdTask<T> _task = task;

		public bool IsCompleted
		{
			[DebuggerHidden]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _task.Status.IsCompleted();
		}

		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T GetResult()
		{
			var s = _task.source;
			return s == null ? _task.result : s.GetResult(_task.token);
		}

		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void OnCompleted(Action continuation)
		{
			var s = _task.source;
			if (s == null)
			{
				continuation();
			}
			else
			{
				s.OnCompleted(AwaiterActions.InvokeContinuationDelegate, continuation, _task.token);
			}
		}

		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UnsafeOnCompleted(Action continuation)
		{
			var s = _task.source;
			if (s == null)
			{
				continuation();
			}
			else
			{
				s.OnCompleted(AwaiterActions.InvokeContinuationDelegate, continuation, _task.token);
			}
		}

		/// <summary>
		/// If register manually continuation, you can use it instead of for compiler OnCompleted methods.
		/// </summary>
		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SourceOnCompleted(Action<object> continuation, object state)
		{
			var s = _task.source;
			if (s == null)
			{
				continuation(state);
			}
			else
			{
				s.OnCompleted(continuation, state, _task.token);
			}
		}
	}
}