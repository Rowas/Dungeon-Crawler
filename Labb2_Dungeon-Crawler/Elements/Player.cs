class Player : LevelElements
{
    public string Name = "Adventurer";
    public int maxHealth = 100;
    public int currentHealth = 100;

    public int DamageDices = 2;
    public int dmgDiceSides = 6;
    public int dmgDiceModifier = 1;

    public int DefenseDices = 1;
    public int defDiceSides = 8;
    public int defDiceModifier = 0;

    public Player(int x, int y)
    {
        Position = (x, y);
        objectTile = '@';
        objectColor = ConsoleColor.Yellow;
    }

    public void Attack(int DamageDices, int dmgDiceSides, int dmgDiceModifier, Rat enemyType)
    {
        (int,int,int,int) results = DamageDefenseRolls(enemyType);
        int pDmgDone = results.Item1 - results.Item2;
        int eDmgDone = results.Item3 - results.Item4;
        if (pDmgDone < 1)
        {
            pDmgDone = 0;
        }
        if (eDmgDone < 1)
        {
            eDmgDone = 0;
        }
        Console.WriteLine($"{enemyType.Name} encountered! \nDamage done to Rat using 2D6+1 is: {results.Item1}, {enemyType.Name} defended with 2D4+1: {results.Item2}. Final damage is {pDmgDone}");
        Console.WriteLine($"Damage done to you by {enemyType.Name} using 1D6 is: {results.Item3}, You defended with {results.Item4}. Final damage is {eDmgDone}");
    }

    public (int, int, int, int) DamageDefenseRolls(Rat enemyType)
    {
        Dice playerDamage = new Dice(DamageDices, dmgDiceSides, dmgDiceModifier);
        Dice enemyDefense = new Dice(enemyType.DefenseDice, enemyType.defDiceSides, enemyType.defDiceModifier);
        Dice enemyDamage = new Dice(enemyType.DamageDices, enemyType.dmgDiceSides, enemyType.dmgDiceModifier);
        Dice playerDefense = new Dice(DefenseDices, defDiceSides, defDiceModifier);

        int pDmg = playerDamage.Throw();
        int eDef = enemyDefense.Throw();
        int eDmg = enemyDamage.Throw();
        int pDef = playerDefense.Throw();

        if (pDmg - eDef > 0)
        {
            enemyType.CurrentHealth = enemyType.CurrentHealth - (pDmg - eDef);
        }

        if (eDmg - pDef > 0)
        {
            currentHealth = currentHealth - (eDmg - pDef);
        }

        return (pDmg, eDef, eDmg, pDef);
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
                        Console.Write("                                                                                                          ");
                        Console.SetCursorPosition(0, 1);
                        Attack(DamageDices, dmgDiceSides, dmgDiceModifier, (Rat)element);
                        return 0;
                    }
                    else if (element is Snake)
                    {
                        Console.SetCursorPosition(0, 1);
                        Console.Write("                                                                                                          ");
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
