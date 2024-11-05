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
        //private Cell _targetCell; //Används aldrig?

        public Attack(Grid opponentGrid)
        {
            _opponentGrid = opponentGrid;
        }

        public void Execute(Player player, Cell targetCell)
        {
            targetCell.IsHit = true;

            if (!targetCell.IsEmpty())
            {
                Ship targetShip = targetCell.Ship;
                if (targetShip != null)
                {
                    targetShip.HitTaken++;
                    targetCell.WasRepaired = false;  // Reset repair status on a new hit to be able to repair the same cell multiple times.

                    if (targetShip.IsSunk())
                    {
                        player.RemoveSunkShip(targetShip);
                    }
                }
            }
        }
    }
}
