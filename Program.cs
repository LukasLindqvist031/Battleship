using Battleship;

Grid grid = new Grid();
Ship ship1 = new Ship(1);
Ship ship2 = new Ship(1);
Ship ship3 = new Ship(1);
Ship ship4 = new Ship(1);
Ship ship5 = new Ship(2);
Ship ship6 = new Ship(2);
Ship ship7 = new Ship(2);
Ship ship8 = new Ship(3);
Ship ship9 = new Ship(3);
Ship ship10 = new Ship(4);


ShipPlacementService shipPlacementService = new ShipPlacementService();
shipPlacementService.PlaceShipRandomly(grid, ship1);
shipPlacementService.PlaceShipRandomly(grid, ship2);
shipPlacementService.PlaceShipRandomly(grid, ship3);
shipPlacementService.PlaceShipRandomly(grid, ship4);
shipPlacementService.PlaceShipRandomly(grid, ship5);
shipPlacementService.PlaceShipRandomly(grid, ship6);
shipPlacementService.PlaceShipRandomly(grid, ship7);
shipPlacementService.PlaceShipRandomly(grid, ship8);
shipPlacementService.PlaceShipRandomly(grid, ship9);
shipPlacementService.PlaceShipRandomly(grid, ship10);


grid.PrintGrid();