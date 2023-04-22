using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle
{
    public class Lobby
    {
        bool retry = false;
        public void ChooseIfcontinue()
        {
            Console.WriteLine("Do you want to play again");
            Console.WriteLine("1. Yes");
            Console.WriteLine("2. No");
            Console.Write("Enter choice (1/2): ");
            string choice = Console.ReadLine();
            if (choice == "2")
            {
                retry = false;
            }
            else
            {
                retry = true;  
            }
        }
        public void RepeatTheGame() 
        {
            if(retry) 
            {
                Game game = new Game();
                game.Seafield();
                game.PlayGame();

            }            
            else
            {
                Environment.Exit(0);
            }
        
        }
    }
}
