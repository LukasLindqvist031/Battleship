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

        public void PlaceShipRandomly(Grid grid, Ship ship)
        {
            bool placed = false;

            while (!placed)
            {
                bool vertical = _random.Next(2) == 0;

                int startRow = _random.Next(vertical ? Grid.GridSize - ship.Length : Grid.GridSize);
                int startColumn = _random.Next(!vertical ? Grid.GridSize - ship.Length : Grid.GridSize);

                if (IsValidPlacement(grid, ship, startRow, startColumn, vertical))
                {
                    for (int i = 0; i < ship.Length; i++)
                    {
                        if (vertical)
                        {
                            grid.Grids[startRow + i, startColumn].Ship = ship; 
                        }
                        else
                        {
                            grid.Grids[startRow, startColumn + i].Ship = ship; 
                        }
                    }
                    placed = true; 
                }
            }
        }

        private bool IsValidPlacement(Grid grid, Ship ship, int startRow, int startCol, bool vertical)
        {
            for(int i = 0; i < ship.Length; i++)
            {
                int row = vertical ? startRow + i : startRow;
                int col = vertical ? startCol : startCol + i;

                if (row >= Grid.GridSize || col >= Grid.GridSize || !grid.Grids[row, col].IsEmpty() || !IsCellIsolated(grid, row, col))
                {
                    return false; 
                }
            }
            return true;
        }

        private bool IsCellIsolated(Grid grid, int row, int col)
        {
            if (row > 0 && !grid.Grids[row - 1, col].IsEmpty())
            {
                return false;
            }

            if (row < Grid.GridSize - 1 && !grid.Grids[row + 1, col].IsEmpty())
            {
                return false;
            }

            if (col > 0 && !grid.Grids[row, col - 1].IsEmpty())
            {
                return false;
            }

            if (col < Grid.GridSize - 1 && !grid.Grids[row, col + 1].IsEmpty())
            {
                return false;
            }
            return true;
        }
    }
}