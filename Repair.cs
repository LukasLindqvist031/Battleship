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

        public bool AttemptRepair(Player player)
        {
            foreach (var cell in player.PlayerGrid)
            {
                if (cell.IsHit && cell.HasShip())
                {
                    return true;
                }
            }
            return false;
        }

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

                    // Re-evaluate if the ship was previously marked as sunk
                    if (associatedShip.IsSunk() && !player.Ships.Contains(associatedShip))
                    {
                        player.Ships.Add(associatedShip);
                    }
                }
            }
        }

    }
}