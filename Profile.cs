using System;

namespace SeaBattle
{
    public class Profile
    {
        CustodianOfInformation custodian = new CustodianOfInformation();
        public int WinCount1 { get; set; }
        public int WinCount2 { get; set; }

        public Profile()
        {
           // WinCount1 = 0;
          //  WinCount2 = 0;
        }

        public Profile(int winCount1, int winCount2)
        {
          //  WinCount1 += winCount1;
          //  WinCount2 += winCount2;
            AssingPrewiousInformation(winCount1,winCount2);
        }
        public void AssingPrewiousInformation(int winCount1, int winCount2)
        {
            (WinCount1,WinCount2) = custodian.AssingTheValue();
            WinCount1 += winCount1;
            WinCount2 += winCount2;
        }
    }
}
