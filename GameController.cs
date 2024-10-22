using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class GameController(Player player1, Player player2)
    {

        private bool _isPlayer1Turn = true;

        public Player GetCurrentPlayer()
        {
            return _isPlayer1Turn ? player1 : player2;
        }

        public Player GetOpponent(Player currentPlayer)
        {
            return currentPlayer == player1 ? player2 : player1;
        }

        public void SwitchTurn()
        {
            _isPlayer1Turn = !_isPlayer1Turn;
        }

        public Grid GetOpponentGrid(Player currentPlayer)
        {
            return GetOpponent(currentPlayer).PlayerGrid;
        }

        public void ProcessShooting(Player currentPlayer, Cell targetCell)
        {
            Player opponent = GetOpponent(currentPlayer);
            Attack attack = new Attack(opponent.PlayerGrid, targetCell);
            attack.Execute(currentPlayer);

            // Check if the opponent has lost
            if (opponent.AreAllShipsSunk())
            {
                Console.WriteLine($"{currentPlayer.Name} wins!");
            }

            // Switch turn after shooting
            SwitchTurn();
        }
    }

}
