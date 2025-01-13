//Labb3 - Dungeon Crawler
//Labb3 for Development against databases and database administration in .NET-developer edu program on ITHS winter 2024/2025.
//Branch of Dungeon Crawler game project that was Labb2 for C# .NET-developer edu program on ITHS fall 2024.
//Adding database functionality to the game, allowing for saving of game state.

internal class Program
{
    private static void Main(string[] args)
    {
        string levelFile = "";

        levelFile = MainMenu(levelFile);
        string[] values = new string[2];
        values = levelFile.Split('+');
        levelFile = values[1];
        var playerName = values[0];
        Console.Clear();
        GameLoop start = new GameLoop();
        start.StartUp(levelFile, playerName);
        start.GameRunning();
    }

    public static string MainMenu(string levelFile)
    {
        Console.Clear();
        CenterText("Main Menu");
        Console.WriteLine();
        CenterText("Welcome, Adventurer.");
        CenterText("You have entered a dark place.");
        CenterText("Let's hope you survive...");
        Console.WriteLine();
        CenterText("Pick an option: ");

        CenterText("1. Start a new game. (default)");
        CenterText("2. Load a saved game.");
        Console.WriteLine();
        CenterText("0. Exit the game.");
        Console.SetCursorPosition(Console.WindowWidth / 2, 12);
        string menuChoice = Console.ReadLine();

        switch (menuChoice)
        {
            case "1":
                levelFile = NewGame(levelFile);
                break;
            case "2":
                LoadGame();
                break;
            case "3":
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
        CenterText("Tell me, Adventurer, what is your name? ");
        Console.SetCursorPosition(Console.WindowWidth / 2, 3);
        string playerName = Console.ReadLine();
        if (playerName == "")
        {
            playerName = "Adventurer";
        }
        Console.WriteLine();
        Console.Clear();
        CenterText("Ah, " + playerName + ". I greet you. ");
        CenterText("I hope that your quest will be a fortuitous one.");
        CenterText("But... That is something that time will tell, is it not?");
        Console.WriteLine();
        CenterText("Now, let us begin.");
        Console.WriteLine();
        CenterText("Pick and option below: ");
        Console.WriteLine();
        CenterText("1. Load a pre-made map. (Default)");
        CenterText("2. Load a custom map.");

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

    public static void LoadGame()
    {
        //Not implemented yet.
    }

    public static string LevelPicker()
    {
        Console.Clear();
        CenterText("1. Level 1 (Default).");
        CenterText("2. Level 1 (/w Boss & Items).");
        CenterText("Answer with the number matching your choice:");
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
        CenterText("Enter the name of your custom map.");
        CenterText("Map file have to be placed in .\\Levels\\");
        CenterText("as a .txt file and correctly formatted to work.");
        CenterText("Consult pre-made maps for requirements.");
        CenterText("For further information, type 'Help' or '?'.");
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
        CenterText("The following characters are required for a functioning map: ");
        CenterText("@ = Player tile");
        CenterText("# = Wall tile");
        Console.WriteLine();
        CenterText("The following characters are optional for the map: ");
        CenterText("r = Rats");
        CenterText("s = Snakes");
        CenterText("G = Guards");
        CenterText("F/P = Restorative items (25/50 HP)");
        CenterText("A = Magic Armor");
        CenterText("W = Magic Sword");
        CenterText("B = Boss monster");
        Console.WriteLine();
        CenterText("After having created a custom map, place it in .\\Levels\\ as a txt-file and reload the program.");
    }

    private static void CenterText(String text)
    {
        Console.Write(new string(' ', (Console.WindowWidth - text.Length) / 2));
        Console.WriteLine(text);
    }
}