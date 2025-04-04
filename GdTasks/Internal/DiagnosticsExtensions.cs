﻿using Godot;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;

namespace GdTasks.Internal;

internal static class DiagnosticsExtensions
{
	private static readonly Dictionary<Type, string> BuiltInTypeNames = new()
	{
		{ typeof(void), "void" },
		{ typeof(bool), "bool" },
		{ typeof(byte), "byte" },
		{ typeof(char), "char" },
		{ typeof(decimal), "decimal" },
		{ typeof(double), "double" },
		{ typeof(float), "float" },
		{ typeof(int), "int" },
		{ typeof(long), "long" },
		{ typeof(object), "object" },
		{ typeof(sbyte), "sbyte" },
		{ typeof(short), "short" },
		{ typeof(string), "string" },
		{ typeof(uint), "uint" },
		{ typeof(ulong), "ulong" },
		{ typeof(ushort), "ushort" },
		{ typeof(Task), "Task" },
		{ typeof(GdTask), "GdTask" },
		{ typeof(GdTaskVoid), "GdTaskVoid" }
	};

	private static readonly Regex TypeBeautifyRegex = new("`.+$", RegexOptions.Compiled);
	private static bool _displayFilenames = true;
	[RequiresUnreferencedCode("Calls System.Diagnostics.StackFrame.GetMethod()")]
	public static string CleanupAsyncStackTrace(this StackTrace? stackTrace)
	{
		if (stackTrace == null)
			return string.Empty;

		var sb = new StringBuilder();
		for (var i = 0; i < stackTrace.FrameCount; i++)
		{
			var sf = stackTrace.GetFrame(i);

			var mb = sf?.GetMethod();

			if (mb != null && IgnoreLine(mb))
				continue;

			if (mb != null && IsAsync(mb))
			{
				sb.Append("async ");
				TryResolveStateMachineMethod(ref mb, out var decType);
			}

			// return type
			if (mb is MethodInfo mi)
			{
				sb.Append(BeautifyType(mi.ReturnType, false));
				sb.Append(" ");
			}

			// method name
			sb.Append(BeautifyType(mb?.DeclaringType, false));
			if (!mb.IsConstructor)
			{
				sb.Append(".");
			}

			sb.Append(mb.Name);

			if (mb.IsGenericMethod)
			{
				sb.Append("<");

				foreach (var item in mb.GetGenericArguments())
				{
					sb.Append(BeautifyType(item, true));
				}

				sb.Append(">");
			}

			// parameter
			sb.Append("(");
			sb.Append(string.Join(", ", mb.GetParameters().Select(p =>
				$"{BeautifyType(p.ParameterType, true)} {p.Name}")));
			sb.Append(")");

			// file name
			if (_displayFilenames && sf != null && (sf.GetILOffset() != -1))
			{
				string? fileName = null;

				try
				{
					fileName = sf.GetFileName();
				}
				catch (NotSupportedException)
				{
					_displayFilenames = false;
				}
				catch (SecurityException)
				{
					_displayFilenames = false;
				}

				if (fileName != null)
				{
					sb.Append(' ');
					sb.AppendFormat(CultureInfo.InvariantCulture, "(at {0})", AppendHyperLink(fileName, sf.GetFileLineNumber().ToString()));
				}
			}

			sb.AppendLine();
		}
		return sb.ToString();
	}

	private static string AppendHyperLink(string path, string line)
	{
		var fi = new FileInfo(path);

		if (fi.Directory == null)
			return fi.Name;

		var fname = fi.FullName.Replace(Path.DirectorySeparatorChar, '/').Replace(ProjectSettings.GlobalizePath("res://"), "");
		var withAssetsPath = $"Assets/{fname}";

		return $"<a href=\"{withAssetsPath}\" line=\"{line}\">{withAssetsPath}:{line}</a>";
	}

	private static string BeautifyType(Type t, bool shortName)
	{
		if (BuiltInTypeNames.TryGetValue(t, out var builtin))
			return builtin;

		if (t.IsGenericParameter) 
			return t.Name;
		
		if (t.IsArray) 
			return $"{BeautifyType(t.GetElementType(), shortName)}[]"; // TODO: Check if this is correct
		
		if (t.FullName?.StartsWith("System.ValueTuple") ?? false)
			return $"({string.Join(", ", t.GetGenericArguments().Select(x => BeautifyType(x, true)))})";
		
		if (!t.IsGenericType) return shortName 
			? t.Name 
			: t.FullName?.Replace("GDTask.Triggers.", "").Replace("GDTask.Internal.", "").Replace("GDTask.", "") ?? t.Name;

		var innerFormat = string.Join(", ", t.GetGenericArguments().Select(x => BeautifyType(x, true)));
		var genericType = t.GetGenericTypeDefinition().FullName;
		
		if (genericType == "System.Threading.Tasks.Task`1") 
			genericType = "Task";

		return $"{TypeBeautifyRegex.Replace(genericType, "").Replace("GDTask.Triggers.", "").Replace("GDTask.Internal.", "").Replace("GDTask.", "")}<{innerFormat}>";
	}

	private static bool IgnoreLine(MethodBase methodInfo)
	{
		// TODO: Find a better way to ignore lines

		var declareType = methodInfo.DeclaringType.FullName;

		if (declareType == "System.Threading.ExecutionContext")
			return true;

		if (declareType.StartsWith("System.Runtime.CompilerServices"))
			return true;

		if (declareType.StartsWith("GdTask.CompilerServices"))
			return true;

		if (declareType == "System.Threading.Tasks.AwaitTaskContinuation")
			return true;

		if (declareType.StartsWith("System.Threading.Tasks.Task"))
			return true;

		if (declareType.StartsWith("GdTask.GdTaskCompletionSourceCore"))
			return true;

		if (declareType.StartsWith("GdTask.AwaiterActions"))
			return true;

		return false;
	}

	private static bool IsAsync(MethodBase methodInfo)
	{
		var declareType = methodInfo.DeclaringType;
		return typeof(IAsyncStateMachine).IsAssignableFrom(declareType);
	}

	// code from Ben.Demystifier/EnhancedStackTrace.Frame.cs
	[RequiresUnreferencedCode("Calls System.Type.GetMethods()")]
	private static bool TryResolveStateMachineMethod(ref MethodBase method, out Type? declaringType)
	{
		declaringType = method.DeclaringType;

		var parentType = declaringType?.DeclaringType;


		var methods = parentType?.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
		
		if (methods == null)
			return false;
		
		foreach (var candidateMethod in methods)
		{
			var attributes = candidateMethod.GetCustomAttributes<StateMachineAttribute>(false);
			
			if (attributes == null)
				continue;
			
			foreach (var asma in attributes)
			{
				if (asma.StateMachineType != declaringType)
					continue;

				method = candidateMethod;
				declaringType = candidateMethod.DeclaringType;
				// Mark the iterator as changed; so it gets the + annotation of the original method
				// async statemachines resolve directly to their builder methods so aren't marked as changed
				return asma is IteratorStateMachineAttribute;
			}
		}

		return false;
	}
}