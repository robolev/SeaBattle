﻿using System;

namespace SeaBattle
{
    public class Lobby
    {

        Game game = new Game();

        int player1WinCount;
        int player2WinCount;
        public void ChooseIfcontinue()
        {
            while (true)
            {
                DrawStatistics();
                Console.WriteLine("Do you want to play again");
                Console.WriteLine("1. Yes");
                Console.WriteLine("2. No");
                Console.Write("Enter choice (1/2): ");
                string choice = Console.ReadLine();
                if (choice == "1")
                    CreateNewGame();
                else
                    Environment.Exit(0);
            }
        }
        public void CreateNewGame()
        {
            game = new Game();
            game.PlayGame();
        }
        public (int,int) PLayersWinStatistics(bool player1Win)
        {
            if (player1Win)
                player1WinCount++;
            else
                player2WinCount++;
            return(player1WinCount,player2WinCount);
        }
        public void DrawStatistics()
        {
            bool winner = game.CheckingWin();
            (player1WinCount, player2WinCount) = PLayersWinStatistics(winner);
            Console.WriteLine("Player 1 win statistic");
            Console.WriteLine(player1WinCount + "   ");
            Console.WriteLine("Player 2 win statistic");
            Console.WriteLine(player2WinCount + "   ");
        }
    }
}
