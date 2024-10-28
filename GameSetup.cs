﻿using System;
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
            Console.WriteLine($"{player.Name}'s Grid:");
            DisplayGrid(player.PlayerGrid, true);  // Show ships for player grid

            Console.WriteLine($"\n{gameController.GetOpponent().Name}'s Grid:");
            DisplayGrid(player.OpponentGrid, false);  // Hide ships on opponent grid
        }

        private void DisplayGrid(Grid grid, bool showShips) //Update the mark for the cell, print the cell.mark instead
        {
            Console.Write("   ");
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
                        Console.Write(cell.HasShip() ? "X " : "M ");  
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
                            Cell targetCell;

                            do
                            {
                                targetCell = GetTargetCell(currentplayer.OpponentGrid);

                                if (targetCell.IsHit)
                                {
                                    Console.WriteLine("This cell has already been hit. Please enter new coordinates.");
                                }

                            } while (targetCell.IsHit);  // Repeat until an un-hit cell is selected

                            // Now call Execute with a valid, un-hit cell
                            attack.Execute(currentplayer, targetCell);
                            validAction = true;
                        }
                        else if (action is Repair repair)
                        {

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

        private Cell GetTargetCell(Grid opponentGrid)
        {
            int row = 0, col = 0;
            bool validInput = false;

            while (!validInput)
            {
                Console.WriteLine("\nEnter target coordinates:");
                Console.Write("Row (1-" + (Grid.GridSize) + "): ");
                if (int.TryParse(Console.ReadLine(), out row))
                {
                    Console.Write("Column (1-" + (Grid.GridSize) + "): ");
                    if (int.TryParse(Console.ReadLine(), out col))
                    {
                        if (row >= 0 && row <= Grid.GridSize && col >= 0 && col <= Grid.GridSize)
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
