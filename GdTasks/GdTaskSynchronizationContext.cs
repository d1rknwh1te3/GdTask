using Godot;
using System.Runtime.InteropServices;

namespace GdTasks;

public partial class GdTaskSynchronizationContext : SynchronizationContext
{
	private const int InitialSize = 16;
	private const int MaxArrayLength = 0X7FEFFFFF;
	private static Callback[] _actionList = new Callback[InitialSize];
	private static int _actionListCount;
	private static bool _dequing;
	private static SpinLock _gate = new(false);
	private static int _opCount;
	private static Callback[] _waitingList = new Callback[InitialSize];
	private static int _waitingListCount;
	public override SynchronizationContext CreateCopy() => this;

	public override void OperationCompleted() => Interlocked.Decrement(ref _opCount);

	public override void OperationStarted() => Interlocked.Increment(ref _opCount);

	public override void Post(SendOrPostCallback d, object? state)
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

					var newArray = new Callback[newLength];
					Array.Copy(_waitingList, newArray, _waitingListCount);
					_waitingList = newArray;
				}
				_waitingList[_waitingListCount] = new Callback(d, state);
				_waitingListCount++;
			}
			else
			{
				// Ensure Capacity
				if (_actionList.Length == _actionListCount)
				{
					var newLength = _actionListCount * 2;
					if ((uint)newLength > MaxArrayLength) newLength = MaxArrayLength;

					var newArray = new Callback[newLength];
					Array.Copy(_actionList, newArray, _actionListCount);
					_actionList = newArray;
				}
				_actionList[_actionListCount] = new Callback(d, state);
				_actionListCount++;
			}
		}
		finally
		{
			if (lockTaken) _gate.Exit(false);
		}
	}

	public override void Send(SendOrPostCallback d, object? state) => d(state);
	// delegate entrypoint.
	internal static void Run()
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
			_actionList[i] = default;
			action.Invoke();
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

	[StructLayout(LayoutKind.Auto)]
	private readonly struct Callback(SendOrPostCallback callback, object state)
	{
		public void Invoke()
		{
			try
			{
				callback(state);
			}
			catch (Exception ex)
			{
				GD.PrintErr(ex);
			}
		}
	}
}