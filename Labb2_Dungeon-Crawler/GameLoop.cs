using System.ComponentModel.Design;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;

class GameLoop
{
    private int turnCounter = 1;
    private string Name = "Adventurer";

    public int CurrentHP {  get; private set; }
    public int MaxHP { get; private set; }

    static int NewHP = 100;

    public ConsoleKeyInfo checkKey;

    private LevelData level1 = new LevelData();

    public LevelData Level1
    {
        get
        {
            return level1;
        }
    }

    public void StartUp(string levelFile)
    {
        Console.CursorVisible = false;
        Console.CursorTop = 0;
        Console.CursorLeft = 0;

        Directory.SetCurrentDirectory(".\\Levels\\");

        level1.Load(levelFile);
    }

    public void GameRunning()
    {
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("Use arrow keys to move, space to wait, and escape to exit.");
        Console.WriteLine();

        foreach (LevelElements element in Level1.Elements)
        {
            if (element is Player)
            {
                Player player = (Player)element;
                player.Exploration(Level1.Elements);
            }
        }

        do
        {
            foreach (LevelElements element in Level1.Elements)
            {
                if (element is Player)
                {
                    Player player = (Player)element;
                    player.Exploration(Level1.Elements);
                    MaxHP = player.maxHealth;
                    CurrentHP = player.currentHealth;
                    if (CurrentHP > player.maxHealth)
                    {
                        player.currentHealth = player.maxHealth;
                        CurrentHP = player.maxHealth;
                    }
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

            foreach (LevelElements element in Level1.Elements.ToList())
            {
                switch (element)
                {
                    case Player:
                        Player player = (Player)element;
                        player.Movement(checkKey, Level1.Elements);
                        break;
                    case Enemy:
                        Enemy enemy = (Enemy)element;
                        enemy.Update(Level1.Elements);
                        break;
                    case Wall:
                        Wall wall = (Wall)element;
                        wall.DrawWall();
                        break;
                    case Items:
                        Items item = (Items)element;
                        item.Update(level1.Elements);
                        break;
                    case Equipment:
                        Equipment equipment = (Equipment)element;
                        equipment.Update(level1.Elements);
                        break;
                }
            }
        } while (checkKey.Key != ConsoleKey.Escape);
    }

    public static void Encounter(Player player, Enemy enemy, char F, List<LevelElements> elements)
    {
        Console.SetCursorPosition(0, 1);
        GameLoop gameLoop = new GameLoop();

        string firstActor;
        string secondActor;

        if (F == 'P')
        {
            firstActor = player.Name;
            secondActor = enemy.Name;
        }
        else
        {
            firstActor = enemy.Name;
            secondActor = player.Name;
        }

        (int, string, int, string) playerCombat = player.Combat();
        (int, string, int, string) enemyCombat = enemy.Combat();

        int playerDamage = playerCombat.Item1 - enemyCombat.Item3;

        int enemyDamage = enemyCombat.Item1 - playerCombat.Item3;

        if (playerDamage < 1)
        {
            playerDamage = 0;
        }
        else if (playerDamage > 1)
        {
            enemy.CurrentHealth = enemy.CurrentHealth - playerDamage;

            if (enemy.CurrentHealth < 1)
            {
                enemy.IsDead = true;
            }
        }

        if (enemyDamage < 1)
        {
            enemyDamage = 0;
        }
        else if (enemyDamage > 0 && enemy.IsDead == false)
        {
            player.currentHealth = player.currentHealth - enemyDamage;
            NewHP = player.currentHealth;
            if (player.currentHealth < 1)
            {
                player.IsDead = true;
            }
        }

        PrintCombatResult(playerCombat, enemyCombat, playerDamage, enemyDamage, firstActor, secondActor, player, enemy, elements);
    }

    public static void PrintCombatResult((int,string,int,string) playerCombat, (int,string,int,string) enemyCombat, int playerDamage, int enemyDamage, string firstActor, string secondActor, Player player, Enemy enemy, List<LevelElements> elements)
    {
        if (firstActor == "Adventurer")
        {
            for (int i = 0; i < 13; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("                                                         ");
            }
            Console.SetCursorPosition(0, 2);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{secondActor} encountered.");
            Console.WriteLine();
            Console.WriteLine($"{firstActor} rolled {playerCombat.Item2} to attack, result: {playerCombat.Item1}.");
            Console.WriteLine($"{secondActor} defended using {enemyCombat.Item4}, result: {enemyCombat.Item3}.");
            Console.WriteLine($"Damage done by {firstActor} to {secondActor} is: {playerDamage}.");
            if (enemy.IsDead == true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine();
                Console.WriteLine($"{secondActor} has been slain.");
                for (int i = 9; i < 12; i++)
                {
                    Console.SetCursorPosition(0, i);
                    Console.Write("                                                         ");
                }
                Console.ResetColor();

                enemy.Die(elements);
            }
            else
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Counter attack by {secondActor}, {enemyCombat.Item2} with result: {enemyCombat.Item1}.");
                Console.WriteLine($"{firstActor} defended with {playerCombat.Item4}, result: {playerCombat.Item3}.");
                Console.WriteLine($"Counter attack by {secondActor} against {firstActor} did {enemyDamage}.");
                if (player.IsDead == true)
                {
                    player.GameOver();
                }
            }
        }
        else
        {
            for (int i = 0; i < 13; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("                                                         ");
            }
            Console.SetCursorPosition(0, 2);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{secondActor} encountered.");
            Console.WriteLine();
            Console.WriteLine($"{firstActor} rolled {enemyCombat.Item2} to attack, result: {enemyCombat.Item1}.");
            Console.WriteLine($"{secondActor} defended using {playerCombat.Item4}, result: {playerCombat.Item3}.");
            Console.WriteLine($"Damage done by {firstActor} to {secondActor} is: {enemyDamage}.");
            if (player.IsDead == true)
            {
                player.GameOver();
            }
            else
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Counter attack by {secondActor}, {playerCombat.Item2} with result: {playerCombat.Item1}.");
                Console.WriteLine($"{firstActor} defended with {enemyCombat.Item4}, result: {enemyCombat.Item3}.");
                Console.WriteLine($"Counter attack by {secondActor} against {firstActor} did {playerDamage}.");
            }
            if (enemy.IsDead == true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine();
                Console.WriteLine($"{firstActor} has been slain.");
                Console.ResetColor();
                enemy.Die(elements);
            }
        }
    }

