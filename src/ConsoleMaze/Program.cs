using Amazing;
using System.Text;
using Index = Amazing.Index;

namespace ConsoleMaze;

class Program
{
	static void Main(string[] args)
	{
		int size;
		if (args.Length > 0 && int.TryParse(args[0], out size))
		{
			Size = Math.Max(size, 6);
		}
		while (true)
		{
			Game();
			//Bonus();
			continue;
		}
	}

	static void Bonus()
	{
		Index pos = new Index();
		Index speed = new Index(1, 1);
		StringBuilder fill = new StringBuilder(Console.WindowWidth);
		Console.Clear();
		Console.SetCursorPosition(0, 0);
		for (int i = 0; i < Console.WindowWidth; i++)
		{
			fill.Append('#');
		}
		for (int i = 0; i < Console.WindowHeight; i++)
		{
			Console.Write(fill.ToString());
		}
		Console.SetCursorPosition(0, 0);
		Console.BackgroundColor = ConsoleColor.Red;
		Console.ForegroundColor = ConsoleColor.White;
		Console.WriteLine("""
			/---\
			|   |
			\___/
			""");
		Console.BackgroundColor = ConsoleColor.Black;
		Console.ForegroundColor = ConsoleColor.Gray;
		while (!Console.KeyAvailable)
		{
			var p = pos + speed;
			if (p.Y + 3 >= Console.WindowHeight || p.Y <= 0) { speed.Y *= -1; }
			if (p.X + 5 >= Console.WindowWidth || p.X <= 0) { speed.X *= -1; }
			Console.MoveBufferArea(pos.X, pos.Y, 5, 3, p.X, p.Y, 'm', ConsoleColor.Blue, ConsoleColor.Green);

			pos = p;
			Thread.Sleep(1000 / 30);
		}
		Console.ReadKey(false);
	}

	static int Size = 16;
	static void Game()
	{
	start:
		Console.Clear();
		BasicMaze maze = new BasicMaze(Size, Size * 3 / 5);
		maze.Generate(0.15f);

		Console.WindowWidth = Math.Max(Console.WindowWidth, maze.Dimensions.X + 2);
		Console.WindowHeight = Math.Max(Console.WindowHeight, maze.Dimensions.Y + 2);

		// Draw the maze
		for (int j = 0; j < maze.Dimensions.Y; j++)
		{
			Console.SetCursorPosition(0, maze.Dimensions.Y - j);
			for (int i = 0; i < maze.Dimensions.X; i++)
			{
				var index = new Index(i, j);
				if (index == maze.EndPosition)
				{
					Console.BackgroundColor = ConsoleColor.DarkRed;
				}
				if (index == maze.StartPosition)
				{
					Console.BackgroundColor = ConsoleColor.Blue;
				}
				Console.Write(maze[i, j].ToChar());
				Console.BackgroundColor = ConsoleColor.Black;
			}
		}

		// Game loop

		Index pos = maze.StartPosition;
		while (pos != maze.EndPosition)
		{
			Console.SetCursorPosition(maze.Dimensions.X + 1, maze.Dimensions.Y + 1);
			var key = Console.ReadKey().Key;
			Index nextPos = new Index();
			switch (key)
			{
				case ConsoleKey.LeftArrow:
					nextPos = pos - Index.UnitX;
					break;
				case ConsoleKey.RightArrow:
					nextPos = pos + Index.UnitX;
					break;
				case ConsoleKey.UpArrow:
					nextPos = pos + Index.UnitY;
					break;
				case ConsoleKey.DownArrow:
					nextPos = pos - Index.UnitY;
					break;
				case ConsoleKey.Enter:
					++Size;
					var solution = maze.Solve(pos, maze.EndPosition);
					Console.BackgroundColor = ConsoleColor.DarkGreen;
					foreach (var step in solution)
					{
						Console.SetCursorPosition(step.X, maze.Dimensions.Y - step.Y);
						Console.Write(maze[step.X, step.Y].ToChar());
						Thread.Sleep(20);
					}
					Console.BackgroundColor = ConsoleColor.Black;
					Thread.Sleep(1000);
					goto start;
			}
			if (maze.ConnectedNeighboursOf(pos).Contains(nextPos))
			{
				Console.BackgroundColor = ConsoleColor.Black;
				Console.SetCursorPosition(pos.X, maze.Dimensions.Y - pos.Y);
				Console.Write(maze[pos.X, pos.Y].ToChar());
				pos = nextPos;

				Console.BackgroundColor = ConsoleColor.Blue;
				Console.SetCursorPosition(pos.X, maze.Dimensions.Y - pos.Y);
				Console.Write(maze[pos.X, pos.Y].ToChar());
				Console.BackgroundColor = ConsoleColor.Black;
			}

		}

		++Size;
	}

}
