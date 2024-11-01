using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    // KRAV #3:
    // 1: Bridge Pattern
    // 2: Player-abstraktionen samarbetar med IShootingStrategy-abstraktionen, och spelet sätter ihop dessa typer för att skapa olika spelarbeteenden.
    // 3: Bridge Pattern separerar hierarkierna Player och ShootingStrategy, vilket främjar modularitet och olika strategier.
    public abstract class Player 
    {
        public string Name { get; }
        public Grid PlayerGrid { get; }
        public Grid OpponentGrid { get; }
        public List<Ship> Ships { get; }
        public List<IPlayerAction> Actions { get; }
        public IShootingStrategy ShootingStrategy { get; }


        protected Player(
        string name,
        Grid playerGrid,
        Grid opponentGrid,
        List<Ship> ships,
        List<IPlayerAction> actions,
        IShootingStrategy shootingStrategy
        )
        {
            Name = name;
            PlayerGrid = playerGrid;
            OpponentGrid = opponentGrid;
            Ships = ships;
            Actions = new List<IPlayerAction> { new Attack(opponentGrid), new Repair() };
            ShootingStrategy = shootingStrategy;
            
        }
        public void RemoveSunkShip(Ship ship)
        {
            if (Ships.Contains(ship)) {  Ships.Remove(ship); }
        }

        public bool AreAllShipsSunk()
        {
            return !PlayerGrid.Any(cell => cell.HasShip() && !cell.IsHit);
        }
    }
}
