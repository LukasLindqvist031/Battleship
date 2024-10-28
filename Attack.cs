using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class Attack : IPlayerAction
    {
        public string Name { get; } = "Attack";

        private Grid _opponentGrid;
        private Cell _targetCell;

        public Attack(Grid opponentGrid)
        {
            _opponentGrid = opponentGrid;
        }

        public void Execute(Player player, Cell targetCell)
        {
            if (targetCell == null) //Dubellkolla denna, inte säker på att det är bästa lösningen
            {
                Console.WriteLine("Invalid target cell.");
                return;
            }

            if (targetCell.IsHit)
            {
                Console.WriteLine("This cell has already been hit.");
                return;
            }

            targetCell.IsHit = true;

            if (!targetCell.IsEmpty())
            {
                Ship targetShip = targetCell.Ship;
                if (targetShip != null)
                {
                    targetShip.HitTaken++;
                    targetCell.Mark = "X ";

                    if (targetShip.IsSunk())
                    {
                        player.RemoveSunkShip(targetShip);
                    }
                }
            }
            else
            {
                targetCell.Mark = "M ";
            }
        }
    }
}
