using Battleship;

Grid grid = new Grid();
Ship ship1 = new Ship(3);
Ship ship2 = new Ship(4);
Ship ship3 = new Ship(5);
Ship ship4 = new Ship(1);
Ship ship5 = new Ship(2);

ShipPlacementService shipPlacementService = new ShipPlacementService();
shipPlacementService.PlaceShipRandomly(grid, ship1);
shipPlacementService.PlaceShipRandomly(grid, ship2);
shipPlacementService.PlaceShipRandomly(grid, ship3);
shipPlacementService.PlaceShipRandomly(grid, ship4);
shipPlacementService.PlaceShipRandomly(grid, ship5);


grid.PrintGrid();