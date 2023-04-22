using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle
{
    public class PlayerTypeSlection
    {
        public bool IsMachine { get; set; }
        public bool[,] Hits { get; set; }

        public PlayerTypeSlection()
        {
            ChoosePlayerType();
        }
        public void ChoosePlayerType()
        {
            Console.WriteLine("Choose player type for this player:");
            Console.WriteLine("1. Human");
            Console.WriteLine("2. Machine");
            Console.Write("Enter choice (1/2): ");
            string choice = Console.ReadLine();
            if (choice == "2")
            {
                IsMachine = true;
            }
            else
            {
                IsMachine = false;
            }
        }
    }
}
