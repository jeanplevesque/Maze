namespace Amazing;

public class BasicMaze : IMaze<BasicTile>
{
	public Index Dimensions { get; set; }

	public Index StartPosition { get { return new Index(0, 0); } }

	public Index EndPosition { get { return this.Dimensions - 1; } }

	private BasicTile[,] _tiles;

	public BasicMaze(int xTileCount, int yTileCount)
	{
		this.Dimensions = new Index(xTileCount, yTileCount);
		_tiles = new BasicTile[xTileCount, yTileCount];
	}

	public BasicTile this[Index index]
	{
		get { return _tiles[index.X, index.Y]; }
		set { _tiles[index.X, index.Y] = value; }
	}
	public BasicTile this[int i, int j]
	{
		get { return _tiles[i, j]; }
		set { _tiles[i, j] = value; }
	}

	public bool IsValidPosition(int i, int j)
	{
		return i >= 0 && j >= 0 && i < this.Dimensions.X && j < this.Dimensions.Y;
	}
}
