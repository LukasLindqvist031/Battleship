using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public interface IPlayerAction<T> where T : Player
    {
        void Execute(T player); 
    }
}
