using System;

namespace SeaBattle
{
    public enum CellState
    {
        Empty,
        Ship,
        Hit,
    }

    public class Cell
    {
        private CellState[,] field;
        public Cell(CellState[,] field)
        {
            this.field = field;
        }

        public string GetSymbol(int x, int y)
        {
            CellState cellState = field[x, y];
            string symbol;
            string ship = "O  ";
            string hit = "x  ";


            switch (cellState)
            {
                case CellState.Ship:
                    symbol = ship;
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case CellState.Hit:
                    symbol = hit;
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;             
                default:
                    symbol = "";
                    break;
            }
            return symbol;
        }
    }
}
