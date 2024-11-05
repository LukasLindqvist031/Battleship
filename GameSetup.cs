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

            // Set the Player property for each grid
            playerGrid.Player = human;

            var gameController = new GameController(human, computer, _display);
            RunGameLoop(gameController);
        }

        private void RunGameLoop(GameController gameController)
        {
            bool gameOver = false;
            Player lastPlayer = null; //To be able to use the name in the Game Over announcement

            Console.Clear();
            TextPresentation.WriteCenteredTextWithDelay("Deploying fleet...");
            Thread.Sleep(2000);
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
            _display.DrawGrid(player.PlayerGrid, player.OpponentGrid, hideShips: true, null, null);
        }

        private void HandleHumanTurn(Player currentPlayer)
        {
            var menuItems = new List<MenuItem<IPlayerAction>>
            {
                new("Attack", currentPlayer.Actions[0]),
                new("Repair", currentPlayer.Actions[1])
            };
            var menu = new SimpleMenu<IPlayerAction>(menuItems, belowGrid: true);

            bool actionCompleted = false;
            while (!actionCompleted)
            {
                Console.Clear();
                DisplayGameState(currentPlayer, _gameController); // Redraw the grid without highlighting
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
                        var selectedAction = menu.GetSelectedItem();
                        if (selectedAction is Attack)
                        {
                            actionCompleted = HandleAttack(currentPlayer);
                        }
                        else if (selectedAction is Repair repair)
                        {
                            actionCompleted = HandleRepair(currentPlayer, repair);
                        }
                        break;
                }
            }
        }

        private bool HandleAttack(Player currentPlayer)
        {
            var navigator = currentPlayer.OpponentGridNavigator; // Use the existing navigator
            while (true)
            {
                Console.Clear();
                _display.DrawGrid(currentPlayer.PlayerGrid, currentPlayer.OpponentGrid, true, null, navigator);
                TextPresentation.WriteCenteredText("Use arrow keys to move, Enter to attack, Esc to cancel", Console.WindowHeight - 2);

                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        navigator.MoveUp();
                        break;
                    case ConsoleKey.DownArrow:
                        navigator.MoveDown();
                        break;
                    case ConsoleKey.LeftArrow:
                        navigator.MoveLeft();
                        break;
                    case ConsoleKey.RightArrow:
                        navigator.MoveRight();
                        break;
                    case ConsoleKey.Enter:
                        var targetCell = navigator.GetCurrentCell();
                        if (!targetCell.IsHit)
                        {
                            currentPlayer.Actions.OfType<Attack>().First().Execute(currentPlayer, targetCell);
                            return true;
                        }
                        else
                        {
                            TextPresentation.WriteCenteredText("This cell has already been attacked. Choose another.", Console.WindowHeight - 1);
                        }
                        break;
                    case ConsoleKey.Escape:
                        return false;
                }
            }
        }

        private bool HandleRepair(Player currentPlayer, Repair repair)
        {
            if (!repair.AttemptRepair(currentPlayer))
            {
                Console.Clear();
                _display.DrawGrid(currentPlayer.PlayerGrid, currentPlayer.OpponentGrid, true, null, null);
                TextPresentation.WriteCenteredText("No damaged ships to repair. Choose another action.", Console.WindowHeight - 3);
                TextPresentation.WriteCenteredText("Press any key to continue...", Console.WindowHeight - 2);
                Console.ReadKey(true);
                return false;
            }

            var navigator = currentPlayer.PlayerGridNavigator;
            while (true)
            {
                Console.Clear();
                _display.DrawGrid(currentPlayer.PlayerGrid, currentPlayer.OpponentGrid, true, navigator, null);
                TextPresentation.WriteCenteredText("Use arrow keys to move, Enter to repair, Esc to cancel", Console.WindowHeight - 3);

                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        navigator.MoveUp();
                        break;
                    case ConsoleKey.DownArrow:
                        navigator.MoveDown();
                        break;
                    case ConsoleKey.LeftArrow:
                        navigator.MoveLeft();
                        break;
                    case ConsoleKey.RightArrow:
                        navigator.MoveRight();
                        break;
                    case ConsoleKey.Enter:
                        var targetCell = navigator.GetCurrentCell();
                        if (targetCell.IsHit && targetCell.HasShip())
                        {
                            repair.Execute(currentPlayer, targetCell);
                            return true;
                        }
                        else
                        {
                            TextPresentation.WriteCenteredText("This cell cannot be repaired. Choose another.", Console.WindowHeight - 2);
                        }
                        break;
                    case ConsoleKey.Escape:
                        return false;
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

            bool shouldRepair = random.Next(1, 101) <= 30 && damagedCells.Any();

            if (shouldRepair)
            {
                var repairAction = computerPlayer.Actions.OfType<Repair>().FirstOrDefault();
                if (repairAction != null)
                {
                    Cell targetCell = damagedCells[random.Next(damagedCells.Count)];
                    repairAction.Execute(computerPlayer, targetCell);
                    TextPresentation.WriteCenteredText($"{computerPlayer.Name} chose to repair a damaged cell at ({targetCell.Row}), ({targetCell.Column}).");
                    Thread.Sleep(1500); // Give time to read the message
                }
            }
            else
            {
                computerPlayer.ShootingStrategy?.Shoot(computerPlayer);
                TextPresentation.WriteCenteredText($"{computerPlayer.Name} has completed its turn by shooting.");
                Thread.Sleep(1500); // Give time to read the message
            }
        }
    }
}