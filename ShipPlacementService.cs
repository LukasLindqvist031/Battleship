using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class ShipPlacementService : IShipPlacement
    {
        private Random _random = new Random();

        public void PlaceShipRandomly(Grid grid, Ship[] ships)
        {
            Random random = new Random();
            foreach (Ship ship in ships)
            {
                bool placed = false;
                while (!placed)
                {
                    // Get random starting position
                    var availableCells = grid.Where(cell => cell.IsEmpty()).ToList();
                    if (availableCells.Count == 0) break;

                    Cell startCell = availableCells[random.Next(availableCells.Count)];
                    placed = TryPlaceShip(grid, startCell, ship, random.Next(2) == 0);
                }
            }
        }

        private bool TryPlaceShip(Grid grid, Cell startCell, Ship ship, bool horizontal)
        {
            // Identify potential ship cells based on the starting cell and orientation
            var shipCells = horizontal
                ? grid.Where(c => c.Row == startCell.Row &&
                                c.Column >= startCell.Column &&
                                c.Column < startCell.Column + ship.Length)
                : grid.Where(c => c.Column == startCell.Column &&
                                c.Row >= startCell.Row &&
                                c.Row < startCell.Row + ship.Length);

            // Check if the selected cells are enough and if all are empty and isolated
            if (shipCells.Count() != ship.Length || shipCells.Any(c => !c.IsEmpty() || !IsCellIsolated(grid, c.Row, c.Column)))
                return false;

            // Place the ship if all checks passed
            foreach (var cell in shipCells)
            {
                cell.Ship = ship;
            }
            return true;
        }

        private bool IsCellIsolated(Grid grid, int row, int col)
        {
            // Use LINQ to find the neighbors by checking row and column offsets
            var neighbors = grid.Where(cell =>
                (cell.Row == row - 1 && cell.Column == col) ||    // Up
                (cell.Row == row + 1 && cell.Column == col) ||    // Down
                (cell.Row == row && cell.Column == col - 1) ||    // Left
                (cell.Row == row && cell.Column == col + 1));     // Right

            // Check if any neighbor cell is occupied
            return neighbors.All(cell => cell.IsEmpty());
        }

    }
}