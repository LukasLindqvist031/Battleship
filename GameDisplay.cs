using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class GameDisplay : IDisplay
    {
        public void DrawGrid(Grid playerGrid, Grid opponentGrid, bool hideShips)
        {
            int gridWidth = 23; 
            int totalWidth = gridWidth * 2 + 5; 
            int gridHeight = Grid.GridSize + 1; 

            int startY = TextPresentation.GetCenterY(gridHeight);
            int startX = TextPresentation.GetCenterX(totalWidth);

            string playerName = playerGrid.Player?.Name ?? "Player";
            string opponentName = opponentGrid.Player?.Name ?? "Opponent";

            TextPresentation.WriteCenteredText($"{playerName}'s Grid".PadRight(25) + $"{opponentName}'s Grid", startY - 2);

            string colNumbers = "  0 1 2 3 4 5 6 7 8 9";
            Console.SetCursorPosition(startX, startY);
            Console.Write(colNumbers.PadRight(25));
            Console.Write(colNumbers);

            for (int row = 0; row < Grid.GridSize; row++)
            {
                Console.SetCursorPosition(startX, startY + row + 1);

                Console.Write($"{row} ");
                for (int col = 0; col < Grid.GridSize; col++)
                {
                    char playerSymbol = GetCellSymbol(playerGrid.Grids[row, col], false);
                    Console.Write($"{playerSymbol} ");
                }

                Console.Write("   ");

                Console.Write($"{row} ");
                for (int col = 0; col < Grid.GridSize; col++)
                {
                    char opponentSymbol = GetCellSymbol(opponentGrid.Grids[row, col], hideShips);
                    Console.Write($"{opponentSymbol} ");
                }
            }
        }

        private char GetCellSymbol(Cell cell, bool hideShips)
        {
            if (cell.IsHit && cell.HasShip()) return 'X';
            if (cell.IsHit && cell.IsEmpty()) return 'M';
            if (!cell.IsHit && cell.HasShip() && !hideShips) return 'O';
            if (!cell.IsHit && cell.IsEmpty()) return '~';
            return '~';
        }
    }
}