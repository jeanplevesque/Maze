namespace Amazing;

public interface ITile
{
	TilePaths Paths { get; set; }
}

public static class TileExtensions
{
	/*private static char[] _charMap = new char[]
    {
        '0','╨','╡','╝','╥','║','╗','╣','╞','╚','═','╩','╔','╠','╦','╬'
    };*/
	/*private static char[] _charMap = new char[]
    {
        '0',' ',' ','╝',' ','║','╗','╣',' ','╚','═','╩','╔','╠','╦','╬'
    };*/
	private static char[] _charMap = new char[]
	{
		'0',' ',' ','┘',' ','│','┐','┤',' ','└','─','┴','┌','├','┬','┼'
	};


	public static char ToChar(this ITile tile)
	{
		return _charMap[(byte)tile.Paths];
	}

	public static bool HasOnePath(this ITile tile)
	{
		return tile.Paths == TilePaths.Left || tile.Paths == TilePaths.Right || tile.Paths == TilePaths.Forward || tile.Paths == TilePaths.Backward;
	}
}
