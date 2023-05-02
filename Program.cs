namespace SeaBattle
{
    internal class 
    Program
    {
        static async Task Main(string[] args)
        {
           CustodianOfInformation custodian = new CustodianOfInformation();
            custodian.LoadInformationFromFile();  
           Menu menu = new Menu();
           menu.DrawMenu();
           Lobby loobby = new Lobby();
           loobby.CreateNewGame();
           loobby.ChooseIfcontinue();
        }
    }
}