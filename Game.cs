using System;
using System.Threading.Tasks;

namespace SeaBattle
{
    public class Game
    {
        static Random rand = new Random();
        static int gridSize = 10;
        static char[,] field = new char[gridSize, gridSize];
        static char[,] field2 = new char[gridSize, gridSize];

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
            PlayGame();
        }

        public async Task SeafieldAsync()
        {

            await DrawGridRowsAsync(gridSize, field, 0, 2);
            
            await DrawGridRowsAsync(gridSize, field2, 40,2);

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
                Console.SetCursorPosition(offset + i * 4, 0);
                Console.Write((char)('A' + i));
                Console.Write(" ");
            }
            Console.WriteLine();
        }
        public async Task DrawGridRowsAsync(int gridSize, char[,] field, int startX, int startY)
        {
            while (true)
            {
                Console.SetCursorPosition(startX, startY);
                for (int y = 0; y < gridSize; y++)
                {
                    Console.Write((y + 1).ToString().PadLeft(2));
                    Console.Write(" ");
                    for (int x = 0; x < gridSize; x++)
                    {
                        if (field[x, y] == 'O')
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
                }
                await Task.Delay(500);
               
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
        private void DrawingWavesPattern()
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

            int cursorX = 0;
            int cursorY = 0;
            Console.CursorVisible = false;

            while (true)
            {
                Console.Clear();
                DrawColumnLetters(gridSize);

                char[,] currentField = player1Turn ? field : field2;
                bool[,] currentHits = player1Turn ? player1Hits : player2Hits;

                for (int y = 0; y < gridSize; y++)
                {
                    Console.Write((y + 1).ToString().PadLeft(2));
                    Console.Write(" ");

                    for (int x = 0; x < gridSize; x++)
                    {
                        char cell = ' ';

                        if (currentHits[x, y])
                        {
                            cell = 'X';
                        }
                        else if (currentField[x, y] != '\0')
                        {
                            cell = '~';
                        }
                        Console.Write(cell + "   ");
                    }

                    Console.WriteLine();
                }

                Console.SetCursorPosition(cursorX * 4 + 3, cursorY + 1);
                Console.CursorVisible = true;

                Input();

                Console.CursorVisible = false;

                if(keyCodes == KeyCodes.Enter)
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
                                return;
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
                HandleInput(ref cursorX,ref cursorY,gridSize);
            }
        }
        static void HandleInput(ref int cursorX, ref int cursorY, int gridSize)
        {
            if (keyCodes == KeyCodes.Left && cursorX > 0)
            {
                cursorX--;
            }
            else if (keyCodes == KeyCodes.Right && cursorX < gridSize - 1)
            {
                cursorX++;
            }
            else if (keyCodes == KeyCodes.Up && cursorY > 0)
            {
                cursorY--;
            }
            else if (keyCodes == KeyCodes.Down && cursorY < gridSize - 1)
            {
                cursorY++;
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
