using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class IntelligentShooting : IShootingStrategy
    {
        private readonly Random _random;
        private List<Cell> _hitCells;  // Lista över träffade celler
        private Queue<Cell> _possibleTargets;  // Möjliga målceller runt ett träffat skepp

        public IntelligentShooting()
        {
            _random = new Random();
            _hitCells = new List<Cell>();
            _possibleTargets = new Queue<Cell>();
        }

        public void Shoot(Player player)
        {
            // Hämta motståndarens grid
            Grid opponentGrid = player.Opponent.Grid;

            // Om vi har målceller att skjuta på (närliggande celler), välj från den kön
            if (_possibleTargets.Count > 0)
            {
                Cell targetCell = _possibleTargets.Dequeue(); // Hämta nästa cell från kön (Dequeue removes and returns the object at the beginning of the Queue<Cell>).

                // Om cellen redan har blivit träffad, välj en ny cell
                if (targetCell.IsHit)
                {
                    Shoot(player); // Skjut igen
                    return;
                }

                ExecuteAttack(player, targetCell);
            }
            else
            {
                // Annars, välj en slumpmässig cell om det inte finns några närliggande mål
                Cell targetCell = IsValidShoot(opponentGrid);
                ExecuteAttack(player, targetCell);
            }
        }

        private void ExecuteAttack(Player player, Cell targetCell)
        {
            Attack attack = new Attack(player.Opponent.Grid, targetCell);
            attack.Execute(player);

            if (!targetCell.IsEmpty() && targetCell.IsHit)  // Om träff på skepp
            {
                _hitCells.Add(targetCell);  // Lägg till i listan med träffar

                // Lägg till möjliga målceller (upp, ner, vänster, höger)
                AddAdjacentCellsToTargets(player.Opponent.Grid, targetCell);
            }
        }

        private void AddAdjacentCellsToTargets(Grid opponentGrid, Cell cell)
        {
            int row = cell.Row;
            int col = cell.Column;

            // Lägg till de 4 närliggande cellerna om de är giltiga skottmål
            if (row > 0 && !opponentGrid.Grids[row - 1, col].IsHit) // Upp
                _possibleTargets.Enqueue(opponentGrid.Grids[row - 1, col]); //(Enqueue adds an object to the end of the Queue<Cell>).

            if (row < Grid.GridSize - 1 && !opponentGrid.Grids[row + 1, col].IsHit) // Ner
                _possibleTargets.Enqueue(opponentGrid.Grids[row + 1, col]);

            if (col > 0 && !opponentGrid.Grids[row, col - 1].IsHit) // Vänster
                _possibleTargets.Enqueue(opponentGrid.Grids[row, col - 1]);

            if (col < Grid.GridSize - 1 && !opponentGrid.Grids[row, col + 1].IsHit) // Höger
                _possibleTargets.Enqueue(opponentGrid.Grids[row, col + 1]);
        }

        // Denna metod returnerar en slumpmässig valid cell (samma som RandomShooting, kan man typ göra en egen class eller nått med denna så man slipper upprepa den?).
        private Cell IsValidShoot(Grid opponentGrid)
        {
            Cell targetCell;
            int row, col;

            do
            {
                row = _random.Next(0, Grid.GridSize);
                col = _random.Next(0, Grid.GridSize);
                targetCell = opponentGrid.Grids[row, col];
            }
            while (targetCell.IsHit);

            return targetCell;
        }
    }
}