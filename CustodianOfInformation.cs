using System;
using System.IO;
using System.Xml.Serialization;

namespace SeaBattle
{
    public class CustodianOfInformation
    {
        private XmlSerializer formatter;

        public CustodianOfInformation()
        {
            formatter = new XmlSerializer(typeof(Profile));
        }

        public void SavingToTheFiles(bool ifPlayer1Win, int winCount1,int winCount2)
        {
            Profile profile = new Profile(winCount1, winCount2);

            using (FileStream fs = new FileStream("ProfileInformation.xml", FileMode.Create))
            {
                formatter.Serialize(fs, profile);
            }
        }

        public void LoadInformationFromFile()
        {
            using (FileStream fs = new FileStream("ProfileInformation.xml", FileMode.Open))
            {
                Profile profile = (Profile)formatter.Deserialize(fs);
                Console.WriteLine($"Count1: {profile.WinCount1} --- Count2: {profile.WinCount2}");
            }
        }
        public (int,int) AssingTheValue()
        {
            int WinCount1;
            int WinCount2;
            using (FileStream fs = new FileStream("ProfileInformation.xml", FileMode.Open))
            {
                Profile profile = (Profile)formatter.Deserialize(fs);
                WinCount1 = profile.WinCount1;
                WinCount2 = profile.WinCount2;
            }

            return (WinCount1,WinCount2);
        }
    }
}