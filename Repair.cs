using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class Repair<T> : IPlayerAction<T> where T : Player
    {
        public void Execute(T player)
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
