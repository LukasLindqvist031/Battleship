using Battleship;

IDisplay display = new GameDisplay();

GameSetup game = new GameSetup(display);
game.Run();