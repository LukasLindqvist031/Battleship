using System;

namespace Battleship
{
    public class Grid
    {
        public const int GridSize = 10;
        public Cell[,] Grids { get; set; }

        public Grid()
        {
            Grids = new Cell[GridSize, GridSize];
            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    Grids[row, col] = new Cell(row, col);
                }
            }
        }
    }
}
