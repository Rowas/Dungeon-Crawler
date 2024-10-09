//Labb2 - Dungeon Crawler
//Labb2 for C# programming in .NET-developer edu program on ITHS Fall 2024.
using System;

internal class Program
{
    private static void Main(string[] args)
    {
        string levelFile = "";

        //Main Menu
        CenterText("Welcome, Adventurer.");
        CenterText("You have entered a dark place.");
        CenterText("Let's hope you survive...");
        Console.WriteLine();
        CenterText("Pick an option:");
        CenterText("1. Load a pre-made map.");
        CenterText("2. Load a custom map.");
        Console.SetCursorPosition(Console.WindowWidth / 2, 7);
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

        Console.Clear();
        GameLoop start = new GameLoop();
        start.StartUp(levelFile);
        start.GameRunning();
    }

    public static string LevelPicker()
    {
        Console.Clear();
        CenterText("1. Level 1 (default map).");
        CenterText("2. Level 1 (/w Boss).");
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
        CenterText("Map file have to be placed in ./Levels/");
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
        CenterText("The following characters are required for a functioning map: ");CenterText("@ = Player tile"); CenterText("# = Wall tile");
        Console.WriteLine();
        CenterText("The following characters are optional for the map: ");
        CenterText("r = Rats");
        CenterText("s = Snakes");
        CenterText("G = Guards");
        CenterText("F /P = Restorative items (25/50 HP)");
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