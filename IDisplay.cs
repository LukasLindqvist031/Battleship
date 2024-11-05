using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public interface IDisplay
    {
        public void DrawGrid(Grid playerGrid, Grid opponentGrid, bool hideShips, GridNavigator playerNavigator = null, GridNavigator opponentNavigator = null);
    }
}