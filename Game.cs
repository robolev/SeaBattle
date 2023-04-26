using System;

namespace SeaBattle
{
    public class Game
    {      
        static int gridSize = 10;
        static int cursorX = 0;
        static int cursorY = 0;

        private CellState[,] field = new CellState[gridSize, gridSize];
        private CellState[,] field2 = new CellState[gridSize, gridSize];

        public PlayerTypeSlection? player1;
        public PlayerTypeSlection? player2;
               
        static bool player1Turn;
        static bool end = false;

        public bool[,]? currentHits;
        bool[,] player1Hits = new bool[gridSize, gridSize];
        bool[,] player2Hits = new bool[gridSize, gridSize];

        Map cell = new Map();
        
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
            field = Map.WarshipPlacement(field);
            field2 = Map.WarshipPlacement(field2);
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
 
        public void PlayGame()
        {
            Init();
            while (!end)
            {
                Console.Clear();
                DrawColumnLetters();

                CellState[,] currentField = player1Turn ? field2 : field;

                cell.SetCurrentField(currentField);
                currentHits = player1Turn ? player1Hits : player2Hits;

                cell.DrawGridRows(cursorX,cursorY,gridSize);

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
        public void Init()
        {
            end = false;
            int gridSize = field.GetLength(0);
            bool[,] player1Hits = new bool[gridSize, gridSize];
            bool[,] player2Hits = new bool[gridSize, gridSize];
            player1Turn = true;

            Console.WriteLine("Player 1:");
            player1 = new PlayerTypeSlection();

            Console.WriteLine("Player 2:");
            player2 = new PlayerTypeSlection();

            Console.CursorVisible = false;
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
            CellState[,] field = cell.GetCurrentfield();
           
            if (currentHits[cursorX, cursorY])
            {
                Console.WriteLine("You have already fired on this cell.");
            }           
            else if (field[cursorX, cursorY] == CellState.Ship)
            {
               currentHits[cursorX, cursorY] = true;
               Console.WriteLine("You hit a warship!");
               field[cursorX, cursorY] = CellState.Hit;
            }
            else
            {
                Console.WriteLine("You missed.");
                player1Turn = !player1Turn;
                field[cursorX, cursorY] = CellState.Hit;
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

            CellState[,] field = cell.GetCurrentfield();
            for (int y = 0; y < field.GetLength(0); y++)
            {
                for (int x = 0; x < field.GetLength(1); x++)
                {
                    if (field[x, y] == CellState.Ship && !currentHits[x, y])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public bool  CheckingWin()
        {
            bool player1Win = false;
            if (AreAllWarshipsSunk())
            {
                player1Win = player1Turn ? true : false;
                Console.WriteLine(player1Turn ? "Player 1 wins!" : "Player 2 wins!");
                Console.ReadKey();
                end = true;                
            }
            return player1Win;            
        }
    }   
}
