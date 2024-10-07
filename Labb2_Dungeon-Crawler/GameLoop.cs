using System.ComponentModel.Design;
using System.Numerics;
using System.Runtime.CompilerServices;

class GameLoop
{
    private int turnCounter = 0;
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

        foreach (LevelElements element in level1.Elements)
        {
            if (element is Wall)
            {
                element.DrawWall();
                Console.ResetColor();
            }
            else if (element is Player)
            {
                Player player = (Player)element;
                element.Draw();
                CurrentHP = player.currentHealth;
                MaxHP = player.maxHealth;
                Console.ResetColor();
            }
            else
            {
                element.Draw();
                Console.ResetColor();
            }

        }
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
                }
            }
            Console.ResetColor();
            Console.SetCursorPosition(0, 0);
            Console.Write($"Player: {Name} | HP: {CurrentHP} / {MaxHP}  Turn: {turnCounter++}         ");
            while (Console.KeyAvailable == false)
            {
                Thread.Sleep(50);
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
        for (int i = 1; i < 4; i++)
        {
            Console.SetCursorPosition(0, 1);
            Console.Write(new String(' ', Console.WindowWidth));
        }
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

        int[] dmgNumbers = new int[6];

        dmgNumbers[0] = player.Attack().Item1; //Player attack roll
        dmgNumbers[1] = player.Defend().Item1; //Player defense roll
        dmgNumbers[2] = enemy.Attack().Item1; //Enemy attack roll
        dmgNumbers[3] = enemy.Defend().Item1; //Enemy defense roll
        dmgNumbers[4] = dmgNumbers[0] - dmgNumbers[3]; //Player damage done
        dmgNumbers[5] = dmgNumbers[2] - dmgNumbers[1]; //Enemy damage done

        string[] actorDices = new string[5];

        actorDices[0] = player.Attack().Item2;
        actorDices[1] = player.Defend().Item2;
        actorDices[2] = enemy.Attack().Item2;
        actorDices[3] = enemy.Defend().Item2;

        if (dmgNumbers[4] < 1)
        {
            dmgNumbers[4] = 0;
        }
        else if (dmgNumbers[4] > 1)
        {
            enemy.CurrentHealth = enemy.CurrentHealth - dmgNumbers[4];

            if (enemy.CurrentHealth < 1)
            {
                enemy.IsDead = true;
            }
        }

        if (dmgNumbers[5] < 1)
        {
            dmgNumbers[5] = 0;
        }
        else if (dmgNumbers[5] > 0 && enemy.IsDead == false)
        {
            player.currentHealth = player.currentHealth - dmgNumbers[5];
            NewHP = player.currentHealth;
            if (player.currentHealth < 1)
            {
                player.IsDead = true;
            }
        }

        PrintCombatResult(dmgNumbers, actorDices, firstActor, secondActor, player, enemy);
    }

    public static void PrintCombatResult(int[] dmgNumbers, string[] actorDices, string firstActor, string secondActor, Player player, Enemy enemy)
    {
        if (firstActor == "Adventurer")
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(63, 0);
            Console.Write($"{secondActor} encountered.          ");
            Console.SetCursorPosition(63, 1);
            Console.Write($"{firstActor} rolled {actorDices[0]} to attack, result: {dmgNumbers[0]}.            ");
            Console.SetCursorPosition(63, 2);
            Console.Write($"{secondActor} defended using {actorDices[3]}, result: {dmgNumbers[3]}.            ");
            Console.SetCursorPosition(63, 3);
            Console.Write($"Damage done by {firstActor} to {secondActor} is: {dmgNumbers[4]}.            ");
            if (enemy.IsDead == true)
            {
                Console.SetCursorPosition(63, 5);
                Console.Write($"{secondActor} has been slain.                               ");
                Console.SetCursorPosition(63, 6);
                Console.Write("                                                    ");
                Console.SetCursorPosition(63, 7);
                Console.Write("                                                    ");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(63, 5);
                Console.Write($"Counter attack by {secondActor}, {actorDices[2]} with result: {dmgNumbers[2]}.      ");
                Console.SetCursorPosition(63, 6);
                Console.Write($"{firstActor} defended with {actorDices[1]}, result: {dmgNumbers[1]}.            ");
                Console.SetCursorPosition(63, 7);
                Console.Write($"Counter attack by {secondActor} against {firstActor} did {dmgNumbers[5]}.      ");
                Console.SetCursorPosition(63, 8);
                Console.Write("                                                    ");
                if (player.IsDead == true)
                {
                    player.GameOver();
                }
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(63, 0);
            Console.Write($"{secondActor} encountered.            ");
            Console.SetCursorPosition(63, 1);
            Console.Write($"{firstActor} rolled {actorDices[2]} to attack, result: {dmgNumbers[2]}.            ");
            Console.SetCursorPosition(63, 2);
            Console.Write($"{secondActor} defended using {actorDices[1]}, result: {dmgNumbers[1]}.            ");
            Console.SetCursorPosition(63, 3);
            Console.Write($"Damage done by {firstActor} to {secondActor} is: {dmgNumbers[5]}.            ");
            if (player.IsDead == true)
            {
                player.GameOver();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(63, 5);
                Console.Write($"Counter attack by {secondActor}, {actorDices[0]} with result: {dmgNumbers[0]}.      ");
                Console.SetCursorPosition(63, 6);
                Console.Write($"{firstActor} defended with {actorDices[3]}, result: {dmgNumbers[3]}.            ");
                Console.SetCursorPosition(63, 7);
                Console.Write($"Counter attack by {secondActor} against {firstActor} did {dmgNumbers[4]}.      ");
                Console.SetCursorPosition(63, 8);
                Console.Write("                                                    ");
            }
            if (enemy.IsDead == true)
            {
                Console.SetCursorPosition(63, 8);
                Console.Write($"{firstActor} has been slain.                                  ");
            }
        }
    }

    public static void EquipmentPickup(Player player, Equipment equipment)
    {
        switch (equipment.Name)
        {
            case "Magic Sword":
                Console.SetCursorPosition(63, 5);
                Console.Write($"The {equipment.Name} have been acquired.         ");
                Console.SetCursorPosition(63, 6);
                Console.Write("Attack have been increased to 2D10+2.              ");
                player.damageDices = equipment.DamageDices;
                player.dmgDiceSides = equipment.DmgDiceSides;
                player.dmgDiceModifier = equipment.DmgDiceModifier;
                equipment.IsDead = true;
                break;
            case "Magic Armor":
                Console.SetCursorPosition(63, 5);
                Console.Write($"The {equipment.Name} have been acquired.         ");
                Console.SetCursorPosition(63, 6);
                Console.Write("Defense have been increased to 2D8+2.              ");
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
                    Console.SetCursorPosition(63, 5);
                    Console.Write($"{item.Name} acquired, HP restored with {item.HealthRestore}.              ");
                    player.currentHealth = player.currentHealth + item.HealthRestore;
                    NewHP = player.currentHealth;
                    item.IsDead = true;
                    break;
                }
            case "Potion":
                {
                    Console.SetCursorPosition(63, 5);
                    Console.Write($"{item.Name} acquired, HP restored with {item.HealthRestore}.              ");
                    player.currentHealth = player.currentHealth + item.HealthRestore;
                    NewHP = player.currentHealth;
                    item.IsDead = true;
                    break;
                }
        }
    }
}