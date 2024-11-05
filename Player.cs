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
    public class Player
    {
        public string Name { get; }
        public Grid PlayerGrid { get; }
        public Grid OpponentGrid { get; }
        public List<Ship> Ships { get; }
        public List<IPlayerAction> Actions { get; }
        public IShootingStrategy ShootingStrategy { get; }
        public GridNavigator PlayerGridNavigator { get; }
        public GridNavigator OpponentGridNavigator { get; }

        public Player(string name, Grid playerGrid, Grid opponentGrid, List<Ship> ships, List<IPlayerAction> actions, IShootingStrategy shootingStrategy)
        {
            Name = name;
            PlayerGrid = playerGrid;
            OpponentGrid = opponentGrid;
            Ships = ships;
            Actions = actions;
            ShootingStrategy = shootingStrategy;
            PlayerGridNavigator = new GridNavigator(playerGrid);
            OpponentGridNavigator = new GridNavigator(opponentGrid);
        }

        public bool AreAllShipsSunk()
        {
            return Ships.All(ship => ship.IsSunk());
        }

        public void RemoveSunkShip(Ship ship)
        {
            Ships.Remove(ship);
        }

        // Any other methods that were in the original Player class...
    }
}