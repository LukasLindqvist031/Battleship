using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class IntelligentShooting : IShootingStrategy
    {
        private readonly Random _random;
        private List<Cell> _hitCells;
        private readonly Queue<Cell> _possibleTargets;

        public IntelligentShooting()
        {
            _random = new Random();
            _hitCells = new List<Cell>();
            _possibleTargets = new Queue<Cell>();
        }
        public void Shoot(Player player)
        {
            Grid opponentGrid = player.OpponentGrid;

            if (_possibleTargets.Count > 0)
            {
                Cell targetCell = _possibleTargets.Dequeue();

                if (targetCell.IsHit)
                {
                    Shoot(player);
                    return;
                }
                ExecuteAttack(player, targetCell);
            }
            else
            {
                Cell targetCell = IsValidShoot(opponentGrid);
                ExecuteAttack(player, targetCell);
            }
        }
        private void ExecuteAttack(Player player, Cell targetCell)
        {
            Attack attack = new Attack(player.OpponentGrid);
            attack.Execute(player, targetCell);

            if (!targetCell.IsEmpty() && targetCell.IsHit)
            {
                _hitCells.Add(targetCell);
                AddAdjacentCellsToTargets(player.OpponentGrid, targetCell);
            }
        }

        private void AddAdjacentCellsToTargets(Grid opponentGrid, Cell cell)
        {
            int row = cell.Row;
            int col = cell.Column;

            if (row > 0 && !opponentGrid.Grids[row - 1, col].IsHit)
                _possibleTargets.Enqueue(opponentGrid.Grids[row - 1, col]);

            if (row < Grid.GridSize - 1 && !opponentGrid.Grids[row + 1, col].IsHit)
                _possibleTargets.Enqueue(opponentGrid.Grids[row + 1, col]);

            if (col > 0 && !opponentGrid.Grids[row, col - 1].IsHit)
                _possibleTargets.Enqueue(opponentGrid.Grids[row, col - 1]);

            if (col < Grid.GridSize - 1 && !opponentGrid.Grids[row, col + 1].IsHit)
                _possibleTargets.Enqueue(opponentGrid.Grids[row, col + 1]);
        }
        private Cell IsValidShoot(Grid opponentGrid)
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
