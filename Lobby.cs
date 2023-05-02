using System;
using System.Diagnostics.Contracts;

namespace SeaBattle
{
    public class Lobby
    {
        Game game = new Game();

        CustodianOfInformation custodian = new CustodianOfInformation();
        Profile profile= new Profile();

        bool winner;
        public void ChooseIfcontinue()
        {
            profile.AssingPrewiousInformation(profile.player1WinCount,profile.player2WinCount);
            while (true)
            {
                DrawStatistics();
                Chose();
            }
        }
        public void CreateNewGame()
        {
            game = new Game();
            game.PlayGame();
        }   
        public void DrawStatistics()
        {
            int winCount1;
            int winCount2;
            winner = game.CheckingWin();
            profile.PLayersWinStatistics(winner);
            (winCount1, winCount2) =profile.CalculateStatistic();
            Console.WriteLine($"Count1: {winCount1} --- Count2: {winCount2}");
        }
        public void Chose()
        {
            Console.WriteLine("Do you want to play again");
            Console.WriteLine("1. Yes");
            Console.WriteLine("2. No");
            Console.Write("Enter choice (1/2): ");
            string choice = Console.ReadLine();
            if (choice == "1")
                CreateNewGame();
            else
            {
                profile.SaveCount(profile.player1WinCount, profile.player2WinCount);
                Environment.Exit(0);
            }
           
        }
    }
}
