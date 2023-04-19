using System;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using System.Xml;

namespace SeaBattle
{
    public class Game
    {
        static Random rand = new Random();
        static int gridSize = 10;
        static char[,] field = new char[gridSize, gridSize];
        static char[,] field2 = new char[gridSize, gridSize];
        public Player player1;
        public Player player2;

        enum KeyCodes
        {
            Up,
            Down,
            Left,
            Right,
            Enter
        }
        static KeyCodes keyCodes;
        public void Seafield()
        {
            Console.Clear();
            DrawColumnLetters(gridSize);
            Console.ReadLine();
            field = WarshipPlacement(gridSize,field);
            field2 = WarshipPlacement(gridSize,field2);
        }
        static char[,] WarshipPlacement(int gridSize, char[,] field)
        {          
            int[] shipSizes = { 2, 3, 3, 4, 5 };
            int shipCount = shipSizes.Length;

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

                    if (field[x, y] != '\0')
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
                        field[x, y] = 'O';
                    }

                    shipCount--;
                }
            }

            return field;
        }
        static void DrawColumnLetters(int gridSize)
        {
            Console.Write("   ");
            int offset = (gridSize -4) / 2;
            for (int i = 0; i < gridSize; i++)
            {
                Console.ForegroundColor= ConsoleColor.Cyan;
                Console.SetCursorPosition(offset + i * 4, 0);
                Console.Write((char)('A' + i));
                Console.Write(" ");
            }
            Console.WriteLine();
        }
        public void DrawGridRows(int gridSize, char[,] field, bool[,] currentHits, int cursorX, int cursorY)
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

                        if (currentHits[x, y])
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write("x   ");
                        }
                        else if (field[x, y] == 'O')
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("O   ");
                        }
                        else
                        {
                            DrawingWavesPattern();
                        }

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
            }
        }
        private async Task DrawingWavesPattern()
        {
            int wavePattern = rand.Next(3);
            if (wavePattern == 0)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("~   ");
            }
            else if (wavePattern == 1)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("~~  ");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("~~~ ");
            }
        }
        public void PlayGame()
        {
            int gridSize = field.GetLength(0);
            bool[,] player1Hits = new bool[gridSize, gridSize];
            bool[,] player2Hits = new bool[gridSize, gridSize];
            bool player1Turn = true;
                              
            Console.WriteLine("Player 1:");
            player1 = new Player();

            Console.WriteLine("Player 2:");
            player2 = new Player();

            int cursorX = 0;
            int cursorY = 0;
            Console.CursorVisible = false;
            while (true)
            {
                Console.Clear();
                DrawColumnLetters(gridSize);
 
                char[,] currentField = player1Turn ? field2 : field;
                bool[,] currentHits = player1Turn ? player1Hits : player2Hits;

                DrawGridRows(gridSize, currentField,currentHits,cursorX,cursorY);

                Console.SetCursorPosition(cursorX * 4 + 3, cursorY + 1);
                Console.CursorVisible = true;

                Input();

                Console.CursorVisible = false;
                
                HandlePlayerTurn(ref cursorX, ref cursorY, gridSize, currentHits, currentField,ref player1Turn);
                HandleInput(ref cursorX, ref cursorY);
                CheckKeyPress(ref player1Turn, currentHits, currentField, cursorX, cursorY);
                HandleBarrierChecking(ref cursorX, ref cursorY, gridSize);
            }
        }
        private void HandlePlayerTurn(ref int cursorX, ref int cursorY, int gridSize, bool[,] currentHits, char[,] currentField, ref bool player1Turn)
        {
            if (player1.IsMachine && player1Turn || player2.IsMachine && !player1Turn)
            {
                GenerateRandomCursor(ref cursorX, ref cursorY, gridSize);
                HandleCellSelection(currentHits, currentField, cursorX, cursorY, ref player1Turn);
            }            
        }
        private void CheckKeyPress(ref bool player1Turn, bool[,] currentHits, char[,] currentField, int cursorX, int cursorY)
        {   
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if (keyInfo.Key == ConsoleKey.Enter)
            {
               HandleCellSelection(currentHits, currentField, cursorX, cursorY, ref player1Turn);
            }          
        }
        public void GenerateRandomCursor(ref int cursorX, ref int cursorY, int gridSize)
        {
            Random random = new Random();
            cursorX = random.Next(0, gridSize);
            cursorY = random.Next(0, gridSize);
        }
        private void HandleCellSelection(bool[,] currentHits, char[,] currentField, int cursorX, int cursorY, ref bool player1Turn)
        {
            if (currentHits[cursorX, cursorY])
            {
                Console.WriteLine("You have already fired on this cell.");
                Console.ReadLine();
            }
            else
            {
                currentHits[cursorX, cursorY] = true;

                if (currentField[cursorX, cursorY] != '\0')
                {
                    Console.WriteLine("You hit a warship!");
                    if (AreAllWarshipsSunk(currentField, currentHits))
                    {
                        Console.WriteLine(player1Turn ? "Player 1 wins!" : "Player 2 wins!");
                        Console.ReadLine();
                        Environment.Exit(0);
                    }
                }
                else
                {
                    Console.WriteLine("You missed.");
                    player1Turn = !player1Turn;
                }
            }

            Console.ReadLine();
        }
        static void HandleInput(ref int cursorX, ref int cursorY)
        {
            if (keyCodes == KeyCodes.Left && cursorX > 0)
            {
                cursorX--;
            }
            else if (keyCodes == KeyCodes.Right && cursorX < Console.WindowWidth - 1)
            {
                cursorX++;
            }
            else if (keyCodes == KeyCodes.Up && cursorY > 0)
            {
                cursorY--;
            }
            else if (keyCodes == KeyCodes.Down && cursorY < Console.WindowHeight - 1)
            {
                cursorY++;
            }
        }
        private void HandleBarrierChecking(ref int  cursorX,ref int cursorY,int gridSize)
        {
            if(cursorX > gridSize - 1)
            {
                cursorX = 0;
            }
            else if(cursorY > gridSize - 1)
            {
                cursorY = 0;
            }
        }
        private bool AreAllWarshipsSunk(char[,] field, bool[,] hits)
        {
            for (int y = 0; y < field.GetLength(0); y++)
            {
                for (int x = 0; x < field.GetLength(1); x++)
                {
                    if (field[x, y] == 'O' && !hits[x, y])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
     
    }
    
}