    public static void EquipmentPickup(Player player, Equipment equipment, List<LevelElements> elements)
    {
        switch (equipment.Name)
        {
            case "Magic Sword":
                Console.SetCursorPosition(0, 10);
                Console.WriteLine($"The {equipment.Name} have been acquired.     ");
                Console.WriteLine("Attack have been increased to 2D10+2.");
                player.damageDices = equipment.DamageDices;
                player.dmgDiceSides = equipment.DmgDiceSides;
                player.dmgDiceModifier = equipment.DmgDiceModifier;
                equipment.IsDead = true;
                equipment.Die(elements);
                break;
            case "Magic Armor":
                Console.SetCursorPosition(0, 10);
                Console.WriteLine($"The {equipment.Name} have been acquired.     ");
                Console.WriteLine("Defense have been increased to 2D8+2.");
                player.defenseDices = equipment.DefenseDice;
                player.defDiceSides = equipment.DefDiceSides;
                player.defDiceModifier = equipment.DefDiceModifier;
                equipment.IsDead = true;
                equipment.Die(elements);
                break;
        }
    }
    public static void ItemPickup(Player player, Items item, List<LevelElements> elements)
    {
        switch (item.Name)
        {
            case "Food":
                {
                    for (int i = 13; i > 15; i++)
                    {
                        Console.Write("                                                         ");
                    }
                    Console.SetCursorPosition(0, 13);
                    Console.WriteLine($"{item.Name} acquired, HP restored with {item.HealthRestore}.");
                    player.currentHealth = player.currentHealth + item.HealthRestore;
                    NewHP = player.currentHealth;
                    if (NewHP > player.maxHealth)
                    {
                        NewHP = 100;
                    }
                    item.IsDead = true;
                    item.Die(elements);
                    break;
                }
            case "Potion":
                {
                    for (int i = 13; i > 13; i++)
                    {
                        Console.Write("                                                         "); 
                    }
                    Console.SetCursorPosition(0, 13);
                    Console.WriteLine($"{item.Name} acquired, HP restored with {item.HealthRestore}.");
                    player.currentHealth = player.currentHealth + item.HealthRestore;
                    NewHP = player.currentHealth;
                    if (NewHP > player.maxHealth)
                    {
                        NewHP = 100;
                    }
                    item.IsDead = true;
                    item.Die(elements);
                    break;
                }
        }
    }
}