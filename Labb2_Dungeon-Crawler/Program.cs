//Labb2 - Dungeon Crawler
//Labb for C# programming in .NET-developer edu program on ITHS 2024.
using System.IO.Enumeration;
using System.Transactions;
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
        }
        return pickedLevel;
    }   
    
    public static string CustomMap()
    {
        Console.Clear();
        Console.WriteLine("Enter the name of your custom map.");
        Console.WriteLine("Map file have to be placed in ./Levels/ \n as a .txt file and correctly formatted to work. \n Consult pre-made maps for requirements.");
        string customMap = Console.ReadLine();
        return customMap;
    }
}