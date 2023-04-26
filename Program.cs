namespace SeaBattle
{
    internal class 
    Program
    {
        static async Task Main(string[] args)
        {
           Menu menu = new Menu();
           menu.DrawMenu();
           Lobby loobby = new Lobby();
           loobby.CreateNewGame();
           loobby.ChooseIfcontinue();
        }
    }
}