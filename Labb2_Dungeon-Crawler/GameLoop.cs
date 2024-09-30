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

            foreach (LevelElements element in Level1.Elements)
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
}