using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class GameController
    {
        private readonly Player _humanPlayer;
        private readonly Player _computerPlayer;
        private Player _currentPlayer;
        private bool _isGameOver;

        public GameController(Player humanPlayer, Player computerPlayer)
        {
            _humanPlayer = humanPlayer;
            _computerPlayer = computerPlayer;
            _currentPlayer = humanPlayer; // Human goes first
            _isGameOver = false;
        }
        public Player GetCurrentPlayer()
        {
            return _currentPlayer;
        }

        public Player GetOpponent()
        {
            return _currentPlayer == _humanPlayer ? _computerPlayer : _humanPlayer;
        }

        private void DisplayGameState()
        {
            Console.Clear();
            Console.WriteLine($"\n{_currentPlayer.Name}'s turn");

            if (_currentPlayer == _humanPlayer)
            {
                Console.WriteLine("\nYour Grid:");
                DisplayGrid(_humanPlayer.PlayerGrid, true);

                Console.WriteLine("\nOpponent's Grid:");
                DisplayGrid(_humanPlayer.OpponentGrid, false);
            }
        }

        private void DisplayGrid(Grid grid, bool showShips)
        {
            Console.WriteLine("  A B C D E F G H I J");
            for (int row = 0; row < 10; row++)
            {
                Console.Write((row + 1).ToString().PadLeft(2) + " ");
                for (int col = 0; col < 10; col++)
                {
                    var cell = grid.Grids[row, col];
                    char symbol = GetCellSymbol(cell, showShips);
                    Console.Write($"{symbol} ");
                }
                Console.WriteLine();
            }
        }

        private char GetCellSymbol(Cell cell, bool showShips)
        {
            if (cell.IsHit)
            {
                return cell.HasShip() ? 'X' : 'O';
            }

            if (showShips && cell.HasShip())
            {
                return 'S';
            }

            return '~';
        }

        private IPlayerAction GetHumanAction(List<IPlayerAction> availableActions)
        {
            while (true)
            {
                Console.WriteLine("\nAvailable Actions:");
                for (int i = 0; i < availableActions.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {availableActions[i].Name}");
                }

                Console.Write("\nSelect action (enter number): ");
                if (int.TryParse(Console.ReadLine(), out int choice) &&
                    choice >= 1 &&
                    choice <= availableActions.Count)
                {
                    return availableActions[choice - 1];
                }

                Console.WriteLine("Invalid selection. Please try again.");
            }
        }

        private IPlayerAction GetComputerAction(List<IPlayerAction> availableActions)
        {
            // For now, computer always chooses to attack if possible
            var attackAction = availableActions.FirstOrDefault(a => a is Attack); //LLM generated code. Kommentera/källhänvisa på nåt sätt, idk???
            return attackAction ?? availableActions.First();
        }

        public bool CheckGameOver()
        {
            bool humanShipsDestroyed = _humanPlayer.AreAllShipsSunk();
            bool computerShipsDestroyed = _computerPlayer.AreAllShipsSunk();

            if (humanShipsDestroyed || computerShipsDestroyed)
            {
                _currentPlayer = humanShipsDestroyed ? _computerPlayer : _humanPlayer; // Set winner as current player
                _isGameOver = true;
                return true;
            }

            return false;
        }

        public void SwitchPlayer()
        {
            _currentPlayer = (_currentPlayer == _humanPlayer) ? _computerPlayer : _humanPlayer;
        }

        private void DisplayGameOver() //LLM. Nödväntigt att kommentera?
        {
            Console.Clear();
            Console.WriteLine("\n=== Game Over ===");
            Console.WriteLine($"\nWinner: {_currentPlayer.Name}!");

            // Display final grid states
            Console.WriteLine("\nFinal Grid States:");
            Console.WriteLine("\nPlayer's Grid:");
            DisplayGrid(_humanPlayer.PlayerGrid, true);
            Console.WriteLine("\nComputer's Grid:");
            DisplayGrid(_computerPlayer.PlayerGrid, true);

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
