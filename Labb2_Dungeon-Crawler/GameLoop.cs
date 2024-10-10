using System;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

class GameLoop
{
    private int turnCounter = 1;
    private string Name = "Adventurer";

    public int CurrentHP { get; private set; }
    public int MaxHP { get; private set; }

    static int NewHP = 100;

    public ConsoleKeyInfo checkKey;

    private LevelData _level = new LevelData();

    public LevelData Level { get { return _level; } }

    public void StartUp(string levelFile)
    {
        Console.CursorVisible = false;
        Console.CursorTop = 0;
        Console.CursorLeft = 0;

        Directory.SetCurrentDirectory(".\\Levels\\");

        Level.Load(levelFile);
    }

    public void GameRunning()
    {
        Console.CursorVisible = false;
        Console.SetCursorPosition(0, 20);
        Console.WriteLine("Use arrow keys to move, space to wait, and escape to exit.");

        foreach (var player in from LevelElements element in Level.Elements
                               where element is Player
                               let player = (Player)element
                               select player)
        {
            player.Exploration(Level.Elements);
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
                player.Exploration(Level.Elements);
                MaxHP = player.maxHealth;
                CurrentHP = player.currentHealth;
                if (CurrentHP > player.maxHealth)
                {
                    player.currentHealth = player.maxHealth;
                    CurrentHP = player.maxHealth;
                }
            }

            Console.ResetColor();
            Console.SetCursorPosition(0, 0);
            Console.Write($"Player: {Name} | HP: {CurrentHP} / {MaxHP}  Turn: {turnCounter++}         ");
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
        } while (checkKey.Key != ConsoleKey.Escape);
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
        Console.SetCursorPosition(0, 2);
        Console.ForegroundColor = firstActor == "Adventurer" ? ConsoleColor.Green : ConsoleColor.Red;
        Console.WriteLine($"{secondActor} encountered.".PadRight(55));
        Console.WriteLine("".PadRight(55));

        PrintAttackResult(firstActor, secondActor, playerCombat, enemyCombat, playerDamage, enemyDamage);

        if (firstActor == "Adventurer")
        {
            HandleAdventurerCombat(playerCombat, enemyCombat, playerDamage, enemyDamage, player, enemy, elements);
        }
        else
        {
            HandleEnemyCombat(playerCombat, enemyCombat, playerDamage, enemyDamage, player, enemy, elements);
        }
    }

    private static void PrintAttackResult(string firstActor, string secondActor,
                                          (int, string, int, string) playerCombat,
                                          (int, string, int, string) enemyCombat,
                                          int playerDamage, int enemyDamage)
    {
        Console.WriteLine($"{firstActor} rolled {(firstActor == "Adventurer" ? playerCombat.Item2 : enemyCombat.Item2)} to attack, result: {(firstActor == "Adventurer" ? playerCombat.Item1 : enemyCombat.Item1)}.".PadRight(55));
        Console.WriteLine($"{secondActor} defended using {(firstActor == "Adventurer" ? enemyCombat.Item4 : playerCombat.Item4)}, result: {(firstActor == "Adventurer" ? enemyCombat.Item3 : playerCombat.Item3)}.".PadRight(55));
        Console.WriteLine($"Damage done by {firstActor} to {secondActor} is: {(firstActor == "Adventurer" ? playerDamage : enemyDamage)}.".PadRight(55));
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
            PrintCounterAttack(enemy.Name, enemyCombat, playerCombat, enemyDamage, player, enemy, elements);
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
            PrintCounterAttack(player.Name, playerCombat, enemyCombat, playerDamage, player, enemy, elements);
        }
    }

    private static void PrintCounterAttack(string secondActor,
                                           (int, string, int, string) attackCombat,
                                           (int, string, int, string) defenseCombat,
                                           int damage, Player player, Enemy enemy,
                                           List<LevelElements> elements)
    {
        Console.WriteLine("".PadRight(55));
        Console.ForegroundColor = secondActor == "Adventurer" ? ConsoleColor.Green : ConsoleColor.Red;
        Console.WriteLine($"Counter attack by {secondActor}, {attackCombat.Item2} with result: {attackCombat.Item1}.".PadRight(55));
        Console.WriteLine($"{secondActor} defended with {defenseCombat.Item4}, result: {defenseCombat.Item3}.".PadRight(55));
        Console.WriteLine($"Counter attack by {secondActor} against {player.Name} did {damage}.".PadRight(55));
        if (player.IsDead)
        {
            player.GameOver();
        }
        if (enemy.IsDead)
        {
            PrintEnemySlain(enemy.Name, elements, enemy, isCounter: true);
        }
    }

    private static void PrintEnemySlain(string actor, List<LevelElements> elements, Enemy enemy, bool isCounter)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("".PadRight(55));
        Console.WriteLine($"{actor} has been slain.".PadRight(55));
        if (!isCounter)
        {
            for (int i = 9; i < 13; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(" ".PadRight(55));
            }
        }
        Console.ResetColor();
        enemy.Die(elements);
    }

    public static void EquipmentPickup(Player player, Equipment equipment, List<LevelElements> elements)
    {
        Console.SetCursorPosition(0, 14);
        Console.WriteLine($"The {equipment.Name} have been acquired.".PadRight(55));
        switch (equipment.Name)
        {
            case "Magic Sword":
                Console.WriteLine("Attack have been increased to 2D10+2.".PadRight(55));
                UpdatingStats(player, equipment.DamageDices, equipment.DmgDiceSides, equipment.DmgDiceModifier, isAttack: true);
                break;
            case "Magic Armor":
                Console.WriteLine("Defense have been increased to 2D8+2.".PadRight(55));
                UpdatingStats(player, equipment.DefenseDice, equipment.DefDiceSides, equipment.DefDiceModifier, isAttack: false);
                break;
        }
        equipment.IsDead = true;
        equipment.Die(elements);

        void UpdatingStats(Player player, int dices, int sides, int modifier, bool isAttack)
        {
            if (isAttack)
            {
                player.damageDices = dices;
                player.dmgDiceSides = sides;
                player.dmgDiceModifier = modifier;
            }
            else
            {
                player.defenseDices = dices;
                player.defDiceSides = sides;
                player.defDiceModifier = modifier;
            }
        }
    }
    public static void ItemPickup(Player player, Items item, List<LevelElements> elements)
    {
        Console.SetCursorPosition(0, 17);
        Console.WriteLine($"{item.Name} acquired, HP restored with {item.HealthRestore}.".PadRight(55));

        switch (item.Name)
        {
            case "Food":
                {
                    player.currentHealth = player.currentHealth + item.HealthRestore;
                    break;
                }
            case "Potion":
                {
                    player.currentHealth = player.currentHealth + item.HealthRestore;
                    break;
                }
        }

        NewHP = player.currentHealth > player.maxHealth ? player.maxHealth : player.currentHealth;
        
        item.IsDead = true;
        item.Die(elements);
    }
}