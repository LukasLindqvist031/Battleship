using System;

namespace Battleship
{
    public class Grid
    {
        private const int GridSize = 10; //The size of the game board (10x10)
        public Cell[,] Grids { get; set; } // Use a 2D array instead of a list

        public Grid()
        {
            Grids = new Cell[GridSize, GridSize]; // Initialize the array
            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    Grids[row, col] = new Cell(row, col); // Initialize each cell in the array
                }
            }
        }

        public void PrintGrid()
        {
            Console.WriteLine("   A B C D E F G H I J");
            for (int row = 0; row < GridSize; row++)
            {
                Console.Write((row + 1).ToString().PadLeft(2) + " ");

                for (int col = 0; col < GridSize; col++)
                {
                    var cell = Grids[row, col]; 

                    switch (cell)
                    {
                        case { IsHit: true, Ship: { } }:
                            Console.Write("X ");
                            break;
                        case { IsHit: true, Ship: null }:
                            Console.Write("M ");
                            break;
                        case { IsHit: false, Ship: { } }:
                            Console.Write("O ");
                            break;
                        case { IsHit: false, Ship: null }:
                            Console.Write("~ ");
                            break;
                    }
                }
                Console.WriteLine(); // New row after each row of cells
            }
        }

        public void PlaceTestShip(int row, int col)
        {
            // Validate the row and col inputs are within range
            if (!IsValidCoordinate(row, col))
            {
                Console.WriteLine("Invalid coordinates. Please enter values between 1 and " + GridSize);
                return;
            }

            Grids[row - 1, col - 1].Ship = new Ship { Length = 3 };
        }

        public void ShootTest(int row, int col)
        {
            // Validate the row and col inputs are within range
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
