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
        public void Execute(Player player, Cell targetCell) //Redunacy. Kan använda null, men blir fucked
        {
            foreach (var ship in player.Ships)
            {
                if (!ship.IsSunk() && ship.HitTaken > 0)
                {
                    ship.HitTaken--; 
                }
            }
        }
    }
}
