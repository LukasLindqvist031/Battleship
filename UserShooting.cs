using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class UserShooting : IShootingStrategy
    {
        public void Shoot(Player player)
        {
            Grid opponentGrid = player.OpponentGrid;
            Cell targetCell = GetValidTargetFromUser(opponentGrid);

            if (targetCell != null)
            {
                Attack attack = new Attack(opponentGrid);
                attack.Execute(player, targetCell);

                if (targetCell.HasShip())
                {
                    Console.WriteLine("Hit! You've struck an enemy ship!");
                    if (targetCell.Ship.IsSunk())
                    {
                        Console.WriteLine($"You've sunk a ship of length {targetCell.Ship.Length}!");
                    }
                }
                else
                {
                    Console.WriteLine("Miss! You hit the water.");
                }
                System.Threading.Thread.Sleep(1500);
            }
        }

        private Cell GetValidTargetFromUser(Grid opponentGrid)
        {
            // Starting position for input prompts
            int inputLineY = Console.WindowHeight / 2 + 8;

            while (true)
            {
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

                // Validate chosen cell
                Cell targetCell = opponentGrid.Grids[row, col];
                if (targetCell.IsHit)
                {
                    Console.SetCursorPosition(0, inputLineY + 4);
                    Console.Write(new string(' ', Console.WindowWidth)); // Clear line
                    Console.SetCursorPosition(0, inputLineY + 4);
                    Console.WriteLine("This cell has already been targeted. Please choose another location.");
                    continue;
                }

                // Clear any lingering error messages before returning the valid cell
                Console.SetCursorPosition(0, inputLineY + 4);
                Console.Write(new string(' ', Console.WindowWidth));

                return targetCell;
            }
        }
    }
}
