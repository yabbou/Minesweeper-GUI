using System;
using System.Drawing;

namespace Model
{
    public enum MoveType
    {
        Dig, Flag
    }

    public class Cell
    {
        public bool IsFlagged { get; set; }
        public bool HasBomb { get; internal set; }
        public bool HasBeenDug { get; set; }
    }

    public class MinesweeperModel
    {
        private Cell[,] mBoard;

        private int RowCount { get; set; }
        private int ColCount { get; set; }
        private double Percent { get; set; }
        public string GameStatus { get; set; }

        private int TotalBombs { get; set; }
        private int numOfUntouchedCells;

        public MinesweeperModel(int rowCount, int colCount, double percent)
        {
            RowCount = rowCount;
            ColCount = colCount;
            Percent = percent;
            ResetGameStatus();

            mBoard = new Cell[rowCount, colCount];

            TotalBombs = Convert.ToInt32((rowCount * colCount) * percent);
            numOfUntouchedCells = (rowCount * colCount) - TotalBombs;
            DistributeBombsInBoard();
        }

        private void DistributeBombsInBoard()
        {
            int bombRemainingToDistribute = TotalBombs;
            int percentOfLikeliness_OneSeventh = 7;
            Random rand = new Random();

            for (int row = 0; row < RowCount; row++)
            {
                for (int col = 0; col < ColCount; col++)
                {
                    mBoard[row, col] = new Cell();
                    if (bombRemainingToDistribute > 0 && rand.Next(percentOfLikeliness_OneSeventh) == 0)
                    {
                        mBoard[row, col].HasBomb = true; bombRemainingToDistribute--;
                    }
                }
            }
        }

        public string MakeMove(MoveType type, Point location)
        {
            switch (type)
            {
                case MoveType.Dig:
                    int row = location.Y, col = location.X;
                    mBoard[row, col].HasBeenDug = true;
                    return CheckSurroundingCellsForBombs(row, col);
                case MoveType.Flag:
                    return SetFlag(location);
                default:
                    throw new ArgumentException("User must either dig or flag. Move type not recognized.");
            }
        }

        private string CheckSurroundingCellsForBombs(int row, int col)
        {
            int numOfSurroundingBombs = 0;

            if (mBoard[row, col].IsFlagged) { return "FLAG"; }

            if (mBoard[row, col].HasBomb) { GameStatus = "Nice Try"; return "BOMB!"; }

            for (int r = Math.Max(0, row - 1); r <= Math.Min(row + 1, RowCount - 1); r++)
            {
                for (int c = Math.Max(0, col - 1); c <= Math.Min(col + 1, ColCount - 1); c++)
                {
                    if (mBoard[r, c].HasBomb) { numOfSurroundingBombs++; }
                }
            }

            numOfUntouchedCells--;
            return (numOfSurroundingBombs == 0) ? "0" : numOfSurroundingBombs.ToString();
        }

        private string SetFlag(Point location)
        {
            int row = location.Y;
            int col = location.X;
            var isFlagged = ToggleFlag(mBoard[row, col]);
            return isFlagged ? "FLAG" : "";
        }

        private bool ToggleFlag(Cell cell)
        {
            cell.IsFlagged = !cell.IsFlagged;
            return cell.IsFlagged;
        }

        public bool IsGameOver()
        {
            if (numOfUntouchedCells == 0) { GameStatus = "You  won!"; }
            return !GameStatus.Equals("In progress...");
        }

        public void ResetInternalVariables()
        {
            ResetBoard();
            ResetMoveCounter();
            ResetGameStatus();
        }

        public void ResetBoard()
        {
            DistributeBombsInBoard();
        }

        public void ResetMoveCounter()
        {
            numOfUntouchedCells = (ColCount * RowCount) - TotalBombs;
        }

        private void ResetGameStatus()
        {
            GameStatus = "In progress...";
        }

        public Cell GetBoardCell(int row, int col)
        {
            return mBoard[row, col];
        }
    }
}

