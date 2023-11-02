using System;
using System.Collections.Generic;
using System.Linq;

namespace Amazing;

public interface IMaze<TTile> where TTile : ITile
{
	Index Dimensions { get; }
	Index StartPosition { get; }
	Index EndPosition { get; }
	TTile this[int i, int j] { get; set; }
	TTile this[Index index] { get; set; }
	bool IsValidPosition(int i, int j);
}

public static class MazeExtensions
{
	private static Random _random = new Random(0);

	public static bool IsValidPosition<TTile>(this IMaze<TTile> maze, Index index)
		where TTile : ITile
	{
		return maze.IsValidPosition(index.X, index.Y);
	}

	public static IEnumerable<Index> NeighboursOf<TTile>(this IMaze<TTile> maze, Index index)
		where TTile : ITile
	{
		Index next = index + Index.UnitY;
		if (maze.IsValidPosition(next))
			yield return next;
		next = index - Index.UnitX;
		if (maze.IsValidPosition(next))
			yield return next;
		next = index - Index.UnitY;
		if (maze.IsValidPosition(next))
			yield return next;
		next = index + Index.UnitX;
		if (maze.IsValidPosition(next))
			yield return next;
	}

	public static IEnumerable<Index> ConnectedNeighboursOf<TTile>(this IMaze<TTile> maze, Index index)
		where TTile : ITile
	{
		Index next = index + Index.UnitY;
		var paths = maze[index].Paths;
		if (maze.IsValidPosition(next) && paths.HasFlag(TilePaths.Forward))
			yield return next;
		next = index - Index.UnitX;
		if (maze.IsValidPosition(next) && paths.HasFlag(TilePaths.Left))
			yield return next;
		next = index - Index.UnitY;
		if (maze.IsValidPosition(next) && paths.HasFlag(TilePaths.Backward))
			yield return next;
		next = index + Index.UnitX;
		if (maze.IsValidPosition(next) && paths.HasFlag(TilePaths.Right))
			yield return next;
	}

	public static void Generate<TTile>(this IMaze<TTile> maze, float loopProbability)
		where TTile : ITile
	{
		Stack<Index> stack = new Stack<Index>();
		stack.Push(maze.EndPosition);

		while (stack.Count != 0)
		{
			Index current = stack.Pop();

			var possibilities = maze.NeighboursOf(current).Where(i => maze[i].Paths == TilePaths.None).ToList();
			if (_random.Next() <= int.MaxValue * loopProbability)
			{
				possibilities = possibilities.Concat(maze.NeighboursOf(current).Where(i => maze[i].HasOnePath())).ToList();
			}
			int count = possibilities.Count;
			if (count == 0)
				continue;

			int choice = _random.Next(count);
			var next = possibilities[choice];

			var currentTile = maze[current];
			var nextTile = maze[next];
			currentTile.Paths |= current.GetDirectionTo(next);
			nextTile.Paths |= next.GetDirectionTo(current);
			maze[current] = currentTile;
			maze[next] = nextTile;

			for (int i = 0; i < count; ++i)
			{
				if (i != choice && maze[possibilities[i]].Paths == TilePaths.None)
				{
					stack.Push(current);
				}
			}
			stack.Push(possibilities[choice]);

			//Console.SetCursorPosition(next.X, maze.Dimensions.Y - next.Y);
			//Console.Write(maze[next.X, next.Y].ToChar());
			//Console.SetCursorPosition(current.X, maze.Dimensions.Y - current.Y);
			//Console.Write(maze[current.X, current.Y].ToChar());
			//System.Threading.Thread.Sleep(5);
		}
	}
	public static List<Index> Solve<TTile>(this IMaze<TTile> maze, Index startPosition, Index endPosition)
		where TTile : ITile
	{
		int[,] weights = new int[maze.Dimensions.X, maze.Dimensions.Y];
		for (int i = 0; i < maze.Dimensions.X; i++)
		{
			for (int j = 0; j < maze.Dimensions.Y; j++)
			{
				weights[i, j] = int.MaxValue;
			}
		}
		List<Index> solution = new List<Index>();
		List<Index> frontier = new List<Index>();
		frontier.Add(startPosition);
		int weight = 0;
		weights.Set(startPosition, weight);
		while (weights.Get(endPosition) == int.MaxValue)
		{
			++weight;
			var nexts = frontier.SelectMany(f => maze.ConnectedNeighboursOf(f).Where(i => weights.Get(i) == int.MaxValue)).ToList();
			frontier.Clear();
			foreach (var item in nexts)
			{
				weights.Set(item, weight);
				frontier.Add(item);
				//Console.BackgroundColor = ConsoleColor.DarkGreen;
				//Console.SetCursorPosition(3*item.X + maze.Dimensions.X, maze.Dimensions.Y - item.Y);
				//Console.Write(weights[item.X, item.Y]);
				//Console.BackgroundColor = ConsoleColor.Black;
				//System.Threading.Thread.Sleep(5);
			}
		}

		Index pos = endPosition;
		solution.Add(pos);
		while (pos != startPosition)
		{
			pos = maze.ConnectedNeighboursOf(pos).First(i => weights.Get(i) == weights.Get(pos) - 1);
			solution.Add(pos);
			//Console.BackgroundColor = ConsoleColor.DarkRed;
			//Console.SetCursorPosition(3 * pos.X + maze.Dimensions.X, maze.Dimensions.Y - pos.Y);
			//Console.Write(weights[pos.X, pos.Y]);
			//Console.BackgroundColor = ConsoleColor.Black;
		}
		solution.Reverse();
		return solution;
	}
}
