namespace Amazing;

public struct BasicTile : ITile
{
	public TilePaths Paths { get; set; }

	public override string ToString()
	{
		return this.ToChar().ToString();
	}
}
