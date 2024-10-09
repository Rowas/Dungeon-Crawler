//Labb2 - Dungeon Crawler
//Labb2 for C# programming in .NET-developer edu program on ITHS Fall 2024.
internal class Program
{
    private static void Main(string[] args)
    {
        string levelFile = "";
        //Main Menu
        Console.WriteLine("Pick an option:");
        Console.WriteLine("1. Load a pre-made map.");
        Console.WriteLine("2. Load a custom map.");
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

        Console.CursorVisible = false;
        Console.Clear();
        GameLoop start = new GameLoop();
        start.StartUp(levelFile);
        start.GameRunning();
    }

    public static string LevelPicker()
    {
        Console.Clear();
        Console.WriteLine("1. Level 1 (default map).");
        Console.WriteLine("2. Level 1 (/w Boss).");
        Console.WriteLine("Answer with the number matching your choice:");
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
        Console.WriteLine("Enter the name of your custom map.");
        Console.WriteLine("Map file have to be placed in ./Levels/ \nas a .txt file and correctly formatted to work. \n\nConsult pre-made maps for requirements. \nFor further information, type 'Help' or '?'.");
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
        Console.WriteLine("The following characters are required for a functioning map: \n@ = Player tile \n# = Wall tile \n");
        Console.WriteLine("The following characters are optional for the map: \nr = Rats \ns = Snakes \nG = Guards \nF/P = Restorative items (25/50 HP) \nA = Magic Armor \nW = Magic Sword \nB = Boss monster\n");
        Console.WriteLine("After having created a custom map, place it in .\\Levels\\ as a txt-file and reload the program.");
    }
}