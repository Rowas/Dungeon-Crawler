﻿using Dungeon_Crawler.GeneralMethods;
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

    public void StartUp(string levelFile, string playerName)
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
            Level.LoadGame();
        }

        MapName = Program.levelFile;
    }

    public void GameRunning()
    {

        foreach (var player in from LevelElements element in Level.Elements
                               where element is Player
                               let player = (Player)element
                               select player)
        {
            player.Exploration(Level.Elements);
            currentPlayer = player;
            PrintUI(currentPlayer);
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
                        player.Movement(checkKey, Level.Elements);
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

            PrintUI(currentPlayer);

            DrawGameState(Level.Elements);

        } while (checkKey.Key != ConsoleKey.Escape);
    }

    public static void PrintUI(Player player)
    {
        Dice defDice = new Dice(player.defDices, player.defDiceSides, player.defDiceModifier);
        Dice dmgDice = new Dice(player.dmgDices, player.dmgDiceSides, player.dmgDiceModifier);
        Console.ResetColor();
        Console.SetCursorPosition(0, 0);
        Console.Write($"Player: {player.Name} | HP: {player.currentHealth} / {player.maxHealth}  Turn: {turnCounter}         ");
        Console.WriteLine($"Current Damage: {dmgDice} | Current Defense: {defDice}");
        Console.Write($"Items aquired: Magic Sword: {player.swordAquired} | Magic Armor: {player.armorAquired}  ");
        Console.WriteLine($"Current Map: {MapName} | Current Score: {Player.CollectedPointMods * 100}".PadRight(50));

        Console.CursorVisible = false;
        Console.SetCursorPosition(0, 24);
        Console.WriteLine("Use arrow keys to move, space to wait, and escape to exit.");
        Console.WriteLine("Press \"L\" to open the combat log.");
        Console.WriteLine("Press \"S\" to save your game.");
    }

    public static void Encounter(Player player, Enemy enemy, char F, List<LevelElements> elements)
    {

        var (firstActor, secondActor) = DetermineActors(player, enemy, F);

        var playerCombat = player.Combat();
        var enemyCombat = enemy.Combat();

        var playerDamage = CalculateDamage(playerCombat.Item1, enemyCombat.Item3);
        var enemyDamage = CalculateDamage(enemyCombat.Item1, playerCombat.Item3);

        ApplyDamage(player, enemy, playerDamage, enemyDamage);

        PrintCombatResult(playerCombat, enemyCombat, playerDamage, enemyDamage, firstActor, secondActor, player, enemy, elements);

    }

    private static (string firstActor, string secondActor) DetermineActors(Player player, Enemy enemy, char F)
    {
        return F == 'P' ? (player.Name, enemy.Name) : (enemy.Name, player.Name);
    }

    private static int CalculateDamage(int attack, int defense)
    {
        int damage = attack - defense;
        return damage < 1 ? 0 : damage;
    }

    private static void ApplyDamage(Player player, Enemy enemy, int playerDamage, int enemyDamage)
    {
        if (playerDamage > 0)
        {
            enemy.CurrentHealth -= playerDamage;
            if (enemy.CurrentHealth < 1)
            {
                enemy.IsDead = true;
            }
        }

        if (enemyDamage > 0 && !enemy.IsDead)
        {
            player.currentHealth -= enemyDamage;
            NewHP = player.currentHealth;
            if (player.currentHealth < 1)
            {
                player.IsDead = true;
            }
        }
    }

    public static void PrintCombatResult((int, string, int, string) playerCombat,
                                     (int, string, int, string) enemyCombat,
                                     int playerDamage,
                                     int enemyDamage,
                                     string firstActor,
                                     string secondActor,
                                     Player player,
                                     Enemy enemy,
                                     List<LevelElements> elements)
    {
        combatLog.Add(logPosition++, "".PadRight(55));
        combatLog.Add(logPosition++, $"{secondActor} encountered.".PadRight(55));

        PrintAttackResult(firstActor, secondActor, playerCombat, enemyCombat, playerDamage, enemyDamage, player);

        if (firstActor == player.Name)
        {
            HandleAdventurerCombat(playerCombat, enemyCombat, playerDamage, enemyDamage, player, enemy, elements);
        }
        else
        {
            HandleEnemyCombat(playerCombat, enemyCombat, playerDamage, enemyDamage, player, enemy, elements);
        }

        GameLoop.DrawGameState(elements);
    }

    private static void PrintAttackResult(string firstActor, string secondActor,
                                          (int, string, int, string) playerCombat,
                                          (int, string, int, string) enemyCombat,
                                          int playerDamage, int enemyDamage, Player player)
    {
        combatLog.Add(logPosition++, "".PadRight(55));
        combatLog.Add(logPosition++, $"{firstActor} rolled {(firstActor == player.Name ? playerCombat.Item2 : enemyCombat.Item2)} to attack, result: {(firstActor == "Adventurer" ? playerCombat.Item1 : enemyCombat.Item1)}.".PadRight(55));
        combatLog.Add(logPosition++, $"{secondActor} defended using {(firstActor == player.Name ? enemyCombat.Item4 : playerCombat.Item4)}, result: {(firstActor == "Adventurer" ? enemyCombat.Item3 : playerCombat.Item3)}.".PadRight(55));
        combatLog.Add(logPosition++, $"Damage done by {firstActor} to {secondActor} is: {(firstActor == player.Name ? playerDamage : enemyDamage)}.".PadRight(55));
    }

    private static void HandleAdventurerCombat((int, string, int, string) playerCombat,
                                               (int, string, int, string) enemyCombat,
                                               int playerDamage, int enemyDamage,
                                               Player player, Enemy enemy,
                                               List<LevelElements> elements)
    {
        if (enemy.IsDead)
        {
            PrintEnemySlain(enemy.Name, elements, enemy, isCounter: false);
        }
        else
        {
            PrintCounterAttack(enemy.Name, player.Name, enemyCombat, playerCombat, enemyDamage, player, enemy, elements);
        }
    }

    private static void HandleEnemyCombat((int, string, int, string) playerCombat,
                                          (int, string, int, string) enemyCombat,
                                          int playerDamage, int enemyDamage,
                                          Player player, Enemy enemy,
                                          List<LevelElements> elements)
    {
        if (player.IsDead)
        {
            player.GameOver();
        }
        else
        {
            PrintCounterAttack(player.Name, enemy.Name, playerCombat, enemyCombat, playerDamage, player, enemy, elements);
        }
    }

    private static void PrintCounterAttack(string secondActor, string firstActor,
                                           (int, string, int, string) attackCombat,
                                           (int, string, int, string) defenseCombat,
                                           int damage, Player player, Enemy enemy,
                                           List<LevelElements> elements)
    {
        combatLog.Add(logPosition++, "".PadRight(55));
        combatLog.Add(logPosition++, $"Counter attack by {secondActor}, {attackCombat.Item2} with result: {attackCombat.Item1}.".PadRight(55));
        combatLog.Add(logPosition++, $"{firstActor} defended with {defenseCombat.Item4}, result: {defenseCombat.Item3}.".PadRight(55));
        combatLog.Add(logPosition++, $"Counter attack by {secondActor} against {firstActor} did {damage}.".PadRight(55));

        if (player.IsDead)
        {
            player.GameOver();
        }
        if (enemy.IsDead)
        {
            Player.CollectedPointMods = Player.CollectedPointMods + enemy.PointModifier;
            PrintEnemySlain(enemy.Name, elements, enemy, isCounter: true);
        }
    }

    private static void PrintEnemySlain(string actor, List<LevelElements> elements, Enemy enemy, bool isCounter)
    {
        combatLog.Add(logPosition++, " ".PadRight(55));
        combatLog.Add(logPosition++, $"{actor} has been slain.".PadRight(55));
        Console.ResetColor();
        Player.CollectedPointMods = Player.CollectedPointMods + enemy.PointModifier;
        enemy.Die(elements);
    }

    public static void EquipmentPickup(Player player, Equipment equipment, List<LevelElements> elements)
    {
        combatLog.Add(logPosition++, " ".PadRight(55));
        combatLog.Add(logPosition++, $"The {equipment.Name} have been acquired.".PadRight(55));
        switch (equipment.Name)
        {
            case "Magic Sword":
                combatLog.Add(logPosition++, "Attack have been increased to 2D10+2.".PadRight(55));
                Player.CollectedPointMods = Player.CollectedPointMods + equipment.PointModifier;
                UpdatingStats(player, equipment.DamageDices, equipment.DmgDiceSides, equipment.DmgDiceModifier, isAttack: true);
                player.swordAquired = true;
                break;
            case "Magic Armor":
                combatLog.Add(logPosition++, "Defense have been increased to 2D8+2.".PadRight(55));
                Player.CollectedPointMods = Player.CollectedPointMods + equipment.PointModifier;
                UpdatingStats(player, equipment.DefenseDice, equipment.DefDiceSides, equipment.DefDiceModifier, isAttack: false);
                player.armorAquired = true;
                break;
        }
        equipment.IsDead = true;
        equipment.Die(elements);

        void UpdatingStats(Player player, int dices, int sides, int modifier, bool isAttack)
        {
            if (isAttack)
            {
                player.dmgDices = dices;
                player.dmgDiceSides = sides;
                player.dmgDiceModifier = modifier;
            }
            else
            {
                player.defDices = dices;
                player.defDiceSides = sides;
                player.defDiceModifier = modifier;
            }
        }
    }
    public static void ItemPickup(Player player, Items item, List<LevelElements> elements)
    {
        combatLog.Add(logPosition++, " ".PadRight(55));
        combatLog.Add(logPosition++, $"{item.Name} acquired, HP restored with {item.HealthRestore}.".PadRight(55));

        switch (item.Name)
        {
            case "Food":
                {
                    player.currentHealth = player.currentHealth + item.HealthRestore;
                    Player.CollectedPointMods = Player.CollectedPointMods + item.PointModifier;
                    break;
                }
            case "Potion":
                {
                    player.currentHealth = player.currentHealth + item.HealthRestore;
                    Player.CollectedPointMods = Player.CollectedPointMods + item.PointModifier;
                    break;
                }
        }

        NewHP = player.currentHealth > player.maxHealth ? player.maxHealth : player.currentHealth;

        item.IsDead = true;
        item.Die(elements);
    }

    public static void GameLog(List<LevelElements> elements)
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

        DrawGameState(elements);
    }

    public static void DrawGameState(List<LevelElements> elements)
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

        DrawCombatLog(player);
        PrintUI(player);
    }

    public static void DrawCombatLog(Player? player)
    {

        if (GameLoop.combatLog.Count() > 0)
        {
            List<string> output = new List<string>();
            Console.ResetColor();
            Console.SetCursorPosition(0, 3);

            int n1 = combatLog.Count;
            int n2 = n1 - 9;

            for (int i = 4; i < 24; i++)
            {
                Console.WriteLine(" ".PadRight(55));
            }

            Console.SetCursorPosition(0, 3);

            var spotCheck = combatLog.Values.Reverse().Take(1).ToList();
            var combatCheck = combatLog.Values.Reverse().Take(2).ToArray();

            if (combatCheck[0].Contains("Counter attack") || combatCheck[0].Contains("slain"))
            {
                spotCheck = combatLog.Where(s => s.Key >= n2 && s.Key <= n1).Select(s => s.Value).ToList();
                if (combatCheck[0].Contains("slain"))
                {
                    if (spotCheck[1].Contains("Rat"))
                    {
                        spotCheck = combatLog.Where(s => s.Key >= n2 - 2 && s.Key <= n1).Select(s => s.Value).ToList();
                    }
                    else
                    {
                        spotCheck.RemoveRange(0, 2);
                    }
                }
                if (spotCheck[1].Contains($"{player.Name}"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    for (int i = 0; i < 6; i++)
                    {
                        Console.WriteLine(spotCheck[i]);
                    }
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    for (int i = 0; i < 6; i++)
                    {
                        Console.WriteLine(spotCheck[i]);
                    }
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                n2 = n1 - 3;
                output = combatLog.Where(s => s.Key >= n2 && s.Key <= n1).Select(s => s.Value).ToList();
                spotCheck = combatLog.Values.Reverse().Take(1).ToList();

                if (spotCheck[0].Contains("slain") && spotCheck[0].Contains("Rat") == false)
                {
                    n2 = n1 - 1;
                    output = combatLog.Where(s => s.Key >= n2 && s.Key <= n1).Select(s => s.Value).ToList();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    foreach (var log in output)
                    {
                        Console.WriteLine(log);
                    }
                }
                else if (spotCheck[0].Contains("slain") && spotCheck[0].Contains("Rat"))
                {
                    output = combatLog.Where(s => s.Key >= n2 - 2 && s.Key <= n1).Select(s => s.Value).ToList();
                    foreach (var log in output)
                    {
                        if (log.Contains("slain"))
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                        }
                        Console.WriteLine(log);
                    }
                }
                else
                {
                    foreach (var log in output)
                    {
                        Console.WriteLine(log);
                    }
                }
            }

            if (spotCheck[0].Contains("Food ") || spotCheck[0].Contains("Potion "))
            {
                n2 = n1 - 1;
                output = combatLog.Where(s => s.Key >= n2 && s.Key <= n1).Select(s => s.Value).ToList();
                Console.ForegroundColor = ConsoleColor.Yellow;
                foreach (var log in output)
                {
                    Console.WriteLine(log);
                }
            }

            if (spotCheck[0].Contains("Attack ") || spotCheck[0].Contains("Defense "))
            {
                n2 = n1 - 1;
                output = combatLog.Where(s => s.Key >= n2 && s.Key <= n1).Select(s => s.Value).ToList();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                foreach (var log in output)
                {
                    Console.WriteLine(log);
                }
            }
        }
    }
}

