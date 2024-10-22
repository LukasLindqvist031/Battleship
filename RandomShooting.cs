using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class RandomShooting : IShootingStrategy
    {
        private readonly Random _random;

        public RandomShooting()
        {
            _random = new Random();
        }

        public void Shoot(Player player)
        {
            // Get the opponent's grid
            Grid opponentGrid = player.Opponent.Grid; // Assuming Player has an Opponent reference and Grid

            // Find a valid cell to shoot at
            Cell targetCell = IsValidShoot(opponentGrid);

            // Use Attack class to perform the shooting action
            Attack attack = new Attack(opponentGrid, targetCell);
            attack.Execute(player);
        }

        public Cell IsValidShoot(Grid opponentGrid)
        {
            Cell targetCell;
            int row, col;

            do
            {
                row = _random.Next(0, Grid.GridSize);
                col = _random.Next(0, Grid.GridSize);
                targetCell = opponentGrid.Grids[row, col];
            }
            while (targetCell.IsHit);

            return targetCell;
        }
    }
}
