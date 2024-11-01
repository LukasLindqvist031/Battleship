using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    using System;
    using System.Linq;

    public class Repair : IPlayerAction
    {
        public string Name { get; } = "Repair";

        public bool AttemptRepair(Player player)
        {
            // Check PlayerGrid for any cells with IsHit and an associated Ship
            foreach (var cell in player.PlayerGrid)
            {
                if (cell.IsHit && cell.HasShip())
                {
                    return true;  // A damaged cell exists, so allow repair
                }
            }

            // If no damaged cells are found, return false
            return false;
        }

        public void Execute(Player player, Cell targetCell)
        {
            if (targetCell != null && targetCell.IsHit)
            {
                targetCell.IsHit = false;  // Reset the hit status to indicate repair
                targetCell.WasRepaired = true;

                // Locate the ship associated with this cell and adjust its HitTaken count
                Ship associatedShip = targetCell.Ship;
                if (associatedShip != null && associatedShip.HitTaken > 0)
                {
                    associatedShip.HitTaken--; // Decrement HitTaken to account for the repair
                }
            }
        }

    }
}
