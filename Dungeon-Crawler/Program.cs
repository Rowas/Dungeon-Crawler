//Labb3 - Dungeon Crawler
//Labb3 for Development against databases and database administration in .NET-developer edu program on ITHS winter 2024/2025.
//Branch of Dungeon Crawler game project that was Labb2 for C# .NET-developer edu program on ITHS fall 2024.
//Adding database functionality to the game, allowing for saving of game state.

using Dungeon_Crawler;
using Dungeon_Crawler.DBModel;

internal class Program
{
    private static void Main(string[] args)
    {
        MainMenuLoops loops = new();

        Console.ResetColor();
        using (var db = new SaveGameContext())
        {
            db.Database.EnsureCreated();
        }

        loops.MainMenu();
    }
}