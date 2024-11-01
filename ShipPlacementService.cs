using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    // KRAV #5:
    // 1: LINQ
    // 2: LINQ querys hjälper till att identifiera tillgängliga celler för skeppsplacering genom att filtrera och verifiera cellisolering.
    // 3: LINQ förenklar logiken, vilket gör koden lättare att läsa.
    public class ShipPlacementService : IShipPlacement
    {
        public void PlaceShipRandomly(Grid grid, Ship[] ships)
        {
            Random random = new Random();
            foreach (Ship ship in ships)
            {
                bool placed = false;
                while (!placed)
                {
                    var availableCells = grid.Where(cell => cell.IsEmpty()).ToList();
                    if (availableCells.Count == 0) break;

                    Cell startCell = availableCells[random.Next(availableCells.Count)];
                    placed = TryPlaceShip(grid, startCell, ship, random.Next(2) == 0);
                }
            }
        }
        private bool TryPlaceShip(Grid grid, Cell startCell, Ship ship, bool horizontal) //LINQ-kravet
        {
            var shipCells = horizontal
                ? grid.Where(c => c.Row == startCell.Row &&
                                c.Column >= startCell.Column &&
                                c.Column < startCell.Column + ship.Length)
                : grid.Where(c => c.Column == startCell.Column &&
                                c.Row >= startCell.Row &&
                                c.Row < startCell.Row + ship.Length);

            if (shipCells.Count() != ship.Length || shipCells.Any(c => !c.IsEmpty() || !IsCellIsolated(grid, c.Row, c.Column)))
                return false;

            foreach (var cell in shipCells)
            {
                cell.Ship = ship;
            }
            return true;
        }

        private bool IsCellIsolated(Grid grid, int row, int col)
        {
            var neighbors = grid.Where(cell =>
                (cell.Row == row - 1 && cell.Column == col) ||
                (cell.Row == row + 1 && cell.Column == col) ||
                (cell.Row == row && cell.Column == col - 1) ||
                (cell.Row == row && cell.Column == col + 1));

            return neighbors.All(cell => cell.IsEmpty());
        }

    }
}