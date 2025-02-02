//Labb3 - Dungeon Crawler
//Labb3 for Development against databases and database administration in .NET-developer edu program on ITHS winter 2024/2025.
//Branch of Dungeon Crawler game project that was Labb2 for C# .NET-developer edu program on ITHS fall 2024.
//Adding database functionality to the game, allowing for saving of game state.

using Dungeon_Crawler.DBModel;
using Dungeon_Crawler.GeneralMethods;
using Dungeon_Crawler.MainMenu;

internal class Program
{
    private static void Main(string[] args)
    {
        MainMenuLoops loops = new();
        try
        {
            TextCenter.CenterText("Checking Database Connection for Save Game functionality.");
            TextCenter.CenterText("Please wait a moment.");
            TextCenter.CenterText("(Test will take up to 10 seconds.)");

            Console.ResetColor();
            using (var db = new SaveGameContext())
            {
                db.Database.EnsureCreated();
                //db.Database.CanConnect();
            }
            Console.WriteLine();
            TextCenter.CenterText("Database Connection Found.");
            TextCenter.CenterText("Save Game functionality available.");
            TextCenter.CenterText("Press any key to continue.");
            Console.ReadKey();

            loops.MainMenu(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            TextCenter.CenterText("No database connection available. ");
            TextCenter.CenterText("Starting game with Save Game functionality turned off. ");
            TextCenter.CenterText("Press any key to continue.");
            Console.ReadKey();
            loops.MainMenu(false);
        }
    }
}