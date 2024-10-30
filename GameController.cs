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
        private readonly IDisplay _display;
        private Player _currentPlayer;
        private bool _isGameOver;

        public GameController(Player humanPlayer, Player computerPlayer, IDisplay display)
        {
            _humanPlayer = humanPlayer;
            _computerPlayer = computerPlayer;
            _display = display;
            _currentPlayer = humanPlayer;
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
    }
}
