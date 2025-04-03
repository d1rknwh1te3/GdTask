using GdTasks.Extensions;
using Godot;

namespace GdTasks.Models.Triggers;

public abstract partial class AsyncTriggerBase<T> : Node
{
	protected internal bool CalledDestroy;
	protected internal bool CalledEnterTree;
	private TriggerEvent<T> _triggerEvent;

	/// <inheritdoc/>
	public override void _EnterTree() => CalledEnterTree = true;

	/// <inheritdoc/>
	public override void _Notification(int what)
	{
		if (what == NotificationPredelete)
			OnDestroy();
	}

	internal void AddHandler(ITriggerHandler<T> handler) => _triggerEvent.Add(handler);

	internal void RemoveHandler(ITriggerHandler<T> handler) => _triggerEvent.Remove(handler);

	protected void RaiseEvent(T value) => _triggerEvent.SetResult(value);

	private void OnDestroy()
	{
		if (CalledDestroy) return;
		CalledDestroy = true;

		_triggerEvent.SetCompleted();
	}
}

public sealed partial class AsyncTriggerHandler<T> : IGdTaskSource<T>, ITriggerHandler<T>, IDisposable
{
	private static Action<object> _cancellationCallback = CancellationCallback;

	private readonly AsyncTriggerBase<T> _trigger;

	private bool _callOnce;
	private CancellationToken _cancellationToken;
	private GdTaskCompletionSourceCore<T> _core;
	private bool _isDisposed;
	private CancellationTokenRegistration _registration;
	internal AsyncTriggerHandler(AsyncTriggerBase<T> trigger, bool callOnce)
	{
		if (_cancellationToken.IsCancellationRequested)
		{
			_isDisposed = true;
			return;
		}

		_trigger = trigger;
		_cancellationToken = CancellationToken.None;
		_registration = default;
		_callOnce = callOnce;

		trigger.AddHandler(this);

		TaskTracker.TrackActiveTask(this, 3);
	}

	internal AsyncTriggerHandler(AsyncTriggerBase<T> trigger, CancellationToken cancellationToken, bool callOnce)
	{
		if (cancellationToken.IsCancellationRequested)
		{
			_isDisposed = true;
			return;
		}

		_trigger = trigger;
		_cancellationToken = cancellationToken;
		_callOnce = callOnce;

		trigger.AddHandler(this);

		if (cancellationToken.CanBeCanceled) 
			_registration = cancellationToken.RegisterWithoutCaptureExecutionContext(_cancellationCallback, this);

		TaskTracker.TrackActiveTask(this, 3);
	}

	ITriggerHandler<T> ITriggerHandler<T>.Next { get; set; }
	ITriggerHandler<T> ITriggerHandler<T>.Prev { get; set; }
	internal CancellationToken CancellationToken => _cancellationToken;
	
	public void Dispose()
	{
		if (_isDisposed)
			return;

		_isDisposed = true;
		TaskTracker.RemoveTracking(this);
		_registration.Dispose();
		_trigger.RemoveHandler(this);
	}

	T IGdTaskSource<T>.GetResult(short token)
	{
		try
		{
			return _core.GetResult(token);
		}
		finally
		{
			if (_callOnce)
			{
				Dispose();
			}
		}
	}

	void IGdTaskSource.GetResult(short token) => ((IGdTaskSource<T>)this).GetResult(token);

	GdTaskStatus IGdTaskSource.GetStatus(short token) => _core.GetStatus(token);

	void ITriggerHandler<T>.OnCanceled(CancellationToken cancellationToken) => _core.TrySetCanceled(cancellationToken);

	void ITriggerHandler<T>.OnCompleted() => _core.TrySetCanceled(CancellationToken.None);

	void IGdTaskSource.OnCompleted(Action<object> continuation, object state, short token) => _core.OnCompleted(continuation, state, token);

	void ITriggerHandler<T>.OnError(Exception ex) => _core.TrySetException(ex);

	void ITriggerHandler<T>.OnNext(T value) => _core.TrySetResult(value);

	GdTaskStatus IGdTaskSource.UnsafeGetStatus() => _core.UnsafeGetStatus();

	private static void CancellationCallback(object state)
	{
		var self = (AsyncTriggerHandler<T>)state;
		self.Dispose();

		self._core.TrySetCanceled(self._cancellationToken);
	}
}