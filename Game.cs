using System;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using System.Xml;

namespace SeaBattle
{
    public class Game
    {
        static Random rand = new Random();
        static int gridSize = 10;
        private CellState[,] field = new CellState[gridSize, gridSize];
        private CellState[,] field2 = new CellState[gridSize, gridSize];
        public PlayerTypeSlection? player1;
        public PlayerTypeSlection player2;
        static int cursorX = 0;
        static int cursorY = 0;
        private CellState[,]? currentField;
        public bool[,]? currentHits;
        static bool player1Turn;
        enum KeyCodes
        {
            Up,
            Down,
            Left,
            Right,
            Enter,
            Default
        }
        static KeyCodes keyCodes;
        static KeyCodes lastKeyCode;
        public void Seafield()
        {
            Console.Clear();
            field = WarshipPlacement(field);
            field2 = WarshipPlacement(field2);
        }
        static CellState[,] WarshipPlacement(CellState[,] field)
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
        static void DrawColumnLetters()
        {
            Console.Write("   ");
            int offset = (gridSize - 4) / 2;
            for (int i = 0; i < gridSize; i++)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.SetCursorPosition(offset + i * 4, 0);
                Console.Write((char)('A' + i));
                Console.Write(" ");
            }
            Console.WriteLine();
        }
        public void DrawGridRows()
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
                            DrawingWavesPattern();

                        Cell cell = new Cell(currentField);

                        string cellChar = cell.GetSymbol(x, y);
                        Console.Write(cellChar + " ");
                        Console.ResetColor();
                    }
                    Console.WriteLine();
                    Thread.Sleep(10);
                }
            }
        }
        static void Input()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    keyCodes = KeyCodes.Up;
                    break;
                case ConsoleKey.LeftArrow:
                    keyCodes = KeyCodes.Left;
                    break;
                case ConsoleKey.DownArrow:
                    keyCodes = KeyCodes.Down;
                    break;
                case ConsoleKey.RightArrow:
                    keyCodes = KeyCodes.Right;
                    break;
                case ConsoleKey.Enter:
                    keyCodes = KeyCodes.Enter;
                    break;
                default:
                    keyCodes = KeyCodes.Default;
                    break;
            }
            lastKeyCode = keyCodes;
        }
        private async Task DrawingWavesPattern()
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
                Console.Write(Doublewave +" ");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(Triplewave);
            }
        }
        public void PlayGame()
        {
            int gridSize = field.GetLength(0);
            bool[,] player1Hits = new bool[gridSize, gridSize];
            bool[,] player2Hits = new bool[gridSize, gridSize];
            player1Turn = true;

            Console.WriteLine("Player 1:");
            player1 = new PlayerTypeSlection();

            Console.WriteLine("Player 2:");
            player2 = new PlayerTypeSlection();


            Console.CursorVisible = false;
            while (true)
            {
                Console.Clear();
                DrawColumnLetters();

                currentField = player1Turn ? field2 : field;
                currentHits = player1Turn ? player1Hits : player2Hits;

                DrawGridRows();

                Console.SetCursorPosition(cursorX * 4 + 3, cursorY + 1);
                Console.CursorVisible = true;

                Input();

                Console.CursorVisible = false;

                HandlePlayerTurn();
                if (lastKeyCode == KeyCodes.Enter)
                {
                    HandleCellSelection();
                }
                HandleBarrierChecking();
                CheckingWin();
            }
        }
        private void HandlePlayerTurn()
        {
            if (player1.IsMachine && player1Turn || player2.IsMachine && !player1Turn)
            {
                GenerateRandomCursor();
                HandleCellSelection();
            }
            else
            {
                HandleInput();
            }
        }

        public void GenerateRandomCursor()
        {
            Random random = new Random();
            cursorX = random.Next(0, gridSize);
            cursorY = random.Next(0, gridSize);
        }
        private void HandleCellSelection()
        {
            if (currentHits[cursorX, cursorY])
            {
                Console.WriteLine("You have already fired on this cell.");
            }                         
            else if(currentField[cursorX, cursorY] == CellState.Ship)
            {
               currentHits[cursorX, cursorY] = true;
               Console.WriteLine("You hit a warship!");
               currentField[cursorX, cursorY] = CellState.Hit;
            }
            else
            {
                Console.WriteLine("You missed.");
                player1Turn = !player1Turn;
            }
            Console.ReadLine();
        }
        static void HandleInput()
        {
            if (keyCodes == KeyCodes.Left && cursorX > 0)
            {
                cursorX--;
            }
            else if (keyCodes == KeyCodes.Right)
            {
                cursorX++;
            }
            else if (keyCodes == KeyCodes.Up && cursorY > 0)
            {
                cursorY--;
            }
            else if (keyCodes == KeyCodes.Down)
            {
                cursorY++;
            }
        }
        private void HandleBarrierChecking()
        {
            if (cursorX > gridSize - 1)
            {
                cursorX = 0;
            }
            else if (cursorY > gridSize - 1)
            {
                cursorY = 0;
            }
        }
        private bool AreAllWarshipsSunk()
        {
            for (int y = 0; y < currentField.GetLength(0); y++)
            {
                for (int x = 0; x < currentField.GetLength(1); x++)
                {
                    if (currentField[x, y] == CellState.Ship && !currentHits[x, y])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        private void CheckingWin()
        {
            if (AreAllWarshipsSunk())
            {
                Console.WriteLine(player1Turn ? "Player 1 wins!" : "Player 2 wins!");
                Environment.Exit(0);
            }
        }
    }   
}
