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

    public void StartUp()
    {
        Console.CursorVisible = false;
        Console.CursorTop = 0;
        Console.CursorLeft = 0;

        Directory.SetCurrentDirectory(".\\Levels\\");

        level1.Load("level1.txt");

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
        Console.WriteLine("Use arrow keys to move, space to wait, and escape to exit.");
        Console.WriteLine();
        do
        {
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

        int pDmg = player.Attack();
        int pDef = player.Defend();
        int eDmg = enemy.Attack();
        int eDef = enemy.Defend();
        int pDmgDone = pDmg - eDef;
        int eDmgDone = eDmg - pDef;

        if (pDmgDone < 1)
        {
            pDmgDone = 0;
        }
        else if (pDmgDone > 1)
        {
            enemy.CurrentHealth = enemy.CurrentHealth - pDmgDone;

            if (enemy.CurrentHealth < 1)
            {
                enemy.IsDead = true;
            }
        }

        if (eDmgDone < 1)
        {
            eDmgDone = 0;
        }
        else if (eDmgDone > 0 && enemy.IsDead == false)
        {
            player.currentHealth = player.currentHealth - eDmgDone;
            NewHP = player.currentHealth;
            if (player.currentHealth < 1)
            {
                player.IsDead = true;
            }
        }
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"{secondActor} encountered! \nDamage done to {secondActor} using 2D6+1 is: {pDmg}, {firstActor} defended with 2D4+1: {eDef}. Final damage is {pDmgDone}    ");
        Console.ResetColor();
        if (enemy.IsDead == true)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(new String(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, 3);
            Console.WriteLine($"The {enemy.Name} have been slain.");
            Console.ResetColor();
        }
        else if (player.IsDead == true)
        {
            player.GameOver();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(0, 3);
            Console.WriteLine($"Damage done to {secondActor} by {firstActor} using 1D6 is: {eDmg}, {firstActor} defended with {pDef}. Final damage is {eDmgDone}    ");
            Console.ResetColor();
        }
    }
}