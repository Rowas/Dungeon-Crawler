using System.ComponentModel.Design;
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
                player.DistanceCheck(Level1.Elements);
            }
        }

        do
        {
            foreach (LevelElements element in Level1.Elements)
            {
                if (element is Player)
                {
                    Player player = (Player)element;
                    player.DistanceCheck(Level1.Elements);
                    MaxHP = player.maxHealth;
                    CurrentHP = player.currentHealth;
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
                if (element is Player)
                {
                    Player player = (Player)element;
                    CurrentHP = player.currentHealth;
                    player.Movement(checkKey, Level1.Elements);
                }
                else if (element is Enemy)
                {
                    Enemy enemy = (Enemy)element;
                    enemy.Update(Level1.Elements);
                }
                else if (element is Equipment)
                {
                    Equipment equipment = (Equipment)element;
                    equipment.Update(level1.Elements);
                }
                else if (element is Items)
                {
                    Items item = (Items)element;
                    item.Update(level1.Elements);
                }
                else if (element is Wall)
                {
                    element.DrawWall();
                }
            }
            if (NewHP > 100)
            {
                NewHP = 100;
            }
            CurrentHP = NewHP;

        } while (checkKey.Key != ConsoleKey.Escape);
    }

    public static void Encounter(Player player, Enemy enemy, char F)
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

        int[] CombatRollResults = new int[6];

        CombatRollResults[0] = player.Attack().Item1; //Player attack roll
        CombatRollResults[1] = player.Defend().Item1; //Player defense roll
        CombatRollResults[2] = enemy.Attack().Item1; //Enemy attack roll
        CombatRollResults[3] = enemy.Defend().Item1; //Enemy defense roll
        CombatRollResults[4] = player.Attack().Item1 - enemy.Defend().Item1; //Player damage done
        CombatRollResults[5] = enemy.Attack().Item1 - player.Defend().Item1; //Enemy damage done

        string[] actorDices = new string[5];

        actorDices[0] = player.Attack().Item2;
        actorDices[1] = player.Defend().Item2;
        actorDices[2] = enemy.Attack().Item2;
        actorDices[3] = enemy.Defend().Item2;

        if (CombatRollResults[4] < 1)
        {
            CombatRollResults[4] = 0;
        }
        else if (CombatRollResults[4] > 1)
        {
            enemy.CurrentHealth = enemy.CurrentHealth - CombatRollResults[4];

            if (enemy.CurrentHealth < 1)
            {
                enemy.IsDead = true;
            }
        }

        if (CombatRollResults[5] < 1)
        {
            CombatRollResults[5] = 0;
        }
        else if (CombatRollResults[5] > 0 && enemy.IsDead == false)
        {
            player.currentHealth = player.currentHealth - CombatRollResults[5];
            NewHP = player.currentHealth;
            if (player.currentHealth < 1)
            {
                player.IsDead = true;
            }
        }

        PrintCombatResult(CombatRollResults, actorDices, firstActor, secondActor, player, enemy);
    }

    public static void PrintCombatResult(int[] dmgNumbers, string[] actorDices, string firstActor, string secondActor, Player player, Enemy enemy)
    {
        if (firstActor == "Adventurer")
        {
            for (int i = 0; i < 12; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("                                                         ");
            }
            Console.SetCursorPosition(0, 2);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{secondActor} encountered.");
            Console.WriteLine();
            Console.WriteLine($"{firstActor} rolled {actorDices[0]} to attack, result: {dmgNumbers[0]}.");
            Console.WriteLine($"{secondActor} defended using {actorDices[3]}, result: {dmgNumbers[3]}.");
            Console.WriteLine($"Damage done by {firstActor} to {secondActor} is: {dmgNumbers[4]}.");
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
                enemy.Draw();
            }
            else
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Counter attack by {secondActor}, {actorDices[2]} with result: {dmgNumbers[2]}.");
                Console.WriteLine($"{firstActor} defended with {actorDices[1]}, result: {dmgNumbers[1]}.");
                Console.WriteLine($"Counter attack by {secondActor} against {firstActor} did {dmgNumbers[5]}.");
                if (player.IsDead == true)
                {
                    player.GameOver();
                }
            }
        }
        else
        {
            for (int i = 0; i < 12; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("                                                         ");
            }
            Console.SetCursorPosition(0, 2);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{secondActor} encountered.");
            Console.WriteLine();
            Console.WriteLine($"{firstActor} rolled {actorDices[2]} to attack, result: {dmgNumbers[2]}.");
            Console.WriteLine($"{secondActor} defended using {actorDices[1]}, result: {dmgNumbers[1]}.");
            Console.WriteLine($"Damage done by {firstActor} to {secondActor} is: {dmgNumbers[5]}.");
            if (player.IsDead == true)
            {
                player.GameOver();
            }
            else
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Counter attack by {secondActor}, {actorDices[0]} with result: {dmgNumbers[0]}.");
                Console.WriteLine($"{firstActor} defended with {actorDices[3]}, result: {dmgNumbers[3]}.");
                Console.WriteLine($"Counter attack by {secondActor} against {firstActor} did {dmgNumbers[4]}.");
            }
            if (enemy.IsDead == true)
            {
                Console.WriteLine();
                Console.WriteLine($"{firstActor} has been slain.");
                enemy.Draw();
            }
        }
    }

    public static void EquipmentPickup(Player player, Equipment equipment)
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
                break;
            case "Magic Armor":
                Console.SetCursorPosition(0, 10);
                Console.WriteLine($"The {equipment.Name} have been acquired.     ");
                Console.WriteLine("Defense have been increased to 2D8+2.");
                player.defenseDices = equipment.DefenseDice;
                player.defDiceSides = equipment.DefDiceSides;
                player.defDiceModifier = equipment.DefDiceModifier;
                equipment.IsDead = true;
                break;
        }
    }
    public static void ItemPickup(Player player, Items item)
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
                    item.IsDead = true;
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
                    item.IsDead = true;
                    break;
                }
        }
    }
}