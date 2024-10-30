using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class GameDisplay : IDisplay
    {
        public GameDisplay() { }

        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void DrawGrid(Grid playerGrid, Grid opponentGrid, bool hideShips)
        {
            Console.WriteLine("  0 1 2 3 4 5 6 7 8 9   ".PadRight(25) + "  0 1 2 3 4 5 6 7 8 9");
            for (int row = 0; row < Grid.GridSize; row++)
            {
                // Display player grid row
                Console.Write($"{row} ");
                for (int col = 0; col < Grid.GridSize; col++)
                {
                    char playerSymbol = GetCellSymbol(playerGrid.Grids[row, col], false);
                    Console.Write($"{playerSymbol} ");
                }

                // Spacer between grids
                Console.Write("   ");

                // Display opponent grid row
                Console.Write($"{row} ");
                for (int col = 0; col < Grid.GridSize; col++)
                {
                    char opponentSymbol = GetCellSymbol(opponentGrid.Grids[row, col], hideShips);
                    Console.Write($"{opponentSymbol} ");
                }

                Console.WriteLine();
            }
        }

        private char GetCellSymbol(Cell cell, bool hideShips)
        {
            if (cell.IsHit && cell.HasShip()) return 'X'; // Hit ship
            if (cell.IsHit && cell.IsEmpty()) return 'M'; // Missed shot
            if (!cell.IsHit && cell.HasShip() && !hideShips) return 'O'; // Repaired ship part, visible to owner
            if (!cell.IsHit && cell.IsEmpty()) return '~'; // Water for empty, unhit cells
            return '~'; // Default to water for any remaining cases
        }
    }
}
