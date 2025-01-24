using Dungeon_Crawler.DBModel;
using Dungeon_Crawler.GeneralMethods;
using MongoDB.Driver.Linq;

class GameLoop
{
    public static string MapName { get; set; }
    public static int TurnCounter { get; set; } = 1;
    public int CurrentHP { get; private set; } = 100;
    public int MaxHP { get; private set; } = 100;
    public Player CurrentPlayer { get; set; }

    public static Dictionary<int, string> combatLog = [];
    public static int logPosition { get; set; } = 1;

    public static int NewHP = 100;

    public ConsoleKeyInfo checkKey;

    private LevelData _level = new();
    private GameLoad _load = new();

    public LevelData Level { get { return _level; } }
    public GameLoad Load { get { return _load; } }

    public static bool IsPlayerDead { get; set; } = false;

    public void StartUp(string levelFile, string playerName, bool newGame)
    {
        combatLog = new();
        TurnCounter = 1;
        logPosition = new();
        NewHP = 100;
        MapName = string.Empty;
        IsPlayerDead = false;
        Player.CollectedPointMods = 0;

        Console.CursorVisible = false;
        Console.CursorTop = 0;
        Console.CursorLeft = 0;
        MapName = levelFile;
        IsPlayerDead = false;
        ClearConsole.ConsoleClear();

        if (Directory.GetCurrentDirectory() == "D:\\Dev\\Source\\Repos\\Dungeon-Crawler\\Dungeon-Crawler\\bin\\Debug\\net8.0")
        {
            Directory.SetCurrentDirectory(".\\Levels\\");
        }

        if (newGame == true)
        {
            Level.Load(levelFile, playerName);
        }
        else
        {
            Load.LoadGame(Level.Elements, levelFile);
        }

    }

    public void GameRunning(bool sg)
    {
        if (Level.Elements.Count == 0)
        {
            return;
        }
        DrawGameState(Level.Elements, sg);

        foreach (var player in from LevelElements element in Level.Elements
                               where element is Player
                               let player = (Player)element
                               select player)
        {
            player.Exploration(Level.Elements);
            CurrentPlayer = player;
        }
        foreach (var grue in from LevelElements element in Level.Elements
                             where element is Grue
                             let grue = (Grue)element
                             select grue)
        {
            grue.Update([.. Level.Elements]);
        }

        do
        {
            if (TurnCounter > 150)
            {
                var rand = new Random();
                if (rand.NextDouble() < 0.25)
                {
                    var elements = Level.Elements.ToList();
                    foreach (LevelElements element in Level.Elements.ToList())
                    {
                        if (element is Grue || LevelElements.GrueSpawned == true)
                        {
                            break;
                        }
                        else
                        {
                            if (rand.NextDouble() < 0.5)
                            {
                                LevelElements.GrueSpawned = true;
                                Level.Elements.Add(new Grue(107, 13));
                                Grue.Warning();
                            }
                            else
                            {
                                LevelElements.GrueSpawned = true;
                                Level.Elements.Add(new Grue(63, 6));
                                Grue.Warning();
                            }
                        }
                    }
                }
            }
            if (TurnCounter > 250)
            {
                var rand = new Random();
                if (rand.NextDouble() < 0.5)
                {
                    LevelElements.GrueSpawned = true;
                    Level.Elements.Add(new Grue(107, 13));
                    Grue.Warning();
                }
                else
                {
                    LevelElements.GrueSpawned = true;
                    Level.Elements.Add(new Grue(63, 6));
                    Grue.Warning();
                }
            }
            foreach (var player in from LevelElements element in Level.Elements
                                   where element is Player
                                   let player = (Player)element
                                   select player)
            {
                CurrentPlayer = player;
                player.Exploration(Level.Elements);
                MaxHP = player.maxHealth;
                CurrentHP = player.CurrentHealth;
                if (CurrentHP > player.maxHealth)
                {
                    player.CurrentHealth = player.maxHealth;
                    CurrentHP = player.maxHealth;
                }
            }

            while (Console.KeyAvailable == false)
            {
                Thread.Sleep(16);
            }
            checkKey = Console.ReadKey(true);

            if (checkKey.Key == ConsoleKey.Escape)
            {
                ClearConsole.ConsoleClear();
                TextCenter.CenterText("Are you sure you wish to exit the game?");
                Console.WriteLine();
                TextCenter.CenterText("Press 'Escape' again to exit.");
                TextCenter.CenterText("Press any other key to continue");

                while (Console.KeyAvailable == false)
                {
                    Thread.Sleep(16);
                }
                checkKey = Console.ReadKey(true);

                if (checkKey.Key == ConsoleKey.Escape)
                {
                    return;
                }

                Console.SetCursorPosition(0, 0);
                Console.WriteLine(" ".PadRight(100));
                Console.WriteLine(" ".PadRight(100));
                Console.WriteLine(" ".PadRight(100));
                Console.WriteLine(" ".PadRight(100));

                TurnCounter--;
            }

            foreach (LevelElements element in Level.Elements.ToList())
            {
                switch (element)
                {
                    case Player:
                        Player player = (Player)element;
                        CurrentPlayer = player;
                        player.Movement(checkKey, Level.Elements, MapName, sg);
                        if (IsPlayerDead == true) { return; }
                        break;
                    case Enemy:
                        Enemy enemy = (Enemy)element;
                        enemy.Update(Level.Elements);
                        break;
                    case Wall:
                        Wall wall = (Wall)element;
                        wall.DrawWall();
                        break;
                    case Items:
                        Items item = (Items)element;
                        item.Update(Level.Elements);
                        break;
                    case Equipment:
                        Equipment equipment = (Equipment)element;
                        equipment.Update(Level.Elements);
                        break;
                }
            }

            DrawGameState(Level.Elements, sg);

        } while (checkKey.Key != ConsoleKey.Escape);
        return;
    }

