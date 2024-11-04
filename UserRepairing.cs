using System;
using System.Linq;

namespace Battleship
{
    public class UserRepairing
    {
        public bool HandleRepair(Player humanPlayer)
        {
            var damagedCells = humanPlayer.PlayerGrid.Grids
                .Cast<Cell>()
                .Where(cell => cell.IsHit && cell.Ship != null && !cell.Ship.IsSunk())
                .ToList();

            if (!damagedCells.Any())
            {
                int messageLineY = Console.CursorTop + 1;
                Console.SetCursorPosition(0, messageLineY);
                TextPresentation.WriteCenteredText("No damaged ships to repair. Choose another action.", 24);
                return false;
            }

            while (true)
            {
                var targetSelector = new TargetSelector(humanPlayer.PlayerGrid);
                Cell targetCell = targetSelector.GetValidTarget(forRepair: true);

                if (damagedCells.Contains(targetCell))
                {
                    var repairAction = humanPlayer.Actions.OfType<Repair>().FirstOrDefault();
                    if (repairAction != null)
                    {
                        repairAction.Execute(humanPlayer, targetCell);
                        Console.WriteLine($"Repaired ship at ({targetCell.Row}, {targetCell.Column}).");
                        return true;
                    }
                }
                else
                {
                    Console.WriteLine("The selected cell is not damaged. Please choose a damaged cell.");
                }
            }
        }
    }
}
