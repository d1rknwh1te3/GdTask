using GdTasks.Extensions;
using Godot;

namespace GdTasks.Models.Triggers;

public static partial class AsyncTriggerExtensions
{
	public static AsyncDestroyTrigger GetAsyncDestroyTrigger(this Node node)
		=> node.GetOrAddImmediateChild<AsyncDestroyTrigger>();
}

public sealed partial class AsyncDestroyTrigger : Node
{
	private bool _called;
	private CancellationTokenSource? _cancellationTokenSource;
	private bool _enterTreeCalled;
	public CancellationToken CancellationToken
	{
		get
		{
			_cancellationTokenSource ??= new CancellationTokenSource();

			return _cancellationTokenSource.Token;
		}
	}

	/// <inheritdoc/>
	public override void _EnterTree() => _enterTreeCalled = true;

	/// <inheritdoc/>
	public override void _Notification(int what)
	{
		if (what == NotificationPredelete)
			OnDestroy();
	}

	public GdTask OnDestroyAsync()
	{
		if (_called) return GdTask.CompletedTask;

		var tcs = new GdTaskCompletionSource();

		// OnDestroy = Called Cancel.
		CancellationToken.RegisterWithoutCaptureExecutionContext(state =>
		{
			var tcs2 = (GdTaskCompletionSource)state;
			tcs2.TrySetResult();
		}, tcs);

		return tcs.Task;
	}

	private void OnDestroy()
	{
		_called = true;

		_cancellationTokenSource?.Cancel();
		_cancellationTokenSource?.Dispose();
	}
}