//Labb3 - Dungeon Crawler
//Labb3 for Development against databases and database administration in .NET-developer edu program on ITHS winter 2024/2025.
//Branch of Dungeon Crawler game project that was Labb2 for C# .NET-developer edu program on ITHS fall 2024.
//Adding database functionality to the game, allowing for saving of game state.

using Dungeon_Crawler.DBModel;
using Dungeon_Crawler.GeneralMethods;

internal class Program
{
    public static string levelFile = "";
    private static void Main(string[] args)
    {
        Console.ResetColor();
        using (var db = new SaveGameContext())
        {
            db.Database.EnsureCreated();
        }
        while (levelFile == "")
        {
            levelFile = MainMenu(levelFile);
        }
        string[] values = new string[2];
        values = levelFile.Split('+');
        var playerName = values[0];
        levelFile = values[1];

        ClearConsole.ConsoleClear();
        GameLoop start = new GameLoop();

        start.StartUp(levelFile, playerName);
        start.GameRunning();
    }

    public static string MainMenu(string levelFile)
    {
        string menuChoice;
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkRed;
        TextCenter.CenterText("Main Menu");
        Console.ResetColor();
        Console.WriteLine();
        TextCenter.CenterText("Welcome, Adventurer.");
        TextCenter.CenterText("You have entered a dark place.");
        TextCenter.CenterText("Let's hope you survive...");
        Console.WriteLine();
        TextCenter.CenterText("Pick an option: ");
        Console.WriteLine();
        TextCenter.CenterText("1. Start a new game. (default)");
        TextCenter.CenterText("2. Load a saved game.");
        TextCenter.CenterText("3. View highscores.");
        Console.WriteLine();
        TextCenter.CenterText("0. Exit the game.");
        Console.SetCursorPosition(Console.WindowWidth / 2, 13);
        menuChoice = Console.ReadLine();

        switch (menuChoice)
        {
            case "1":
                levelFile = NewGame(levelFile);
                break;
            case "2":
                levelFile = "Adventurer+GameLoaded";
                break;
            case "3":
                PrintHighScore();
                break;
            case "0":
                Environment.Exit(0);
                break;
            default:
                levelFile = NewGame(levelFile);
                break;
        }
        return levelFile;
    }

    public static string NewGame(string levelFile)
    {
        Console.Clear();
        TextCenter.CenterText("Tell me, Adventurer, what is your name? ");
        Console.SetCursorPosition(Console.WindowWidth / 2, 3);
        string playerName = Console.ReadLine();
        if (playerName == "")
        {
            playerName = "Adventurer";
        }
        Console.WriteLine();
        Console.Clear();
        TextCenter.CenterText("Ah, " + playerName + ". I greet you. ");
        TextCenter.CenterText("I hope that your quest will be a fortuitous one.");
        TextCenter.CenterText("But... That is something that time will tell, is it not?");
        Console.WriteLine();
        TextCenter.CenterText("Now, let us begin.");
        Console.WriteLine();
        TextCenter.CenterText("Pick and option below: ");
        Console.WriteLine();
        TextCenter.CenterText("1. Load a pre-made map. (Default)");
        TextCenter.CenterText("2. Load a custom map.");

        Console.SetCursorPosition(Console.WindowWidth / 2, 12);
        string menuChoice = Console.ReadLine();

        switch (menuChoice)
        {
            case "1":
                levelFile = LevelPicker();
                break;
            case "2":
                levelFile = CustomMap();
                break;
            default:
                levelFile = LevelPicker();
                break;
        }
        levelFile = playerName + "+" + levelFile;
        return levelFile;
    }

    public static string LevelPicker()
    {
        Console.Clear();
        TextCenter.CenterText("1. Level 1 (Default).");
        TextCenter.CenterText("2. Level 1 (/w Boss & Items).");
        TextCenter.CenterText("Answer with the number matching your choice:");
        Console.SetCursorPosition(Console.WindowWidth / 2, 3);
        string pickedLevel = Console.ReadLine();
        switch (pickedLevel)
        {
            case "1":
                pickedLevel = "Level1.txt";
                break;
            case "2":
                pickedLevel = "Level1_w_Boss.txt";
                break;
            default:
                pickedLevel = "Level1.txt";
                break;
        }
        return pickedLevel;
    }

    public static string CustomMap()
    {
        Console.Clear();
        TextCenter.CenterText("Enter the name of your custom map.");
        TextCenter.CenterText("Map file have to be placed in .\\Levels\\");
        TextCenter.CenterText("as a .txt file and correctly formatted to work.");
        TextCenter.CenterText("Consult pre-made maps for requirements.");
        TextCenter.CenterText("For further information, type 'Help' or '?'.");
        Console.SetCursorPosition(Console.WindowWidth / 2, 5);
        string customMap = Console.ReadLine();
        switch (customMap)
        {
            case "help":
                MapHelp();
                Environment.Exit(0);
                break;
            case "Help":
                MapHelp();
                Environment.Exit(0);
                break;
            case "?":
                MapHelp();
                Environment.Exit(0);
                break;
        }
        return customMap;
    }

    public static void MapHelp()
    {
        Console.Clear();
        TextCenter.CenterText("The following characters are required for a functioning map: ");
        TextCenter.CenterText("@ = Player tile");
        TextCenter.CenterText("# = Wall tile");
        Console.WriteLine();
        TextCenter.CenterText("The following characters are optional for the map: ");
        TextCenter.CenterText("r = Rats");
        TextCenter.CenterText("s = Snakes");
        TextCenter.CenterText("G = Guards");
        TextCenter.CenterText("F/P = Restorative items (25/50 HP)");
        TextCenter.CenterText("A = Magic Armor");
        TextCenter.CenterText("W = Magic Sword");
        TextCenter.CenterText("B = Boss monster");
        Console.WriteLine();
        TextCenter.CenterText("After having created a custom map, place it in .\\Levels\\ as a txt-file and reload the program.");
    }

    public static void PrintHighScore()
    {
        using (var db = new SaveGameContext())
        {
            var highscores = db.Highscores.OrderByDescending(s => s.Score).ToList();
            Console.Clear();
            TextCenter.CenterText("Highscores");
            Console.WriteLine();
            foreach (var highscore in highscores)
            {
                TextCenter.CenterText("Player: " + highscore.PlayerName + " | Map: " + highscore.MapName + " | Score: " + highscore.Score + " | Date: " + highscore.SaveDate);
            }
        }
        Console.ReadKey();
    }
}