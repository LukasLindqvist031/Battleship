using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class Grid
    {
        private const int GridSize = 10; //The size of the game board (10x10)
        private Cell[,] cells;

        public Grid()
        {
            cells = new Cell[GridSize, GridSize];
            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    cells[row, col] = new Cell(); //Initiates every cell
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
                    var cell = cells[row, col];
                    if (cell.IsHit)
                    {
                        if (cell.Ship != null)
                        {
                            Console.Write("X "); //Ship hit
                        }
                        else
                        {
                            Console.Write("M "); //Miss (cell without ship)
                        }
                    }
                    else
                    {
                        if (cell.Ship != null)
                        {
                            Console.Write("O "); //Ship that has not been hit
                        }
                        else
                        {
                            Console.Write("~ "); //Empty water
                        }
                    }
                }
            }

            Console.WriteLine(); //New row after each row of cells
        }
        public void PlaceTestShip(int row, int col) //Testmethod to place a ship in a specific spot
        {
            cells[row, col].Ship = new Ship { Length = 3 };
        }

        public void ShootTest(int row, int col) //Testmethod to shoot a specific cell
        {
            cells[row, col].IsHit = true;
        }
    }
}