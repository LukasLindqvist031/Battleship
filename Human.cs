using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class Human : Player
    {
        public Human(string name, Grid grid, List<Ship> ships, List<IPlayerAction> actions, IShootingStrategy shootingStrategy)
            : base(name, grid, ships, actions, shootingStrategy) { }
        public void ChooseAndPerformAction(int actionIndex)
        {
            // If the action is shooting, use PerformShooting instead of a generic action
            if (actionIndex == 0) // Assuming index 0 is for shooting
            {
                PerformShooting(); // Executes the shooting strategy
            }
            else
            {
                PerformAction(actionIndex); // Other actions (e.g., repair, etc.)
            }
        }
    }
}
