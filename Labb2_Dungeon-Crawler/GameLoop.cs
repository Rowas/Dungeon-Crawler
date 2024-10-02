using System.Runtime.CompilerServices;

class GameLoop
{
    private int turnCounter = 0;
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
            Console.SetCursorPosition(0, 0);
            Console.Write($"Turn: {turnCounter++}");

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
                    player.Movement(checkKey, Level1.Elements);
                }
                else if (element is Enemy)
                {
                    Enemy enemy = (Enemy)element;
                    enemy.Update(Level1.Elements);
                }
            }
        } while (checkKey.Key != ConsoleKey.Escape);
    }

    public static void Encounter(Player player, Enemy enemy)
    {
        GameLoop gameLoop = new GameLoop();
        (int, int) pResults = player.DamageDefenseRolls();
        int eDmg = enemy.Attack();
        int eDef = enemy.Defend();
        int pDmgDone = pResults.Item1 - eDef;
        int eDmgDone = eDmg - pResults.Item2;

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
        else if (eDmgDone < 0 && enemy.IsDead == false)
        {
            player.currentHealth = player.currentHealth - eDmgDone;
        }

        Console.SetCursorPosition(0, 1);
        Console.Write(new String(' ', Console.BufferWidth + 5));
        Console.SetCursorPosition(0, 1);
        Console.WriteLine($"{enemy.Name} encountered! \nDamage done to {enemy.Name} using 2D6+1 is: {pResults.Item1}, {enemy.Name} defended with 2D4+1: {eDef}. Final damage is {pDmgDone}");
        if (enemy.IsDead == true)
        {
            Console.SetCursorPosition(0, 3);
            Console.Write(new String(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, 3);
            Console.WriteLine("The Enemy have been slain.");
        }
        else
        {
            Console.SetCursorPosition(0, 3);
            Console.Write(new String(' ', Console.BufferWidth + 5));
            Console.SetCursorPosition(0, 3);
            Console.WriteLine($"Damage done to {player.Name} by {enemy.Name} using 1D6 is: {eDmg}, You defended with {pResults.Item2}. Final damage is {eDmgDone}");
        }
    }
}