﻿using Godot;
using System.Threading;

namespace Fractural.Tasks.Triggers;

public static partial class AsyncTriggerExtensions
{
	public static AsyncDestroyTrigger GetAsyncDestroyTrigger(this Node node)
	{
		return node.GetOrAddImmediateChild<AsyncDestroyTrigger>();
	}
}

public sealed partial class AsyncDestroyTrigger : Node
{
	private bool _enterTreeCalled = false;
	private bool _called = false;
	private CancellationTokenSource _cancellationTokenSource;

	public CancellationToken CancellationToken
	{
		get
		{
			if (_cancellationTokenSource == null)
			{
				_cancellationTokenSource = new CancellationTokenSource();
			}

			return _cancellationTokenSource.Token;
		}
	}

	public override void _EnterTree()
	{
		_enterTreeCalled = true;
	}

	public override void _Notification(int what)
	{
		if (what == NotificationPredelete)
			OnDestroy();
	}

	private void OnDestroy()
	{
		_called = true;

		_cancellationTokenSource?.Cancel();
		_cancellationTokenSource?.Dispose();
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
}