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

                // Display result of the attack
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

                // Small pause to let the player see the result
                System.Threading.Thread.Sleep(1500);
            }
        }

        private Cell GetValidTargetFromUser(Grid opponentGrid)
        {
            while (true)
            {
                Console.WriteLine("\nEnter target coordinates:");

                // Get row input
                Console.Write($"Row (0-{Grid.GridSize - 1}): ");
                if (!int.TryParse(Console.ReadLine(), out int row) || row < 0 || row >= Grid.GridSize)
                {
                    Console.WriteLine("Invalid row number. Please enter a number between " + $"0 and {Grid.GridSize - 1}.");
                    continue;
                }

                // Get column input
                Console.Write($"Column (0-{Grid.GridSize - 1}): ");
                if (!int.TryParse(Console.ReadLine(), out int col) || col < 0 || col >= Grid.GridSize)
                {
                    Console.WriteLine("Invalid column number. Please enter a number between " + $"0 and {Grid.GridSize - 1}.");
                    continue;
                }

                Cell targetCell = opponentGrid.Grids[row, col];

                // Check if cell has already been hit
                if (targetCell.IsHit)
                {
                    Console.WriteLine("This cell has already been targeted. Please choose another location.");
                    continue;
                }

                return targetCell;
            }
        }
    }
}
