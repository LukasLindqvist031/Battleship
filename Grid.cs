using System;
using System.Collections;
using System.Collections.Generic;

namespace Battleship
{
    // KRAV #4:
    // 1: Iterator Pattern
    // 2: Den anpassade GridEnumerator itererar över alla celler.
    // 3: Detta gör Grid-strukturen kompatibel med LINQ och möjliggör flexibla speloperationer.
    public class Grid : IEnumerable<Cell>
    {
        public const int GridSize = 10;
        public Cell[,] Grids { get; set; }

        public Grid()
        {
            Grids = new Cell[GridSize, GridSize];
            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    Grids[row, col] = new Cell(row, col);
                }
            }
        }
        public IEnumerator<Cell> GetEnumerator()
        {
            return new GridEnumerator(this);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        private class GridEnumerator : IEnumerator<Cell>
        {
            private readonly Grid _grid;
            private int _currentRow;
            private int _currentCol;
            private bool _start;

            public GridEnumerator(Grid grid)
            {
                _grid = grid;
                _currentRow = 0;
                _currentCol = -1;
                _start = true;
            }

            public Cell Current
            {
                get
                {
                    if (_currentCol < 0 || _currentRow >= GridSize)
                        throw new InvalidOperationException();
                    return _grid.Grids[_currentRow, _currentCol];
                }
            }

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                if (_start)
                {
                    _currentCol = 0;
                    _start = false;
                    return true;
                }

                _currentCol++;
                if (_currentCol >= GridSize)
                {
                    _currentCol = 0;
                    _currentRow++;
                }

                return _currentRow < GridSize;
            }
            public void Reset()
            {
                _currentRow = 0;
                _currentCol = -1;
                _start = true;
            }
            public void Dispose()
            {
            }
        }
    }
}