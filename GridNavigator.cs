using System;

namespace Battleship
{
    public class GridNavigator
    {
        public int CurrentRow { get; private set; }
        public int CurrentCol { get; private set; }
        private readonly Grid _grid;

        public GridNavigator(Grid grid)
        {
            _grid = grid;
            CurrentRow = 0;
            CurrentCol = 0;
        }

        public void MoveUp()
        {
            if (CurrentRow > 0)
                CurrentRow--;
        }

        public void MoveDown()
        {
            if (CurrentRow < Grid.GridSize - 1)
                CurrentRow++;
        }

        public void MoveLeft()
        {
            if (CurrentCol > 0)
                CurrentCol--;
        }

        public void MoveRight()
        {
            if (CurrentCol < Grid.GridSize - 1)
                CurrentCol++;
        }

        public Cell GetCurrentCell()
        {
            return _grid.Grids[CurrentRow, CurrentCol];
        }
    }

}
