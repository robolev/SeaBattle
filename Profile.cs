using System;
using System.Reflection;

namespace SeaBattle
{
    public class Profile
    {
        CustodianOfInformation custodian = new CustodianOfInformation();
        public int WinCount1 { get; set; }
        public int WinCount2 { get; set; }

        public int player1WinCount;
        public int player2WinCount;

        bool winner;

        public Profile()
        {
            player1WinCount = 0;
            player2WinCount = 0;
        }

        public Profile(int winCount1, int winCount2)
        {
            AssingPrewiousInformation(winCount1, winCount2);
        }

        public void AssingPrewiousInformation(int winCount1, int winCount2)
        {
            (WinCount1, WinCount2) = custodian.AssingTheValue();
            WinCount1 += winCount1;
            WinCount2 += winCount2;
        }

        public void PLayersWinStatistics(bool player1Win)
        {
            player1WinCount = 0;
            player2WinCount = 0;
            if (player1Win)
                player1WinCount++;
            else
                player2WinCount++;
        }

        public void SaveCount(int player1WinCount, int player2WinCount)
        {
            custodian.SavingToTheFiles(player1WinCount, player2WinCount);
        }

        public (int, int) CalculateStatistic()
        {
            WinCount1 += player1WinCount;
            WinCount2 += player2WinCount;

            return (WinCount1, WinCount2);
        }
    }
}
