namespace Amazing;

public struct Index
{
	public int X;
	public int Y;

	public Index(int x, int y)
	{
		this.X = x;
		this.Y = y;
	}

	public TilePaths GetDirectionTo(Index destination)
	{
		var delta = destination - this;
		if (delta == UnitX)
			return TilePaths.Right;
		if (delta == -UnitX)
			return TilePaths.Left;
		if (delta == UnitY)
			return TilePaths.Forward;
		if (delta == -UnitY)
			return TilePaths.Backward;
		return TilePaths.None;
	}

	public static Index UnitX { get { return _unitX; } }

	public static Index UnitY { get { return _unitY; } }

	private static readonly Index _unitX = new Index(1, 0);
	private static readonly Index _unitY = new Index(0, 1);

	public static Index operator +(Index left, Index right)
	{
		return new Index(left.X + right.X, left.Y + right.Y);
	}

	public static Index operator -(Index left, Index right)
	{
		return new Index(left.X - right.X, left.Y - right.Y);
	}

	public static Index operator *(Index left, Index right)
	{
		return new Index(left.X * right.X, left.Y * right.Y);
	}

	public static Index operator /(Index left, Index right)
	{
		return new Index(left.X / right.X, left.Y / right.Y);
	}

	public static Index operator +(Index left, int right)
	{
		return new Index(left.X + right, left.Y + right);
	}
	public static Index operator -(Index left, int right)
	{
		return new Index(left.X - right, left.Y - right);
	}
	public static Index operator *(Index left, int right)
	{
		return new Index(left.X * right, left.Y * right);
	}
	public static Index operator /(Index left, int right)
	{
		return new Index(left.X / right, left.Y / right);
	}

	public static Index operator -(Index left)
	{
		return left * -1;
	}

	public static bool operator ==(Index left, Index right)
	{
		return left.X == right.X && left.Y == right.Y;
	}
	public static bool operator !=(Index left, Index right)
	{
		return left.X != right.X || left.Y != right.Y;
	}

	public override bool Equals(object obj)
	{
		if (obj is Index)
			return (Index)obj == this;
		return base.Equals(obj);
	}
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
	public override string ToString()
	{
		return string.Format("{{{0},{1}}}", X, Y);
	}
}

public static class IndexExtensions
{
	public static T Get<T>(this T[,] array, Index index)
	{
		return array[index.X, index.Y];
	}
	public static void Set<T>(this T[,] array, Index index, T value)
	{
		array[index.X, index.Y] = value;
	}
}
