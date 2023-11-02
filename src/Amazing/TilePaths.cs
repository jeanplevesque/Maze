using System;

namespace Amazing;

[Flags]
public enum TilePaths : byte
{
	None = 0,
	Forward = 0x01,
	Left = 0x02,
	Backward = 0x04,
	Right = 0x08,
	All = 0x0F
}
