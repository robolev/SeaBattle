using SeaBattle.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle
{
    internal class ImageRender
    {
        static string InitPath = Directory.GetCurrentDirectory() + "\\Resources\\";
        public void Render(string ImageName) 
        {            
            string imagePath = InitPath + ImageName;
            using (Image<Rgba32> image = Image.Load<Rgba32>(imagePath))
            {
                int consoleWidth = Console.WindowWidth;
                int consoleHeight = Console.WindowHeight;

                // resize the image to fit the console window size
                image.Mutate(x => x.Resize(consoleWidth, consoleHeight));

                for (int y = 0; y < image.Height; y++)
                {
                    for (int x = 0; x < image.Width; x++)
                    {
                        Rgba32 pixel = image[x, y];
                        ConsoleColor color = GetClosestConsoleColor(pixel.R, pixel.G, pixel.B);
                        Console.BackgroundColor = color;
                        Console.Write(" ");
                    }
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine();
                }
            }

        }
        static ConsoleColor GetClosestConsoleColor(byte r, byte g, byte b)
        {
            ConsoleColor closestColor = ConsoleColor.Black;
            int closestDistance = int.MaxValue;

            foreach (ConsoleColor consoleColor in Enum.GetValues(typeof(ConsoleColor)))
            {
                if (consoleColor == ConsoleColor.Black || consoleColor == ConsoleColor.White)
                    continue;

                ConsoleColor color = consoleColor;
                if (consoleColor >= ConsoleColor.DarkGray)
                {
                    color -= 8;
                }

                int distance = ColorDistance(r, g, b, ConsoleColorToRgb(color));
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestColor = consoleColor;
                }
            }

            return closestColor;
        }

        static int ColorDistance(byte r1, byte g1, byte b1, (byte r2, byte g2, byte b2) color2)
        {
            int rd = r1 - color2.r2;
            int gd = g1 - color2.g2;
            int bd = b1 - color2.b2;
            return rd * rd + gd * gd + bd * bd;
        }

        static (byte r, byte g, byte b) ConsoleColorToRgb(ConsoleColor color)
        {
            switch (color)
            {
                case ConsoleColor.Black:
                    return (0, 0, 0);
                case ConsoleColor.DarkBlue:
                    return (0, 0, 128);
                case ConsoleColor.DarkGreen:
                    return (0, 128, 0);
                case ConsoleColor.DarkCyan:
                    return (0, 128, 128);
                case ConsoleColor.DarkRed:
                    return (128, 0, 0);
                case ConsoleColor.DarkMagenta:
                    return (128, 0, 128);
                case ConsoleColor.DarkYellow:
                    return (128, 128, 0);
                case ConsoleColor.Gray:
                    return (192, 192, 192);
                case ConsoleColor.DarkGray:
                    return (128, 128, 128);
                case ConsoleColor.Blue:
                    return (0, 0, 255);
                case ConsoleColor.Green:
                    return (0, 255, 0);
                case ConsoleColor.Cyan:
                    return (0, 255, 255);
                case ConsoleColor.Red:
                    return (255, 0, 0);
                case ConsoleColor.Magenta:
                    return (255, 0, 255);
                case ConsoleColor.Yellow:
                    return (255, 255, 0);
                case ConsoleColor.White:
                    return (255, 255, 255);
                default:
                    throw new ArgumentException($"Unknown ConsoleColor value: {color}", nameof(color));
            }
        }
    }
}
