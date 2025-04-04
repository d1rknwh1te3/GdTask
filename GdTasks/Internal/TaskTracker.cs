﻿using System.Diagnostics;
using System.Text;

namespace GdTasks.Internal;

// public for add user custom.
public static class TaskTracker
{
	// TODO: Work on task tracker after getting tasks functioning
#if DEBUG

	private static int _trackingId;

	public const string EnableAutoReloadKey = "GDTaskTrackerWindow_EnableAutoReloadKey";
	public const string EnableTrackingKey = "GDTaskTrackerWindow_EnableTrackingKey";
	public const string EnableStackTraceKey = "GDTaskTrackerWindow_EnableStackTraceKey";

	public static class EditorEnableState
	{
		public static bool EnableAutoReload { get; set; } //UnityEditor.EditorPrefs.SetBool(EnableAutoReloadKey, value);
		public static bool EnableTracking { get; set; }  //UnityEditor.EditorPrefs.SetBool(EnableTrackingKey, value);
		public static bool EnableStackTrace { get; set; } //UnityEditor.EditorPrefs.SetBool(EnableStackTraceKey, value);
	}

#endif

	private static List<KeyValuePair<IGdTaskSource, (string formattedType, int trackingId, DateTime addTime, string stackTrace)>> _listPool = new();

	private static readonly WeakDictionary<IGdTaskSource, (string formattedType, int trackingId, DateTime addTime, string stackTrace)> Tracking = new();

	[Conditional("DEBUG")]
	public static void TrackActiveTask(IGdTaskSource task, int skipFrame)
	{
#if DEBUG
		_dirty = true;
		if (!EditorEnableState.EnableTracking) return;
		var stackTrace = EditorEnableState.EnableStackTrace ? new StackTrace(skipFrame, true).CleanupAsyncStackTrace() : "";

		string typeName;
		if (EditorEnableState.EnableStackTrace)
		{
			var sb = new StringBuilder();
			TypeBeautify(task.GetType(), sb);
			typeName = sb.ToString();
		}
		else
		{
			typeName = task.GetType().Name;
		}
		Tracking.TryAdd(task, (typeName, Interlocked.Increment(ref _trackingId), DateTime.UtcNow, stackTrace));
#endif
	}

	[Conditional("DEBUG")]
	public static void RemoveTracking(IGdTaskSource task)
	{
#if DEBUG
		_dirty = true;
		if (!EditorEnableState.EnableTracking) return;
		var success = Tracking.TryRemove(task);
#endif
	}

	private static bool _dirty;

	public static bool CheckAndResetDirty()
	{
		var current = _dirty;
		_dirty = false;
		return current;
	}

	/// <summary>(trackingId, awaiterType, awaiterStatus, createdTime, stackTrace)</summary>
	public static void ForEachActiveTask(Action<int, string, GdTaskStatus, DateTime, string> action)
	{
		lock (_listPool)
		{
			var count = Tracking.ToList(ref _listPool, clear: false);
			try
			{
				for (var i = 0; i < count; i++)
				{
					action(_listPool[i].Value.trackingId, _listPool[i].Value.formattedType, _listPool[i].Key.UnsafeGetStatus(), _listPool[i].Value.addTime, _listPool[i].Value.stackTrace);
					_listPool[i] = default;
				}
			}
			catch
			{
				_listPool.Clear();
				throw;
			}
		}
	}

	private static void TypeBeautify(Type type, StringBuilder sb)
	{
		if (type.IsNested)
		{
			// TypeBeautify(type.DeclaringType, sb);
			sb.Append(type.DeclaringType.Name.ToString());
			sb.Append(".");
		}

		if (type.IsGenericType)
		{
			var genericsStart = type.Name.IndexOf("`", StringComparison.Ordinal);

			sb.Append(genericsStart != -1 ? type.Name.Substring(0, genericsStart) : type.Name);
			sb.Append("<");

			var first = true;

			foreach (var item in type.GetGenericArguments())
			{
				if (!first)
					sb.Append(", ");

				first = false;
				TypeBeautify(item, sb);
			}
			sb.Append(">");
		}
		else
		{
			sb.Append(type.Name);
		}
	}
}