using Galaxon.Core.Exceptions;
using Galaxon.Core.Numbers.Extensions;

namespace Galaxon.Numerics.Integers;

public static class Grids
{
    public static uint?[,] ConstructSpiral(int size, bool clockwise, EDirection start)
    {
        // Make sure it's odd.
        if (size % 2 != 1)
        {
            throw new ArgumentInvalidException(nameof(size), "Must be odd.");
        }

        // Create the data structure.
        var spiral = new uint?[size, size];

        bool isVacant(int x, int y) =>
            x >= 0 && x < size && y >= 0 && y < size && spiral[x, y] == null;

        // Start in the centre.
        int x = size / 2;
        int y = x;
        EDirection direction =
            clockwise ? Direction.TurnAntiClockwise(start) : Direction.TurnClockwise(start);

        // Loop until the spiral is built.
        for (uint n = 1; n <= size * size; n++)
        {
            // Set the value of the current position.
            spiral[x, y] = n;

            // Try to turn.
            switch (direction)
            {
                case EDirection.Up:
                    if (clockwise && isVacant(x + 1, y))
                    {
                        // Go right.
                        x++;
                        direction = EDirection.Right;
                    }
                    else if (!clockwise && isVacant(x - 1, y))
                    {
                        // Go left.
                        x--;
                        direction = EDirection.Left;
                    }
                    else if (isVacant(x, y - 1))
                    {
                        // Go up again.
                        y--;
                    }
                    break;

                case EDirection.Right:
                    if (clockwise && isVacant(x, y + 1))
                    {
                        // Go down.
                        y++;
                        direction = EDirection.Down;
                    }
                    else if (!clockwise && isVacant(x, y - 1))
                    {
                        // Go up.
                        y--;
                        direction = EDirection.Up;
                    }
                    else if (isVacant(x + 1, y))
                    {
                        // Go right again.
                        x++;
                    }
                    break;

                case EDirection.Down:
                    if (clockwise && isVacant(x - 1, y))
                    {
                        // Go left.
                        x--;
                        direction = EDirection.Left;
                    }
                    else if (!clockwise && isVacant(x + 1, y))
                    {
                        // Go right.
                        x++;
                        direction = EDirection.Right;
                    }
                    else if (isVacant(x, y + 1))
                    {
                        // Go down again.
                        y++;
                    }
                    break;

                case EDirection.Left:
                    if (clockwise && isVacant(x, y - 1))
                    {
                        // Go up.
                        y--;
                        direction = EDirection.Up;
                    }
                    else if (!clockwise && isVacant(x, y + 1))
                    {
                        // Go down.
                        y++;
                        direction = EDirection.Down;
                    }
                    else if (isVacant(x - 1, y))
                    {
                        // Go left again.
                        x--;
                    }
                    break;
            } // switch
        } // for n

        return spiral;
    }

    public static void PrintGrid(uint?[,] grid)
    {
        int size = XInt.Sqrt(grid.Length);
        for (var y = 0; y < size; y++)
        {
            Console.Write("[ ");
            for (var x = 0; x < size; x++)
            {
                Console.Write((grid[x, y]?.ToString() ?? "").PadLeft(8));
            }
            Console.WriteLine(" ]");
        }
    }
}
