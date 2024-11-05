using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class Repair : IPlayerAction
    {
        public string Name { get; } = "Repair";

        public void Execute(Player player, Cell targetCell)
        {
            if (targetCell != null && targetCell.IsHit)
            {
                targetCell.IsHit = false;
                targetCell.WasRepaired = true;
                Ship associatedShip = targetCell.Ship;

                if (associatedShip != null)
                {
                    associatedShip.HitTaken = Math.Max(0, associatedShip.HitTaken - 1);

                    if (associatedShip.IsSunk() && !player.Ships.Contains(associatedShip))
                    {
                        player.Ships.Add(associatedShip);
                    }
                }
            }
        }

    }
}
