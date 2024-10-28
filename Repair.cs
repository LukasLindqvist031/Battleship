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
            // Return true if there is at least one damaged, non-sunk ship
            return player.Ships.Any(ship => !ship.IsSunk() && ship.HitTaken > 0);
        }

        public void Execute(Player player, Cell targetCell = null)
        {
            // Filter for damaged, non-sunk ships
            var damagedShips = player.Ships
                .Where(ship => !ship.IsSunk() && ship.HitTaken > 0)
                .ToList();

            // Select a random damaged ship to repair
            var random = new Random();
            var shipToRepair = damagedShips[random.Next(damagedShips.Count)];
            shipToRepair.HitTaken--;  // Repair by reducing the HitTaken count by one

            // Find the first damaged cell and reset its IsHit status
            var cellToRepair = shipToRepair.PlacedOnCell.FirstOrDefault(cell => cell.IsHit);
            if (cellToRepair != null)
            {
                cellToRepair.IsHit = false;  // Reset IsHit to allow DisplayGrid to show "~"
                //cellToRepair.Mark = "O ";    // Update the cell mark to "O " for a repaired ship part
            }
        }
    }
}
