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

        public void Run()
        {
            // Welcome to Battleship! Press Enter to continue
            // What's your name? 
            // Select a strategy for the computer
            // Create Human and Computer
            // Start playing by selecting a square
            //Menu will be implemented among other places, in ShootingStrategy for Human in his UserShootingStrategy

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

            var playerShips = new List<Ship> //Code Reuse? Reference type? 
            {
                new Ship(5), 
                new Ship(4), 
                new Ship(3), 
                new Ship(3), 
                new Ship(2)  
            };

            var computerShips = new List<Ship>
            {
                new Ship(5),
                new Ship(4),
                new Ship(3),
                new Ship(3),
                new Ship(2)
            };

            foreach (var ship in playerShips)
            {
                _shipPlacementService.PlaceShipRandomly(playerGrid, ship);
            }
            foreach (var ship in computerShips)
            {
                _shipPlacementService.PlaceShipRandomly(computerGrid, ship);
            }

            var human = new Human(
            name: "Player",
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
                shootingStrategy: new RandomShooting()
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

                if(currentPlayer is Human)
                {
                    DisplayGameState(currentPlayer, gameController);
                    HandleHumanTurn(currentPlayer);

                }
            }
        }
        private void DisplayGameState(Player player, GameController gameController)
        {
            Console.WriteLine($"{player.Name}'s Grid:");
            DisplayGrid(player.PlayerGrid, true);  // Show ships for player grid

            Console.WriteLine($"\n{gameController.GetOpponent().Name}'s Grid:");
            DisplayGrid(player.OpponentGrid, false);  // Hide ships on opponent grid
        }

        private void DisplayGrid(Grid grid, bool showShips) //Update the mark for the cell, print the cell.mark instead
        {
            Console.Write("  ");
            for (int i = 0; i < Grid.GridSize; i++)
            {
                Console.Write($"{(char)('A' + i)} ");
            }
            Console.WriteLine();

            for (int row = 0; row < Grid.GridSize; row++)
            {
                Console.Write($"{row + 1}".PadLeft(2) + " ");  
                for (int col = 0; col < Grid.GridSize; col++)
                {
                    var cell = grid.Grids[row, col];
                    if (cell.IsHit)
                    {
                        Console.Write(cell.HasShip() ? "X " : "• ");  
                    }
                    else if (showShips && cell.HasShip())
                    {
                        Console.Write("O ");  
                    }
                    else
                    {
                        Console.Write("~ ");  
                    }
                }
                Console.WriteLine();
            }
        }

        private void HandleHumanTurn(Player currentplayer)
        {
            var menuItem = new List<MenuItem<IPlayerAction>>
            {
                new MenuItem<IPlayerAction>("Attack", currentplayer.Actions[0]),
                new MenuItem<IPlayerAction>("Repair", currentplayer.Actions[1])
            };

            var menu = new SimpleMenu<IPlayerAction>(menuItem);
            bool validAction = false;

            while (!validAction)
            {
                //Console.WriteLine("\n Choose your action:");

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

                        if(action is Attack attack)
                        {
                            //Implementera logiken för att attackera ett skepp
                        }

                        validAction = true;
                        break;
                }
            }
        }

        private void HandleComputerTurn(Player currentplayer)
        {

        }
    }
}
