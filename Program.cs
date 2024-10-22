using Battleship;

Grid playerGrid = new Grid();
Grid opponentGrid = new Grid();

// 2. Place a ship on the opponent's grid at position (5, 5)
opponentGrid.PlaceTestShip(5, 5);

// 3. Create a Human player with an Attack action
List<IPlayerAction> actions = new List<IPlayerAction>
            {
                new Attack(opponentGrid, opponentGrid.Grids[5, 5]) // Targeting cell (5, 5) where the ship is
            };

IShootingStrategy shootingStrategy = null; // Assuming a strategy is injected (mock or simple one for testing)
Human humanPlayer = new Human("Test Player", playerGrid, new List<Ship>(), actions, shootingStrategy);

// 4. Display opponent grid before the attack
Console.WriteLine("Opponent's Grid Before Attack:");
Grid.DisplayGrid(opponentGrid);

// 5. Human player performs attack action (attack at index 0)
humanPlayer.ChooseAndPerformAction(0);

// 6. Display opponent grid after the attack
Console.WriteLine("Opponent's Grid After Attack:");
Grid.DisplayGrid(opponentGrid);