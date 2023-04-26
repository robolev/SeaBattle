using System;

namespace SeaBattle
{
    public enum CellState
    {
        Empty,
        Ship,
        Hit,
        Wave,
        DoubleWave,
        TripleWave,
    }

    public class Map
    {
        private CellState[,]? currentField;

        static Random rand = new Random();
               
        public string GetSymbol(int x, int y)
        {
            CellState cellState = currentField[x, y];
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
        public void SetCurrentField(CellState[,] field)
        {
            currentField = field;
        }
        public CellState[,] GetCurrentfield()
        {
            return currentField;          
        }
        public static CellState[,] WarshipPlacement(CellState[,] field)
        {
            int[] shipSizes = { 2, 3, 3, 4, 5 };
            int shipCount = shipSizes.Length;
            Random rand = new Random();

            while (shipCount > 0)
            {
                int orientation = rand.Next(2); // 0 = horizontal, 1 = vertical
                int startX = rand.Next(0, field.GetLength(0) - shipSizes[shipCount - 1]);
                int startY = rand.Next(0, field.GetLength(1) - shipSizes[shipCount - 1]);

                bool isValidPlacement = true;
                for (int i = 0; i < shipSizes[shipCount - 1]; i++)
                {
                    int x = startX + (orientation == 0 ? i : 0);
                    int y = startY + (orientation == 1 ? i : 0);

                    if (field[x, y] != CellState.Empty)
                    {
                        isValidPlacement = false;
                        break;
                    }
                }

                if (isValidPlacement)
                {
                    for (int i = 0; i < shipSizes[shipCount - 1]; i++)
                    {
                        int x = startX + (orientation == 0 ? i : 0);
                        int y = startY + (orientation == 1 ? i : 0);
                        field[x, y] = CellState.Ship;
                    }
                    shipCount--;
                }
            }
            return field;
        }
        public void DrawGridRows(int cursorX,int cursorY,int gridSize)
        {
            while (!Console.KeyAvailable)
            {
                Console.SetCursorPosition(0, 1);
                for (int y = 0; y < gridSize; y++)
                {
                    Console.Write((y + 1).ToString().PadLeft(2));
                    Console.Write(" ");
                    for (int x = 0; x < gridSize; x++)
                    {
                        if (x == cursorX && y == cursorY)
                        {
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Blue;
                        }
                        if (currentField[x, y] == CellState.Empty)
                            DrawingWavesPattern(x,y);

                        string cellChar = GetSymbol(x,y);
                        Console.Write(cellChar + " ");
                        Console.ResetColor();
                    }
                    Console.WriteLine();
                    Thread.Sleep(10);
                }
            }
        }
        private async Task DrawingWavesPattern(int x ,int y)
        {
            int wavePattern = rand.Next(3);
            string wave = "~";
            string Doublewave = "~~";
            string Triplewave = "~~~";

            if (wavePattern == 0)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(wave + "  ");
            }
            else if (wavePattern == 1)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write(Doublewave + " ");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(Triplewave);
            }
        }

    }
}
