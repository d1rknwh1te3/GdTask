﻿using Godot;
using System.Diagnostics;

namespace GdTasks.Internal;

internal sealed class ContinuationQueue(PlayerLoopTiming timing)
{
	private const int MaxArrayLength = 0X7FEFFFFF;
	private const int InitialSize = 16;

	private Action[] _actionList = new Action[InitialSize];
	private Action[] _waitingList = new Action[InitialSize];
	private SpinLock _gate = new(false);

	private bool _dequing;

	private int _actionListCount;
	private int _waitingListCount;

	public void Enqueue(Action continuation)
	{
		var lockTaken = false;

		try
		{
			_gate.Enter(ref lockTaken);

			if (_dequing)
			{
				// Ensure Capacity
				if (_waitingList.Length == _waitingListCount)
				{
					var newLength = _waitingListCount * 2;
					if ((uint)newLength > MaxArrayLength) newLength = MaxArrayLength;

					var newArray = new Action[newLength];
					Array.Copy(_waitingList, newArray, _waitingListCount);
					_waitingList = newArray;
				}
				_waitingList[_waitingListCount] = continuation;
				_waitingListCount++;
			}
			else
			{
				// Ensure Capacity
				if (_actionList.Length == _actionListCount)
				{
					var newLength = _actionListCount * 2;
					if ((uint)newLength > MaxArrayLength) newLength = MaxArrayLength;

					var newArray = new Action[newLength];
					Array.Copy(_actionList, newArray, _actionListCount);
					_actionList = newArray;
				}
				_actionList[_actionListCount] = continuation;
				_actionListCount++;
			}
		}
		finally
		{
			if (lockTaken) _gate.Exit(false);
		}
	}

	public int Clear()
	{
		var rest = _actionListCount + _waitingListCount;

		_actionListCount = 0;
		_actionList = new Action[InitialSize];

		_waitingListCount = 0;
		_waitingList = new Action[InitialSize];

		return rest;
	}

	// delegate entrypoint.
	public void Run()
	{
		// for debugging, create named stacktrace.
#if DEBUG
		switch (timing)
		{
			case PlayerLoopTiming.PhysicsProcess:
				PhysicsProcess();
				break;

			case PlayerLoopTiming.Process:
				Process();
				break;

			case PlayerLoopTiming.PausePhysicsProcess:
				PausePhysicsProcess();
				break;

			case PlayerLoopTiming.PauseProcess:
				PauseProcess();
				break;

			default:
				throw new ArgumentOutOfRangeException(nameof(timing), timing, null);
		}
#else
            RunCore();
#endif
	}

	private void PhysicsProcess() => RunCore();

	private void Process() => RunCore();

	private void PausePhysicsProcess() => RunCore();

	private void PauseProcess() => RunCore();

	[DebuggerHidden]
	private void RunCore()
	{
		{
			var lockTaken = false;
			try
			{
				_gate.Enter(ref lockTaken);
				if (_actionListCount == 0) return;
				_dequing = true;
			}
			finally
			{
				if (lockTaken) _gate.Exit(false);
			}
		}

		for (var i = 0; i < _actionListCount; i++)
		{
			var action = _actionList[i];
			_actionList[i] = null;
			try
			{
				action();
			}
			catch (Exception ex)
			{
				GD.PrintErr(ex);
			}
		}

		{
			var lockTaken = false;
			try
			{
				_gate.Enter(ref lockTaken);
				_dequing = false;

				var swapTempActionList = _actionList;

				_actionListCount = _waitingListCount;
				_actionList = _waitingList;

				_waitingListCount = 0;
				_waitingList = swapTempActionList;
			}
			finally
			{
				if (lockTaken) _gate.Exit(false);
			}
		}
	}
}