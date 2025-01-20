using Dungeon_Crawler.DBModel;
using Dungeon_Crawler.GeneralMethods;
using MongoDB.Driver.Linq;

class GameLoop
{
    public static string MapName { get; set; }
    public static int turnCounter { get; set; } = 1;
    string? name { get; set; }
    public int CurrentHP { get; private set; } = 100;
    public int MaxHP { get; private set; } = 100;
    public Player currentPlayer { get; set; }

    public static Dictionary<int, string> combatLog = new Dictionary<int, string>();
    public static int logPosition { get; set; } = 1;

    public static int NewHP = 100;

    public ConsoleKeyInfo checkKey;

    private LevelData _level = new LevelData();

    public LevelData Level { get { return _level; } }

    public void StartUp(string levelFile, string playerName, string selectedGame)
    {
        Console.CursorVisible = false;
        Console.CursorTop = 0;
        Console.CursorLeft = 0;

        name = playerName;

        Directory.SetCurrentDirectory(".\\Levels\\");

        if (levelFile != "GameLoaded")
        {
            Level.Load(levelFile, playerName);
        }
        else
        {
            GameLoad.LoadGame(Level.Elements, selectedGame);
        }

        MapName = Program.levelFile;
    }

    public void GameRunning()
    {
        DrawGameState(Level.Elements, combatLog);

        foreach (var player in from LevelElements element in Level.Elements
                               where element is Player
                               let player = (Player)element
                               select player)
        {
            player.Exploration(Level.Elements);
            currentPlayer = player;
        }
        foreach (var grue in from LevelElements element in Level.Elements
                             where element is Grue
                             let grue = (Grue)element
                             select grue)
        {
            grue.Update(Level.Elements.ToList());
        }

        do
        {
            if (turnCounter > 150)
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
                            Level.Elements.ToList();
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
            foreach (var player in from LevelElements element in Level.Elements
                                   where element is Player
                                   let player = (Player)element
                                   select player)
            {
                currentPlayer = player;
                player.Exploration(Level.Elements);
                MaxHP = player.maxHealth;
                CurrentHP = player.currentHealth;
                if (CurrentHP > player.maxHealth)
                {
                    player.currentHealth = player.maxHealth;
                    CurrentHP = player.maxHealth;
                }
            }

            while (Console.KeyAvailable == false)
            {
                Thread.Sleep(16);
            }
            checkKey = Console.ReadKey(true);

            foreach (LevelElements element in Level.Elements.ToList())
            {
                switch (element)
                {
                    case Player:
                        Player player = (Player)element;
                        currentPlayer = player;
                        player.Movement(checkKey, Level.Elements, combatLog);
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

            DrawGameState(Level.Elements, combatLog);

        } while (checkKey.Key != ConsoleKey.Escape);
    }

    public static void GameLog(List<LevelElements> elements, Dictionary<int, string> combatLog)
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

        DrawGameState(elements, combatLog);
    }

    public static void DrawGameState(List<LevelElements> elements, Dictionary<int, string> combatLog)
    {

        Player? player = null;

        foreach (var element in elements)
        {
            if (element.Position == (0, 0))
            {
                element.Position = (element.xPos, element.yPos);
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

        UIMethods.DrawCombatLog(player, combatLog);
        UIMethods.PrintUI(player);
    }
}