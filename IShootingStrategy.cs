using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    // KRAV #2:
    // 1: Strategy pattern
    // 2: Implementeringar av IShootingStrategy låter datorn använda olika strategier som väljs under körning.
    // 3: Detta ger flexibilitet, då olika spelstilar kan användas genom att ändra strategi utan att behöva ändra datorns logik.
    public interface IShootingStrategy
    {
        void Shoot(Player player);
    }
}