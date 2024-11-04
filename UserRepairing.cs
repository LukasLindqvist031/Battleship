using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class UserRepairing
    {
        public bool HandleRepair(Player humanPlayer)
        {
            // Get a list of damaged cells
            var damagedCells = humanPlayer.PlayerGrid.Grids
                .Cast<Cell>()
                .Where(cell => cell.IsHit && cell.Ship != null && !cell.Ship.IsSunk())
                .ToList();

            // Check if there are any damaged cells
            if (!damagedCells.Any())
            {
                Console.WriteLine("No damaged ships to repair. Choose another action.");
                return false; // Return false if there are no damaged ships
            }

            // Prompt the user for coordinates to repair a damaged cell
            while (true)
            {
                Console.WriteLine("Enter the coordinates of the cell you want to repair:");
                Cell targetCell = GetValidTargetFromUser(humanPlayer.PlayerGrid);

                // Check if the selected cell is part of the damaged cells
                if (damagedCells.Contains(targetCell))
                {
                    var repairAction = humanPlayer.Actions.OfType<Repair>().FirstOrDefault();
                    if (repairAction != null)
                    {
                        repairAction.Execute(humanPlayer, targetCell);
                        Console.WriteLine($"Repaired ship at ({targetCell.Row}, {targetCell.Column}).");
                        return true; // Return true after a successful repair
                    }
                }
                else
                {
                    // Notify the user that the selected cell is not damaged
                    Console.WriteLine("The selected cell is not damaged. Please choose a damaged cell.");
                }
            }
        }

        private Cell GetValidTargetFromUser(Grid playerGrid)
        {
            while (true)
            {
                Console.Write($"Row (0-{Grid.GridSize - 1}): ");
                if (!int.TryParse(Console.ReadLine(), out int row) || row < 0 || row >= Grid.GridSize)
                {
                    Console.WriteLine($"Invalid row number. Please enter a number between 0 and {Grid.GridSize - 1}.");
                    continue;
                }

                Console.Write($"Column (0-{Grid.GridSize - 1}): ");
                if (!int.TryParse(Console.ReadLine(), out int col) || col < 0 || col >= Grid.GridSize)
                {
                    Console.WriteLine($"Invalid column number. Please enter a number between 0 and {Grid.GridSize - 1}.");
                    continue;
                }

                return playerGrid.Grids[row, col];
            }
        }
    }
}
