namespace GdTasks.Models;

public partial class CancellationTokenEqualityComparer : IEqualityComparer<CancellationToken>
{
	public static readonly IEqualityComparer<CancellationToken> Default = new CancellationTokenEqualityComparer();

	public bool Equals(CancellationToken x, CancellationToken y) => x.Equals(y);

	public int GetHashCode(CancellationToken obj) => obj.GetHashCode();
}