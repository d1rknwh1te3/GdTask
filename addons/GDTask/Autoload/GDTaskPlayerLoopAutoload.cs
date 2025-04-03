using Fractural.Tasks.Internal;
using Godot;
using System;

namespace Fractural.Tasks;

public static class GdTaskLoopRunners
{
	public struct GdTaskLoopRunnerProcess { };
	public struct GdTaskLoopRunnerPhysicsProcess { };
}

public enum PlayerLoopTiming
{
	Process = 0,
	PhysicsProcess = 1,
	PauseProcess = 2,
	PausePhysicsProcess = 3,
}

public interface IPlayerLoopItem
{
	bool MoveNext();
}

/// <summary>
/// Singleton that forwards Godot calls and values to GDTasks.
/// </summary>
public partial class GdTaskPlayerLoopAutoload : Node
{
	public static int MainThreadId => Global._mainThreadId;
	public static bool IsMainThread => System.Threading.Thread.CurrentThread.ManagedThreadId == Global._mainThreadId;
	public static void AddAction(PlayerLoopTiming timing, IPlayerLoopItem action) => Global.LocalAddAction(timing, action);
	public static void ThrowInvalidLoopTiming(PlayerLoopTiming playerLoopTiming) => throw new InvalidOperationException("Target playerLoopTiming is not injected. Please check PlayerLoopHelper.Initialize. PlayerLoopTiming:" + playerLoopTiming);
	public static void AddContinuation(PlayerLoopTiming timing, Action continuation) => Global.LocalAddContinuation(timing, continuation);

	public void LocalAddAction(PlayerLoopTiming timing, IPlayerLoopItem action)
	{
		var runner = _runners[(int)timing];
		if (runner == null)
		{
			ThrowInvalidLoopTiming(timing);
		}
		runner.AddAction(action);
	}

	// NOTE: Continuation means a asynchronous task invoked by another task after the other task finishes.
	public void LocalAddContinuation(PlayerLoopTiming timing, Action continuation)
	{
		var q = _yielders[(int)timing];
		if (q == null)
		{
			ThrowInvalidLoopTiming(timing);
		}
		q.Enqueue(continuation);
	}

	public static GdTaskPlayerLoopAutoload Global
	{
		get
		{
			if (_sGlobal != null) return _sGlobal;

			var newInstance = new GdTaskPlayerLoopAutoload();
			newInstance.Initialize();
			var currentScene = ((SceneTree)Engine.GetMainLoop()).CurrentScene;
			currentScene.AddChild(newInstance);
			currentScene.MoveChild(newInstance, 0);
			newInstance.Name = "GDTaskPlayerLoopAutoload";
			_sGlobal = newInstance;

			return _sGlobal;
		}
	}
	public double DeltaTime => GetProcessDeltaTime();
	public double PhysicsDeltaTime => GetPhysicsProcessDeltaTime();

	private static GdTaskPlayerLoopAutoload _sGlobal;
	private int _mainThreadId;
	private ContinuationQueue[] _yielders;
	private PlayerLoopRunner[] _runners;
	private ProcessListener _processListener;

	public override void _EnterTree()
	{
		if (_sGlobal == null)
		{
			Initialize();
			_sGlobal = this;
			return;
		}
		QueueFree();
	}

	private void Initialize()
	{
		ProcessMode = ProcessModeEnum.Pausable;
		_mainThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
		_yielders = new[] {
			new ContinuationQueue(PlayerLoopTiming.Process),
			new ContinuationQueue(PlayerLoopTiming.PhysicsProcess),
			new ContinuationQueue(PlayerLoopTiming.PauseProcess),
			new ContinuationQueue(PlayerLoopTiming.PausePhysicsProcess),
		};
		_runners = new[] {
			new PlayerLoopRunner(PlayerLoopTiming.Process),
			new PlayerLoopRunner(PlayerLoopTiming.PhysicsProcess),
			new PlayerLoopRunner(PlayerLoopTiming.PauseProcess),
			new PlayerLoopRunner(PlayerLoopTiming.PausePhysicsProcess),
		};
		_processListener = new ProcessListener();
		AddChild(_processListener);
		_processListener.ProcessMode = ProcessModeEnum.Always;
		_processListener.OnProcess += PauseProcess;
		_processListener.OnPhysicsProcess += PausePhysicsProcess;
	}

	public override void _Notification(int what)
	{
		if (what == NotificationPredelete)
		{
			if (Global == this)
				_sGlobal = null;
			if (_yielders != null)
			{
				foreach (var yielder in _yielders)
					yielder.Clear();
				foreach (var runner in _runners)
					runner.Clear();
			}
		}
	}

	public override void _Process(double delta)
	{
		_yielders[(int)PlayerLoopTiming.Process].Run();
		_runners[(int)PlayerLoopTiming.Process].Run();
	}

	public override void _PhysicsProcess(double delta)
	{
		_yielders[(int)PlayerLoopTiming.PhysicsProcess].Run();
		_runners[(int)PlayerLoopTiming.PhysicsProcess].Run();
	}

	private void PauseProcess(double delta)
	{
		_yielders[(int)PlayerLoopTiming.PauseProcess].Run();
		_runners[(int)PlayerLoopTiming.PauseProcess].Run();
	}

	private void PausePhysicsProcess(double delta)
	{
		_yielders[(int)PlayerLoopTiming.PausePhysicsProcess].Run();
		_runners[(int)PlayerLoopTiming.PausePhysicsProcess].Run();
	}
}