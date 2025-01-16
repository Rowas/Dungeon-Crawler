﻿using Dungeon_Crawler.DBModel;
using Dungeon_Crawler.GeneralMethods;

class LevelData
{
    private List<LevelElements> _elements = new List<LevelElements>();

    public List<LevelElements> Elements
    {
        get
        {
            return _elements;
        }
    }

    public void Load(string filename, string playerName)
    {
        try
        {
            using (StreamReader reader = new StreamReader(filename))
            {
                string line;
                int y = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    for (int x = 0; x < line.Length; x++)
                    {
                        switch (line[x])
                        {
                            case '#':
                                Elements.Add(new Wall(x + 59, y + 2));
                                break;
                            case '@':
                                Elements.Add(new Player(x + 59, y + 2, playerName));
                                break;
                            case 'r':
                                Elements.Add(new Rat(x + 59, y + 2));
                                break;
                            case 's':
                                Elements.Add(new Snake(x + 59, y + 2));
                                break;
                            case 'B':
                                Elements.Add(new Boss(x + 59, y + 2));
                                break;
                            case 'G':
                                Elements.Add(new Guard(x + 59, y + 2));
                                break;
                            case 'W':
                                Elements.Add(new Sword(x + 59, y + 2));
                                break;
                            case 'A':
                                Elements.Add(new Armor(x + 59, y + 2));
                                break;
                            case 'F':
                                Elements.Add(new Food(x + 59, y + 2));
                                break;
                            case 'P':
                                Elements.Add(new Potion(x + 59, y + 2));
                                break;
                            case 'E':
                                Elements.Add(new Grue(x + 59, y + 2));
                                break;
                        }
                    }
                    y++;
                }
            }
        }
        catch (Exception ArgumentException)
        {
            Console.Clear();
            TextCenter.CenterText("Invalid Custom Map selected.");
            TextCenter.CenterText("Map does not exist.");
            Console.WriteLine();
            TextCenter.CenterText("Press any key to exit.");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }

    public void LoadGame()
    {

        List<LevelElements> gameState = new List<LevelElements>();

        try
        {
            using (var db = new SaveGameContext())
            {
                var saveGame = db.SaveGames.OrderByDescending(s => s.SaveDate).FirstOrDefault();
                if (saveGame != null)
                {
                    LevelElements.SaveGameName = saveGame.Id.ToString();
                    Program.levelFile = saveGame.MapName.ToString();
                    gameState = LoadGameState(saveGame.gameState);
                    Console.Clear();
                    TextCenter.CenterText("Save game loaded.");
                    Console.WriteLine();
                    TextCenter.CenterText("Press any key to continue.");
                    Console.ReadKey();
                    Console.Clear();
                    DrawGameState(gameState);
                    GameLoop.turnCounter = saveGame.gameState.CurrentTurn;
                }
                else
                {
                    Console.Clear();
                    TextCenter.CenterText("No save game found.");
                    Console.WriteLine();
                    TextCenter.CenterText("Press any key to exit.");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Clear();
            TextCenter.CenterText("Unable to load save game.");
            TextCenter.CenterText("Database corrupted, save game does not exist or save game is from a different version.");
            Console.WriteLine();
            TextCenter.CenterText("Press any key to exit.");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }

    public List<LevelElements> LoadGameState(GameState gameState)
    {

        Elements.Add(gameState.Player);
        Elements.AddRange(gameState.Walls);
        Elements.AddRange(gameState.Bosses);
        Elements.AddRange(gameState.Guards);
        Elements.AddRange(gameState.Rats);
        Elements.AddRange(gameState.Snakes);
        Elements.AddRange(gameState.Armors);
        Elements.AddRange(gameState.Swords);
        Elements.AddRange(gameState.Foods);
        Elements.AddRange(gameState.Potions);
        Elements.AddRange(gameState.Grues);
        return Elements;
    }

    public void DrawGameState(List<LevelElements> elements)
    {
        GameLoop gameLoop = new GameLoop();

        Player? player = null;
        Armor? armor = null;
        Sword? sword = null;
        Food? food = null;
        Potion? potion = null;

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

        //gameLoop.PrintUI(player);

        if (GameLoop.combatLog.Count() > 0)
        {
            List<string> print = new List<string>();
            Console.ResetColor();
            Console.SetCursorPosition(0, 3);
            if (GameLoop.combatLog.Values.Reverse().FirstOrDefault().Contains("Rat") == true && GameLoop.combatLog.Values.Reverse().FirstOrDefault().Contains("slain") == true)
            {
                print = GameLoop.combatLog.Values.Reverse().Take(11).ToList();
            }
            else if (GameLoop.combatLog.Values.Reverse().FirstOrDefault().Contains("increased") == true)
            {
                print = GameLoop.combatLog.Values.Reverse().Take(2).ToList();
            }
            else
            {
                print = GameLoop.combatLog.Values.Reverse().Take(9).ToList();
            }
            print.Reverse();
            int x = 0;
            bool playerEncounter = false;
            foreach (var line in print)
            {
                if (line.Contains(player.Name + " encountered"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    playerEncounter = true;
                }
                else if (x < 5 && playerEncounter == false)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else if (x > 4 && playerEncounter == true)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                if (line.Contains("slain"))
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                if (line.Contains("Magic Sword") == true)
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.SetCursorPosition(0, 19);
                }
                if (line.Contains("Magic Armor") == true)
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.SetCursorPosition(0, 21);
                }

                if (line.Contains("HP restored"))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.SetCursorPosition(0, 18);
                    Console.WriteLine(line);
                    break;
                }
                Console.WriteLine(line);
                x++;
            }
            Console.ResetColor();
        }

    }
}
