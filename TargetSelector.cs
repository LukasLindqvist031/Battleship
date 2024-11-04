using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace Battleship
{
    public class TargetSelector
    {
        private readonly Grid _grid;

        public TargetSelector(Grid grid)
        {
            _grid = grid;
        }

        public Cell GetValidTarget(bool forRepair)
        {
            // Starting position for input prompts
            int inputLineY = Console.WindowHeight / 2 + 8;

            // Row input
            int row;
            while (true)
            {
                Console.SetCursorPosition(0, inputLineY);
                Console.Write(new string(' ', Console.WindowWidth)); // Clear line
                Console.SetCursorPosition(0, inputLineY);
                Console.Write($"Row (0-{Grid.GridSize - 1}): ");
                if (int.TryParse(Console.ReadLine(), out row) && row >= 0 && row < Grid.GridSize)
                {
                    break; // Valid row entered
                }

                // Display error for invalid row input
                Console.SetCursorPosition(0, inputLineY + 1);
                Console.Write(new string(' ', Console.WindowWidth)); // Clear line
                Console.SetCursorPosition(0, inputLineY + 1);
                Console.WriteLine($"Invalid row. Enter a number between 0 and {Grid.GridSize - 1}.");
            }

            // Column input
            int col;
            while (true)
            {
                Console.SetCursorPosition(0, inputLineY + 2);
                Console.Write(new string(' ', Console.WindowWidth)); // Clear line
                Console.SetCursorPosition(0, inputLineY + 2);
                Console.Write($"Column (0-{Grid.GridSize - 1}): ");
                if (int.TryParse(Console.ReadLine(), out col) && col >= 0 && col < Grid.GridSize)
                {
                    break; // Valid column entered
                }

                // Display error for invalid column input
                Console.SetCursorPosition(0, inputLineY + 3);
                Console.Write(new string(' ', Console.WindowWidth)); // Clear line
                Console.SetCursorPosition(0, inputLineY + 3);
                Console.WriteLine($"Invalid column. Enter a number between 0 and {Grid.GridSize - 1}.");
            }

            // Validate chosen cell based on the action type
            Cell targetCell = _grid.Grids[row, col];
            if (!forRepair && targetCell.IsHit)
            {
                // For attack, disallow already hit cells
                Console.SetCursorPosition(0, inputLineY + 4);
                Console.Write(new string(' ', Console.WindowWidth)); // Clear line
                Console.SetCursorPosition(0, inputLineY + 4);
                Console.WriteLine("This cell has already been targeted. Please choose another location.");

                // Recursively call the method for a new selection
                return GetValidTarget(forRepair);
            }
            else if (forRepair && (!targetCell.IsHit || targetCell.Ship == null || targetCell.Ship.IsSunk()))
            {
                // For repair, require an already hit, non-sunk cell with a ship
                Console.SetCursorPosition(0, inputLineY + 4);
                Console.Write(new string(' ', Console.WindowWidth)); // Clear line
                Console.SetCursorPosition(0, inputLineY + 4);
                Console.WriteLine("The selected cell is not damaged or has no ship. Please choose a damaged cell.");

                // Recursively call the method for a new selection
                return GetValidTarget(forRepair);
            }

            // Clear any lingering error messages before returning the valid cell
            Console.SetCursorPosition(0, inputLineY + 4);
            Console.Write(new string(' ', Console.WindowWidth));

            return targetCell;
        }
    }
}
