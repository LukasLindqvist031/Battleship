using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class Attack : IPlayerAction
    {
        public string Name { get; } = "Attack";

        private Grid _opponentGrid;
        private Cell _targetCell;

        public Attack(Grid opponentGrid)
        {
            _opponentGrid = opponentGrid;
        }

        public void Execute(Player player, Cell targetCell)
        {
            if (targetCell.IsHit)
            {
                Console.WriteLine("This cell has already been hit."); //Gör vi så att man får välja en ny cell här eller vad händer?
                return;
            }

            targetCell.IsHit = true;

            if (!(targetCell.IsEmpty()))
            {
                Ship targetShip = _targetCell.Ship; //Dependecy Inejction
                targetShip.HitTaken++;
                targetCell.Mark = "X ";



                if (targetShip.IsSunk()) //För att få vinn vilkoret att fungera måste vi tagit bort skepp som blivit nedskjutna
                {
                    player.RemoveSunkShip(targetShip);
                }
            }
            else { targetCell.Mark = "M "; }
        }
    }
}
