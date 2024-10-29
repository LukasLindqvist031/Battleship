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

                // Find the ship that includes this cell and decrement its HitTaken
                foreach (var ship in player.Ships)
                {
                    if (ship.PlacedOnCell.Contains(targetCell))
                    {
                        ship.HitTaken--;  // Decrement HitTaken for the specific ship
                        targetCell.WasRepaired = true; //Mark as repaired
                        break;  // Stop after finding the correct ship
                    }
                }
            }
        }
    }
}
