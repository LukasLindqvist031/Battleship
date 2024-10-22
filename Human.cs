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
            IPlayerAction chosenAction = Actions[actionIndex];

            switch (chosenAction)
            {
                case Attack attackAction:
                    PerformShooting(); 
                    break;

                case Repair repairAction:
                    PerformAction(actionIndex); 
                    break;

                default:
                    throw new InvalidOperationException("Unknown action type!");
            }
        }
    }
}
