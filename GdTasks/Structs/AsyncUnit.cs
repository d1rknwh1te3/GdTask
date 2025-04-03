#pragma warning disable CS1591 // Missing XML comment for publicly visible type or

namespace GdTasks.Structs;

public readonly struct AsyncUnit : IEquatable<AsyncUnit>
{
	public static readonly AsyncUnit Default = new();

	public bool Equals(AsyncUnit other) => true;

	public override int GetHashCode() => 0;

	public override string ToString() => "()";
}