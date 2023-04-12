using System;
using System.Threading.Tasks;

namespace SeaBattle
{
    public class Game
    {
        public Game() 
        {
       
        }
        public void Seafield()
        {
            int gridSize = 10;
            char[,] field = WarshipPlacement(gridSize);

            Console.Clear();
            DrawColumnLetters(gridSize);
            DrawGridRowsAsync(gridSize,field);

            Console.Clear();
            DrawColumnLetters(gridSize);

            Console.ReadLine();
        }

        static char[,] WarshipPlacement(int gridSize)
        {
            char[,] field = new char[gridSize, gridSize];
            Random rand = new Random();
            int shipSize = 3;
            int shipCount = 2;

            while (shipCount > 0)
            {
                int orientation = rand.Next(2); // 0 = horizontal, 1 = vertical
                int startX = rand.Next(0, field.GetLength(0) - shipSize);
                int startY = rand.Next(0, field.GetLength(1) - shipSize);

                bool isValidPlacement = true;
                for (int i = 0; i < shipSize; i++)
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
                    for (int i = 0; i < shipSize; i++)
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
            int offset = (gridSize) / 2;
            for (int i = 0; i < gridSize; i++)
            {
                Console.SetCursorPosition(offset + i * 4, 0);
                Console.Write((char)('A' + i));
                Console.Write(" ");
            }
            Console.WriteLine();
        }
        public async Task DrawGridRowsAsync(int gridSize, char[,] field)
        {
            Random rand = new Random();
            while (true)
            {
                Console.SetCursorPosition(0, 2);
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
                            int wavePattern = rand.Next(3);
                            if (wavePattern == 0)
                            {
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.Write("~   ");
                            }
                            else if (wavePattern == 1)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkCyan;
                                Console.Write("~~~ ");
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.Write("~   ");
                            }
                        }
                        Console.ResetColor();
                    }
                    Console.WriteLine();
                }
                await Task.Delay(500);
            }
        }
    }
    
}
