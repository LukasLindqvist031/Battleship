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
        public List<Cell> Grids { get; set; }
        

        public Grid()
        {
            Grids = new List<Cell>();
            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    Grids.Add(new Cell(row, col));
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
                    int index = row * GridSize + col; //??????
                    var cell = Grids[index];

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
                Console.WriteLine(); //New row after each row of cells
            }
        }
        public void PlaceTestShip(int row, int col)
        {
            // Validate the row and col inputs are within range
            if (row < 1 || row > GridSize || col < 1 || col > GridSize) //Make this logic in a seperate method
            {
                Console.WriteLine("Invalid coordinates. Please enter values between 1 and " + GridSize);
                return;
            }

            // Subtract 1 from row and col to convert 1-based input to 0-based index
            int index = (row - 1) * GridSize + (col - 1);
            Grids[index].Ship = new Ship { Length = 3 };
        }

        public void ShootTest(int row, int col)
        {
            // Validate the row and col inputs are within range
            if (row < 1 || row > GridSize || col < 1 || col > GridSize) //Make this logic in a seperate method
            {
                Console.WriteLine("Invalid coordinates. Please enter values between 1 and " + GridSize);
                return;
            }

            // Subtract 1 from row and col to convert 1-based input to 0-based index
            int index = (row - 1) * GridSize + (col - 1);
            Grids[index].IsHit = true;
        }
    }
}