    public static void GameLog(List<LevelElements> elements, bool sg)
    {
        int y = combatLog.Count;
        int x = y - 27;

        ConsoleKeyInfo checkKey;

        do
        {
            Console.Clear();
            TextCenter.CenterText("Combat Log (Press \"L\" to exit.)");

            var output = combatLog.Where(s => s.Key >= x && s.Key <= y).Select(s => s.Value).ToList();
            foreach (var log in output)
            {
                TextCenter.CenterText(log);
            }

            while (Console.KeyAvailable == false)
            {
                Thread.Sleep(16);
            }
            checkKey = Console.ReadKey(true);

            switch (checkKey.Key)
            {
                case ConsoleKey.UpArrow:
                    if (x > 1)
                    {
                        x--;
                        y--;
                    }
                    ClearConsole.ConsoleClear();
                    break;
                case ConsoleKey.DownArrow:
                    if (y < combatLog.Count)
                    {
                        x++;
                        y++;
                    }
                    ClearConsole.ConsoleClear();
                    break;
            }
        } while (checkKey.Key != ConsoleKey.L && checkKey.Key != ConsoleKey.Escape);

        ClearConsole.ConsoleClear();

        DrawGameState(elements, sg);
    }

    public static void DrawGameState(List<LevelElements> elements, bool sg)
    {

        Player? player = null;

        foreach (var element in elements)
        {
            if (element.Position == (0, 0))
            {
                element.Position = (element.XPos, element.YPos);
            }
            switch (element)
            {
                case Player:
                    player = (Player)element;
                    element.IsVisible = true;
                    element.DrawPlayer();
                    break;
                case Wall:
                    element.DrawWall();
                    break;
                default:
                    element.Draw();
                    break;
            }
        }

        UIMethods.DrawCombatLog(player);
        UIMethods.PrintUI(player, TurnCounter, MapName, sg);
    }
}