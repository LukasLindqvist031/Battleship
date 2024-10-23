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

        public static void DisplayGrid(Grid grid) //Död kod atm
        {
            Console.WriteLine("   A B C D E F G H I J");
            for (int row = 0; row < GridSize; row++) {
                Console.Write((row + 1).ToString().PadLeft(2) + " ");

                for (int col = 0; col < GridSize; col++)
                {
                    Console.Write(grid.Grids[row, col].Mark);
                }
                Console.WriteLine();
            }
        }

        public void PlaceTestShip(int row, int col)
        {
            if (!IsValidCoordinate(row, col))
            {
                Console.WriteLine("Invalid coordinates. Please enter values between 1 and " + GridSize);
                return;
            }

            Grids[row - 1, col - 1].Ship = new Ship(3);
        }

        public void ShootTest(int row, int col)
        {
            if (!IsValidCoordinate(row, col))
            {
                Console.WriteLine("Invalid coordinates. Please enter values between 1 and " + GridSize);
                return;
            }

            Grids[row - 1, col - 1].IsHit = true;
        }

        private bool IsValidCoordinate(int row, int col)
        {
            return row >= 1 && row <= GridSize && col >= 1 && col <= GridSize;
        }
    }
}
