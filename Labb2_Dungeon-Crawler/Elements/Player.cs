class Player : LevelElements
{
    //private LevelData playerData = new LevelData();

    //public LevelData PlayerData
    //{
    //    get
    //    {
    //        return playerData;
    //    }
    //}

    public int DamageDices = 2;
    public int DiceSides = 6;
    public int DiceModifier = 1;

    public Player(int x, int y)
    {
        Position = (x, y);
        objectTile = '@';
        objectColor = ConsoleColor.Yellow;
    }

    public int Attack(int DamageDices, int DiceSides, int DiceModifier)
    {
        Dice damage = new Dice(DamageDices, DiceSides, DiceModifier);
        return damage.Throw();
    }

    public void TakeStep(int d, char dir, List<LevelElements> elements)
    {
        if (dir == 'H')
        {
            d = TileCheck(d, dir, elements);
            Console.SetCursorPosition(Position.Item1, Position.Item2);
            Position = (Position.Item1 + d, Position.Item2);
            Draw();
        }
        else
        {
            d = TileCheck(d, dir, elements);
            Console.SetCursorPosition(Position.Item1, Position.Item2);
            Position = (Position.Item1, Position.Item2 + d);
            Draw();
        }
    }
    public int TileCheck(int d, char dir, List<LevelElements> elements)
    {
        int x = 0;
        int y = 0;
        switch (dir)
        {
            case 'H':
                x = d; break;
            case 'V':
                y = d; break;
        }

        if (elements.Any(b => b.Position == (Position.Item1 + x, Position.Item2 + y)) == true)
        {
            foreach (var element in elements)
            {
                if (element.Position == (Position.Item1 + x, Position.Item2 + y))
                {
                    if (element is Rat)
                    {
                        Console.SetCursorPosition(0, 1);
                        Console.Write("                                              ");
                        Console.SetCursorPosition(0, 1);
                        Console.Write($"Rat! Damage done {Attack(DamageDices, DiceSides, DiceModifier)}");
                        return 0;
                    }
                    else if (element is Snake)
                    {
                        Console.SetCursorPosition(0, 1);
                        Console.Write("                                              ");
                        Console.SetCursorPosition(0, 1);
                        Console.Write("Snake!");
                        return 0;
                    }
                }
            }
            return 0;
        }
        else
        {
            return d;
        }
    }

    public void Movement(ConsoleKeyInfo checkKey, List<LevelElements> elements)
    {
        switch (checkKey.Key)
        {
            case ConsoleKey.RightArrow:
                {
                    TakeStep(1, 'H', elements);
                    break;
                }
            case ConsoleKey.LeftArrow:
                {
                    TakeStep(-1, 'H', elements);
                    break;
                }
            case ConsoleKey.UpArrow:
                {
                    TakeStep(-1, 'V', elements);
                    break;
                }
            case ConsoleKey.DownArrow:
                {
                    TakeStep(1, 'V', elements);
                    break;
                }
            case ConsoleKey.Spacebar:
                {
                    Draw();
                    break;
                }
            case ConsoleKey.Escape:
                {
                    Console.SetCursorPosition(0, 21);
                    Environment.Exit(0);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

}
