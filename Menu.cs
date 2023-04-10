using System;
using Figgle;

namespace SeaBattle
{
    public class Menu
    {
        public Menu() 
        {
            DrawMenu();
        }
        public void DrawMenu()
        {
            Console.WriteLine(FiggleFonts.Standard.Render("SeaBattle  \\__/ "));
            Console.WriteLine("Press any key to start");
            Console.ReadKey();
        }
    }
}
