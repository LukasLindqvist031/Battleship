using Battleship;

Grid grid = new Grid();
grid.PlaceTestShip(1, 2);
grid.ShootTest(1, 2);
grid.ShootTest(1, 3);
grid.ShootTest(1, 1);
grid.PlaceTestShip(4, 4);
grid.PrintGrid();


Grid grid2 = new Grid();
grid2.PrintGrid();