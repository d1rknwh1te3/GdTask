# GdTasks

## Disclaimer

> [!Note] 
> 
> This is a fork of [Fractural's original GDTask](https://github.com/Fractural/GDTask), created because I wasn't comfortable keeping a third-party project together all the time, so I wanted to wrap it in a NuGet package, but unfortunately I made a lot of weird changes

## Changes

1. Changed namespaces:
	- Fractural.GDTask => GdTasks
2. Changed file Structure: 
	- Splitting multi-class files into single-class files
	- Organizing files into folders according to their logical meaning: structs to structs folder, 
3. Changed the style and project
	- Replaced ctors with primary ctors
	- Replaced the expressions ```throw Exception("name")``` to ```ThrowIfNull(nameof(name))```
	- Renamed properties and fields according to code-style: private => from *foo* to *_foo* or public => from *foo* to *Foo*
	- Organized fields, properties, constructors, methods into their proper order: nested classes, constants and static fields, readonly, static сtor, public ctors, etc...
	- Added omitted access modifiers private and so on
4. Added a test project to the repository

## Description

Adds async/await features in Godot for easier async coding.
Based on code from [Cysharp's UniTask library for Unity](https://github.com/Cysharp/UniTask).

## Installation

You need to install the NuGet package [GdTasks](https://www.nuget.org/packages/GdTasks) in your project, then you need to create a script with any name, for example, [AutoLoadGdTasks](https://github.com/d1rknwh1te3/GdTasks/blob/main/GdTasks.Tests/Scripts/AutoLoadGdTasks.cs) (you can take it from the test project by the way) and add the following contents to it, and then in Project => Project Settings (idk how it is in English version) => Global => and add this script there.

```CSharp
﻿using GdTasks.AutoLoader;
using Godot;

namespace GDTask.Scripts;

public partial class AutoLoadGdTasks : Node
{
	public static GdTaskPlayerLoopAutoload Autoload { get; set; }

	public override void _Ready()
	{
		Autoload = new GdTaskPlayerLoopAutoload();

		base._Ready();
	}
}
```

## Examples

```CSharp
using GdTasks;

public Test : Node 
{
	[Signal]
	public delegate void MySignalHandler(int number, bool boolean);
	
	public override _Ready() 
	{
		// Running a task from a non-async method.
		Run().Forget();
	}

	public async GdTaskVoid Run() 
	{
		await GdTask.DelayFrame(100);

		// Waiting some amount of time
		// Note that these delays are paused when the game is paused
		await GdTask.Delay(TimeSpan.FromSeconds(10));
		await GdTask.Delay(TimeSpan.FromSeconds(10), PlayerLoopTiming.Process);
		await GdTask.Delay(TimeSpan.FromSeconds(10), PlayerLoopTiming.PhysicsProcess);
		// Waiting some amount of milliseconds
		await GdTask.Delay(1000);
		// Waiting some amount of milliseconds, regardless of whether the game is paused
		await GdTask.Delay(TimeSpan.FromSeconds(10), PlayerLoopTiming.PauseProcess);
		await GdTask.Delay(TimeSpan.FromSeconds(10), PlayerLoopTiming.PausePhysicsProcess);

		// Awaiting for a signal
		WaitAndEmitMySignal(TimeSpan.FromSeconds(2)).Forget();
		var signalResults = await GdTask.ToSignal(this, nameof(MySignal));
		// signalResults = [10, true]

		// Cancellable awaiting a signal
		var cts = new CancellationTokenSource();
		WaitAndEmitMySignal(TimeSpan.FromSeconds(2)).Forget();
		WaitAndCancelToken(TimeSpan.FromSeconds(1), cts).Forget();
		try 
		{
			var signalResults = await GdTask.ToSignal(this, nameof(MySignal), cts.Token);
		}
		catch (OperationCanceledException _)
		{
			GD.Print("Awaiting MySignal cancelled!");
		}

		// Waiting a single frame
		await GdTask.Yield();
		await GdTask.NextFrame();
		await GdTask.WaitForEndOfFrame();

		// Waiting for specific lifetime call
		await GdTask.WaitForPhysicsProcess();

		// Cancellation of a GdTask
		var cts = new CancellationTokenSource();
		CancellableReallyLongTask(cts.Token).Forget();
		await GdTask.Delay(TimeSpan.FromSeconds(3));
		cts.Cancel();

		// Returning a value from a GdTask
		string result = await RunWithResult();
		return result + " with additional text";
	}

	public async GdTask<string> RunWithResult()
	{
		await GdTask.Delay(TimeSpan.FromSeconds(3));
		return "A result string";
	}

	public async GdTaskVoid ReallyLongTask(CancellationToken cancellationToken)
	{
		GD.Print("Starting long task.");
		await GdTask.Delay(TimeSpan.FromSeconds(1000000), cancellationToken: cancellationToken);
		GD.Print("Finished long task.");
	}
	
	public async GdTaskVoid WaitAndEmitMySignal(TimeSpan delay)
	{
		await GdTask.Delay(delay);
		EmitSignal(nameof(MySignal), 10, true);
	}

	public async GdTaskVoid WaitAndCancelToken(TimeSpan delay, CancellationTokenSource cts)
	{
		await GdTask.Delay(delay);
		cts.Cancel();
	}
}
```
