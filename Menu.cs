using System;
using Figgle;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;


namespace SeaBattle
{
    public class Menu
    {
        public void DrawMenu()
        {
            ImageRender imageRender = new ImageRender();
           // imageRender.Render("CatImage.jpg");
            Console.ForegroundColor= ConsoleColor.Blue;
            Console.WriteLine(FiggleFonts.Standard.Render("SeaBattle"));
            Console.WriteLine("Press any key to start");
            Console.ReadKey();
            CleanConsole();
        }     
      
        public void CleanConsole()
        {
            int left = Console.CursorLeft;
            int top = Console.CursorTop;

            for (int y = 0; y < Console.WindowHeight; y++)
            {
                Console.SetCursorPosition(left, y);
                for (int x = 0; x < Console.WindowWidth; x++)
                {
                    Console.Write(" ");
                }
            }
            Console.SetCursorPosition(left, top);
        }
    }
}
