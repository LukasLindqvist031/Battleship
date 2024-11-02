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
        private GameController _gameController;

        public GameSetup(IDisplay display)
        {
            _shipPlacementService = new ShipPlacementService();
            _display = display;
        }

        private void DisplayWelcome()
        {
            TextPresentation.WriteCenteredTextWithDelay("Welcome to Battleship!");
            Console.ReadLine();
            Console.Clear();
        }

        public string GetPlayerName()
        {
            TextPresentation.WriteCenteredTextWithDelay("Enter your name:", 50, leaveCursorBelow: true);
            string name = Console.ReadLine()?.Trim() ?? "Player";
            Console.Clear();

            return string.IsNullOrEmpty(name) ? "Player" : name;
        }

        private IShootingStrategy SelectComputerStrategy()
        {
            var centerY = Console.WindowHeight / 2;
            TextPresentation.WriteCenteredTextWithDelay("Select the computer's strategy:", 50, yPos: centerY);

            var strategies = new List<MenuItem<IShootingStrategy>>
            {
                new("Random Strategy", new RandomShooting()),
                new("Intelligent Strategy", new IntelligentShooting())
            };

            // Position the menu directly below the text
            var strategyMenu = new SimpleMenu<IShootingStrategy>(strategies, specificY: centerY + 1);
            var navigator = new ActionNavigator<IShootingStrategy>(strategyMenu);

            return navigator.Navigate();
        }

        public void Run()
        {
            DisplayWelcome();
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
            Player lastPlayer = null; //To be able to use the name in the Game Over announcement

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
                    System.Threading.Thread.Sleep(1500); // Brief pause for transition
                }

                gameOver = gameController.CheckGameOver();
                lastPlayer = currentPlayer; //To be able to use the name in the Game Over announcement
                gameController.SwitchPlayer();
            }

            Console.Clear();
            TextPresentation.WriteCenteredTextWithDelay("Game Over!", yPos: (Console.WindowHeight / 2) - 1); 
            TextPresentation.WriteCenteredTextWithDelay($"{lastPlayer.Name} is the winner!", yPos: (Console.WindowHeight / 2) + 1);
            Console.ReadLine();
        }

        private void DisplayGameState(Player player, GameController gameController)
        {
            //Console.WriteLine($"{player.Name}'s Grid:".PadRight(25) + $"{gameController.GetOpponent().Name}'s Grid:");
            _display.DrawGrid(player.PlayerGrid, player.OpponentGrid, hideShips: true);
        }

        private void HandleHumanTurn(Player currentPlayer)
        {
            var menuItem = new List<MenuItem<IPlayerAction>>
        {
            new("Attack", currentPlayer.Actions[0]),
            new("Repair", currentPlayer.Actions[1])
        };

            var menu = new SimpleMenu<IPlayerAction>(menuItem, belowGrid: true);
            bool validAction = false;

            while (!validAction)
            {
                Console.Clear();
                DisplayGameState(currentPlayer, _gameController);
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
                                    Console.SetCursorPosition(0, Console.WindowHeight / 2 + 9);
                                    Console.WriteLine("Enter coordinates of the cell to repair:");

                                    Console.Write("Row (0-" + (Grid.GridSize - 1) + "): ");
                                    int row = int.Parse(Console.ReadLine() ?? "0");

                                    Console.Write("Column (0-" + (Grid.GridSize - 1) + "): ");
                                    int col = int.Parse(Console.ReadLine() ?? "0");

                                    if (row >= 0 && row < Grid.GridSize && col >= 0 && col < Grid.GridSize)
                                    {
                                        targetCell = currentPlayer.PlayerGrid.Grids[row, col];

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
                                Console.WriteLine("No damaged ships to repair. Please choose another action.");
                            }
                        }
                        break;
                }
            }
        }



        private void HandleComputerTurn(Player computerPlayer)
        {
            var random = new Random();

            var damagedCells = computerPlayer.PlayerGrid.Grids
                .Cast<Cell>()
                .Where(cell => cell.IsHit && cell.Ship != null && !cell.Ship.IsSunk())
                .ToList();

            if (!damagedCells.Any())
            {
                computerPlayer.ShootingStrategy?.Shoot(computerPlayer);
                Console.WriteLine($"{computerPlayer.Name} has completed its turn by shooting.");
                return;
            }

            bool shouldRepair = random.Next(1, 101) <= 30;

            if (shouldRepair)
            {
                var repairAction = computerPlayer.Actions.OfType<Repair>().FirstOrDefault();

                if (repairAction != null)
                {
                    Cell targetCell = damagedCells[random.Next(damagedCells.Count)];
                    repairAction.Execute(computerPlayer, targetCell);
                    Console.WriteLine(
                        $"{computerPlayer.Name} chose to repair a damaged cell at ({targetCell.Row}, {targetCell.Column}).");
                }
                else
                {
                    computerPlayer.ShootingStrategy?.Shoot(computerPlayer);
                    Console.WriteLine($"{computerPlayer.Name} has completed its turn by shooting.");
                }
            }
        }
    }
}
