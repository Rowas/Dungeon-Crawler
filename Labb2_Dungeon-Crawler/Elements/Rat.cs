class Rat : Enemy
{
    public Rat(int x, int y)
    {
        Position = (x, y);
        objectTile = 'R';
        objectColor = ConsoleColor.Red;
        Name = "Rat";
        Health = 10;
        DamageDices = 1;
        DiceSides = 6;
        DiceModifier = 0;
        //DefenseDice = 2D4-1;
        IsDead = false;
    }

    public override void Update(List<LevelElements> elements)
    {
        int rand = new Random().Next(1, 5);
        switch (rand)
        {
            case 1:
                {
                    TakeStep(1, 'H', elements);
                    break;
                }
            case 2:
                {
                    TakeStep(-1, 'H', elements);
                    break;
                }
            case 3:
                {
                    TakeStep(-1, 'V', elements);
                    break;
                }
            case 4:
                {
                    TakeStep(1, 'V', elements);
                    break;
                }
        }
        //Attack();
        //Defend();
        //Die();
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
                    if (element is Player)
                    {
                        Console.SetCursorPosition(0, 2);
                        Console.Write("                                              ");
                        Console.SetCursorPosition(0, 2);
                        Console.Write($"Player! Damage done {Attack(DamageDices, DiceSides, DiceModifier)}");
                        break;
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
}
