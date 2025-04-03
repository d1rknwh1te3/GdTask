#pragma warning disable CS1591
#pragma warning disable CS0436

using GdTasks.CompilerServices;
using System.Runtime.CompilerServices;

namespace GdTasks.Structs;

[AsyncMethodBuilder(typeof(AsyncGdTaskVoidMethodBuilder))]
public readonly struct GdTaskVoid
{
	public void Forget()
	{ }
}