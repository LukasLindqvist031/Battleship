using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public interface IPlayerAction
    {
        string Name { get; }
        void Execute(Player player, Cell targetCell);
    }
}