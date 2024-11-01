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
            Grid opponentGrid = player.OpponentGrid;

            Cell targetCell = IsValidShoot(opponentGrid);

            Attack attack = new Attack(opponentGrid);
            attack.Execute(player, targetCell);
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
