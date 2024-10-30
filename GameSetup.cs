using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class GameSetup
    {
        private readonly ShipPlacementService _shipPlacementService;

        public GameSetup()
        {
            _shipPlacementService = new ShipPlacementService();
        }
        private string GetPlayerName()
        {
            Console.WriteLine("Welcome to Battleship!");
            Console.Write("Enter your name: ");
            string name = Console.ReadLine()?.Trim() ?? "Player";
            return string.IsNullOrEmpty(name) ? "Player" : name;
        }

        private IShootingStrategy SelectComputerStrategy()
        {
            var strategies = new List<MenuItem<IShootingStrategy>>
        {
            new MenuItem<IShootingStrategy>("Random Strategy", new RandomShooting()),
            new MenuItem<IShootingStrategy>("Intelligent Strategy", new IntelligentShooting()),
        };

            var menu = new SimpleMenu<IShootingStrategy>(strategies);
            

            while (true)
            {
                //Console.WriteLine("\nSelect computer's strategy:");
                menu.Draw();
                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        menu.Up();
                        break;
                    case ConsoleKey.DownArrow:
                        menu.Down();
                        break;
                    case ConsoleKey.Enter:
                        return menu.GetSelectedItem().Value;
                }
            }
        }


        public void Run()
        {
            // Welcome to Battleship! Press Enter to continue
            // What's your name? 
            // Select a strategy for the computer
            // Create Human and Computer
            // Start playing by selecting a square
            //Menu will be implemented among other places, in ShootingStrategy for Human in his UserShootingStrategy

            Console.Clear();
            string playerName = GetPlayerName();
            Console.Clear();
            IShootingStrategy computerStrategy = SelectComputerStrategy();
            

            Grid playerGrid = new Grid();
            Grid computerGrid = new Grid();

            var playerActions = new List<IPlayerAction> //Code reuse
            {
                new Attack(computerGrid),
                new Repair()
            };
            var computerActions = new List<IPlayerAction>
            {
                new Attack(playerGrid),
                new Repair()
            };

            Ship[] ships = new Ship[]
            {
                new Ship(5),
                new Ship(4),
                new Ship(3),
                new Ship(2),
                new Ship(1)
            };

            _shipPlacementService.PlaceShipRandomly(playerGrid, ships);
            _shipPlacementService.PlaceShipRandomly(computerGrid, ships.Select(s => s.Clone()).ToArray());

            var playerShips = ships.ToList();
            var computerShips = ships.Select(s => s.Clone()).ToList();

            var human = new Human(
            name: playerName,
            playerGrid: playerGrid,
            opponentGrid: computerGrid,
            ships: playerShips,
            actions: playerActions,
            shootingStrategy: new RandomShooting()
            );

            var computer = new Computer(
                name: "Computer",
                playerGrid: computerGrid,
                opponentGrid: playerGrid,
                ships: computerShips,
                actions: computerActions,
                shootingStrategy: new IntelligentShooting()
            );

            var gameController = new GameController(human, computer);
            RunGameLoop(gameController);
        }

        private void RunGameLoop(GameController gameController)
        {
            bool gameOver = false;
            while (!gameOver)
            {
                Player currentPlayer = gameController.GetCurrentPlayer();
                Console.Clear();

                if (currentPlayer is Human)
                {
                    DisplayGameState(currentPlayer, gameController);
                    HandleHumanTurn(currentPlayer);
                }
                else if (currentPlayer is Computer)
                {
                    HandleComputerTurn(currentPlayer);
                    System.Threading.Thread.Sleep(1000); // Brief pause for transition
                }

                gameOver = gameController.CheckGameOver(); 
                gameController.SwitchPlayer();
            }

            Console.WriteLine("Game Over!");
        }
        private void DisplayGameState(Player player, GameController gameController)
        {
            Console.WriteLine($"{player.Name}'s Grid:".PadRight(25) + $"{gameController.GetOpponent().Name}'s Grid:");
            DisplaySideBySideGrids(player.PlayerGrid, player.OpponentGrid, hideOpponentShips: true);
        }

        private void DisplaySideBySideGrids(Grid playerGrid, Grid opponentGrid, bool hideOpponentShips)
        {
            Console.WriteLine("  0 1 2 3 4 5 6 7 8 9   ".PadRight(25) + "  0 1 2 3 4 5 6 7 8 9");
            for (int row = 0; row < Grid.GridSize; row++)
            {
                // Display player grid row
                Console.Write($"{row} ");
                for (int col = 0; col < Grid.GridSize; col++)
                {
                    char playerSymbol = GetCellSymbol(playerGrid.Grids[row, col], false);
                    Console.Write($"{playerSymbol} ");
                }

                // Spacer between grids
                Console.Write("   ");

                // Display opponent grid row
                Console.Write($"{row} ");
                for (int col = 0; col < Grid.GridSize; col++)
                {
                    char opponentSymbol = GetCellSymbol(opponentGrid.Grids[row, col], hideOpponentShips);
                    Console.Write($"{opponentSymbol} ");
                }

                Console.WriteLine();
            }
        }

        public void DisplayGrid(Grid grid, bool hideShips = false)
        {
            Console.WriteLine("  0 1 2 3 4 5 6 7 8 9");
            int currentRow = 0;
            int cellsInRow = 0;

            foreach (Cell cell in grid)
            {
                if (cellsInRow == 0)
                {
                    Console.Write($"{currentRow} ");
                }

                char symbol = GetCellSymbol(cell, hideShips);
                Console.Write($"{symbol} ");

                cellsInRow++;
                if (cellsInRow == Grid.GridSize)
                {
                    Console.WriteLine();
                    cellsInRow = 0;
                    currentRow++;
                }
            }
        }

        private char GetCellSymbol(Cell cell, bool hideShips)
        {
            if (cell.IsHit && cell.HasShip()) return 'X'; // Hit ship
            if (cell.IsHit && cell.IsEmpty()) return 'M'; // Missed shot
            if (!cell.IsHit && cell.HasShip() && !hideShips) return 'O'; // Repaired ship part, visible to owner
            if (!cell.IsHit && cell.IsEmpty()) return '~'; // Water for empty, unhit cells
            return '~'; // Default to water for any remaining cases
        }


        private void HandleHumanTurn(Player currentPlayer)
        {
            var menuItem = new List<MenuItem<IPlayerAction>>
            {
                new MenuItem<IPlayerAction>("Attack", currentPlayer.Actions[0]),
                new MenuItem<IPlayerAction>("Repair", currentPlayer.Actions[1])
            };

            var menu = new SimpleMenu<IPlayerAction>(menuItem);
            bool validAction = false;

            while (!validAction)
            {
                menu.Draw();

                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        menu.Up();
                        break;
                    case ConsoleKey.DownArrow:
                        menu.Down();
                        break;
                    case ConsoleKey.Enter:
                        var selectedItem = menu.GetSelectedItem();
                        var action = selectedItem.Value;

                        if (action is Attack attack)
                        {
                            int messageRow = Console.WindowHeight - 2;
                            Console.SetCursorPosition(0, messageRow);
                            Console.Write(new string(' ', Console.WindowWidth));

                            Cell targetCell = null;
                            bool validInput = false;
                            do
                            {
                                Console.SetCursorPosition(0, messageRow);
                                Console.WriteLine("Enter target coordinates:");

                                Console.Write("Row (0-" + (Grid.GridSize - 1) + "): ");
                                int row = int.TryParse(Console.ReadLine(), out var tempRow) ? tempRow : -1; //Better handling of invalid inputs

                                Console.Write("Column (0-" + (Grid.GridSize - 1) + "): ");
                                int col = int.TryParse(Console.ReadLine(), out var tempCol) ? tempCol : -1; //Better handling of invalid inputs

                                if (row >= 0 && row < Grid.GridSize && col >= 0 && col < Grid.GridSize)
                                {
                                    targetCell = currentPlayer.OpponentGrid.Grids[row, col];
                                    if (!targetCell.IsHit)
                                    {
                                        validInput = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("This cell has already been hit. Please enter new coordinates.");
                                        Console.SetCursorPosition(0, messageRow + 1);
                                        Console.Write(new string(' ', Console.WindowWidth));
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid coordinates. Please enter values within the grid range.");
                                    Console.SetCursorPosition(0, messageRow + 1);
                                    Console.Write(new string(' ', Console.WindowWidth));
                                }

                            } while (!validInput);

                            attack.Execute(currentPlayer, targetCell);
                            validAction = true;
                        }


                        else if (action is Repair repair)
                        {
                            if (repair.AttemptRepair(currentPlayer))
                            {
                                Cell targetCell;
                                bool validCell = false;

                                while (!validCell)
                                {
                                    // Prompt for coordinates
                                    Console.WriteLine("Enter coordinates of the cell to repair:");

                                    Console.Write("Row (0-" + (Grid.GridSize - 1) + "): ");
                                    int row = int.Parse(Console.ReadLine() ?? "0");

                                    Console.Write("Column (0-" + (Grid.GridSize - 1) + "): ");
                                    int col = int.Parse(Console.ReadLine() ?? "0");

                                    // Ensure the coordinates are within grid bounds (0-based index)
                                    if (row >= 0 && row < Grid.GridSize && col >= 0 && col < Grid.GridSize)
                                    {
                                        targetCell = currentPlayer.PlayerGrid.Grids[row, col];

                                        // Check if the selected cell is part of a damaged ship
                                        if (targetCell.IsHit)
                                        {
                                            repair.Execute(currentPlayer, targetCell);
                                            validCell = true;
                                            validAction = true;
                                        }
                                        else
                                        {
                                            Console.WriteLine("This cell is not damaged. Please choose a damaged cell.");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid coordinates. Please enter coordinates within the grid.");
                                    }
                                }
                            }
                            else
                            {
                                Console.SetCursorPosition(0, menu.menuTop + menu._menuItems.Count + 1);
                                Console.WriteLine("No damaged ships to repair. Please choose another action.");
                            }
                        }
                        break;
                }
            }
        }

        private void HandleComputerTurn(Player computerPlayer)
        {
            if (computerPlayer.ShootingStrategy != null)
            {
                computerPlayer.ShootingStrategy.Shoot(computerPlayer);
                Console.WriteLine($"{computerPlayer.Name} has completed its turn.");
            }
        }

        private Cell GetTargetCell(Grid opponentGrid) //Denna är onödig, allt detta görs i HandleHumanTurn nu (testade att kommentera bort den o köra och det funkade).
        {
            int row = 0, col = 0;
            bool validInput = false;

            while (!validInput)
            {
                Console.WriteLine("\nEnter target coordinates:");
                Console.Write("Row (0-" + (Grid.GridSize-1) + "): ");
                if (int.TryParse(Console.ReadLine(), out row))
                {
                    Console.Write("Column (0-" + (Grid.GridSize-1) + "): ");
                    if (int.TryParse(Console.ReadLine(), out col))
                    {
                        if (row >= 0 && row < Grid.GridSize && col >= 0 && col < Grid.GridSize)
                        {
                            validInput = true;
                        }
                    }
                }
                if (!validInput)
                {
                    Console.WriteLine("Invalid coordinates. Please try again.");
                }
            }
            return opponentGrid.Grids[row-1, col-1];
        }
    }
}
