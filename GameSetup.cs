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
        private readonly IDisplay _display;

        public GameSetup(IDisplay display)
        {
            _shipPlacementService = new ShipPlacementService();
            _display = display;
        }
        private string GetPlayerName()
        {
            Console.WriteLine("Welcome to Battleship!");
            Console.Write("Enter your name: ");
            string name = Console.ReadLine()?.Trim() ?? "Player";
            Console.Clear();
            return string.IsNullOrEmpty(name) ? "Player" : name;
        }

        private IShootingStrategy SelectComputerStrategy()
        {
            _display.ShowMessage("Select the computer's strategy:");
            var strategies = new List<MenuItem<IShootingStrategy>>
        {
            new MenuItem<IShootingStrategy>("Random Strategy", new RandomShooting()),
            new MenuItem<IShootingStrategy>("Intelligent Strategy", new IntelligentShooting())
        };

            var strategyMenu = new SimpleMenu<IShootingStrategy>(strategies);
            var navigator = new ActionNavigator<IShootingStrategy>(strategyMenu);

            return navigator.Navigate();
        }


        public void Run()
        {
            string playerName = GetPlayerName();
            IShootingStrategy computerStrategy = SelectComputerStrategy();

            Grid playerGrid = new Grid();
            Grid computerGrid = new Grid();

            var playerActions = new List<IPlayerAction> { new Attack(computerGrid), new Repair() };
            var computerActions = new List<IPlayerAction> { new Attack(playerGrid), new Repair() };

            Ship[] ships = { new Ship(5), new Ship(4), new Ship(3), new Ship(2), new Ship(1) };
            _shipPlacementService.PlaceShipRandomly(playerGrid, ships);
            _shipPlacementService.PlaceShipRandomly(computerGrid, ships.Select(s => s.Clone()).ToArray());

            var human = new Human(playerName, playerGrid, computerGrid, ships.ToList(), playerActions, new UserShooting());
            var computer = new Computer("Computer", computerGrid, playerGrid, ships.Select(s => s.Clone()).ToList(), computerActions, computerStrategy);

            var gameController = new GameController(human, computer, _display);
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
            _display.DrawGrid(player.PlayerGrid, player.OpponentGrid, hideShips: true);
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
                        var action = selectedItem;

                        if (action is Attack attack)
                        {
                            currentPlayer.ShootingStrategy.Shoot(currentPlayer);
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
                                //Console.SetCursorPosition(0, menu.menuTop + menu._menuItems.Count + 1);
                                Console.WriteLine("No damaged ships to repair. Please choose another action.");
                            }
                        }
                        break;
                }
            }
        }

        private void HandleComputerTurn(Player computerPlayer)
        {
            Random random = new Random();

            // Check if there are any damaged cells in non-sunk ships
            var damagedCells = computerPlayer.PlayerGrid.Grids
                .Cast<Cell>()
                .Where(cell => cell.IsHit && cell.Ship != null && !cell.Ship.IsSunk())
                .ToList();

            // If no damaged cells are found, the computer should attack
            if (!damagedCells.Any())
            {
                computerPlayer.ShootingStrategy?.Shoot(computerPlayer);
                Console.WriteLine($"{computerPlayer.Name} has completed its turn by shooting.");
                return;
            }

            // 30% chance to repair if there are damaged cells
            bool shouldRepair = random.Next(1, 101) <= 30;

            if (shouldRepair)
            {
                // Find the Repair action within the computer's actions
                var repairAction = computerPlayer.Actions.OfType<Repair>().FirstOrDefault();

                if (repairAction != null)
                {
                    // Select a random damaged cell from a non-sunk ship
                    Cell targetCell = damagedCells[random.Next(damagedCells.Count)];
                    repairAction.Execute(computerPlayer, targetCell);
                    Console.WriteLine($"{computerPlayer.Name} chose to repair a damaged cell at ({targetCell.Row}, {targetCell.Column}).");
                }
                else
                {
                    Console.WriteLine($"{computerPlayer.Name} attempted to repair but had no repair action available.");
                }
            }
            else
            {
                // If repair is not chosen, proceed with shooting
                computerPlayer.ShootingStrategy?.Shoot(computerPlayer);
                Console.WriteLine($"{computerPlayer.Name} has completed its turn by shooting.");
            }
        }
    }
}
