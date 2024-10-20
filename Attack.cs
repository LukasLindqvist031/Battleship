using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class Attack<T> : IPlayerAction<T> where T : Player
    {
        private Grid _opponentGrid;
        private Cell _targetCell;

        public Attack(Grid opponentGrid, Cell targetCell)
        {
            _opponentGrid = opponentGrid;
            _targetCell = targetCell;
        }

        public void Execute(T player)
        {
            if (_targetCell.IsHit)
            {
                Console.WriteLine("This cell has already been hit.");
                return;
            }

            _targetCell.IsHit = true;

            if (!(_targetCell.IsEmpty()))
            {
                Ship targetShip = _targetCell.Ship; //Dependecy Inejction
                targetShip.HitTaken++;
                _targetCell.Mark = "X ";



                if (targetShip.IsSunk()) //För att få vinn vilkoret att fungera måste vi tagit bort skepp som blivit nedskjutna
                {
                    player.RemoveSunkShip(targetShip);
                }
            }
            else { _targetCell.Mark = "M "; }
        }
    }
}
