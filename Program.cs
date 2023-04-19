namespace SeaBattle
{
    internal class 
    Program
    {
        static async Task Main(string[] args)
        {
           Menu menu = new Menu();
           menu.DrawMenu();
           Game game= new Game();
           game.Seafield();
           game.PlayGame();
        }
    }
}