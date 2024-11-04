using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class UserShooting : IShootingStrategy
    {
        public void Shoot(Player player)
        {
            var targetSelector = new TargetSelector(player.OpponentGrid);
            Cell targetCell = targetSelector.GetValidTarget(forRepair: false);

            if (targetCell != null)
            {
                Attack attack = new Attack(player.OpponentGrid);
                attack.Execute(player, targetCell);

                if (targetCell.HasShip())
                {
                    Console.WriteLine("Hit! You've struck an enemy ship!");
                    if (targetCell.Ship.IsSunk())
                    {
                        Console.WriteLine($"You've sunk a ship of length {targetCell.Ship.Length}!");
                    }
                }
                else
                {
                    Console.WriteLine("Miss! You hit the water.");
                }
                System.Threading.Thread.Sleep(1500);
            }
        }
    }

}
