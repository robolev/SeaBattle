namespace SeaBattle
{
    internal class Program
    {
       
        static int size = 3;
        static void Main(string[] args)
        {
           Menu menu = new Menu();
           menu.DrawMenu();
           Game game= new Game();
           game.Seafield();
        }
    }
}