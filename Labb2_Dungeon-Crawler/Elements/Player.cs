using System.Numerics;
using System.Reflection.Metadata;

class Player : LevelElements
{
    public string Name = "Adventurer";
    public int maxHealth = 100;
    public int currentHealth = 100;

    public int damageDices = 2;
    public int dmgDiceSides = 6;
    public int dmgDiceModifier = 1;

    public int defenseDices = 1;
    public int defDiceSides = 8;
    public int defDiceModifier = 0;

    public bool IsDead = false;

    public Player(int x, int y)
    {
        Position = (x, y);
        objectTile = '@';
        objectColor = ConsoleColor.Yellow;
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

        Player player = this;
        if (elements.Any(b => b.Position == (Position.Item1 + x, Position.Item2 + y)) == true)
        {
            foreach (var element in elements)
            {
                if (element.Position == (Position.Item1 + x, Position.Item2 + y))
                {
                    if (element is Rat)
                    {
                        Console.SetCursorPosition(0, 1);
                        Console.Write(new String(' ', Console.BufferWidth + 5));
                        Console.SetCursorPosition(0, 1);
                        GameLoop.Encounter(player, (Rat)element);
                        return 0;
                    }
                    else if (element is Snake)
                    {
                        Console.SetCursorPosition(0, 1);
                        Console.Write(new String(' ', Console.BufferWidth + 5));
                        Console.SetCursorPosition(0, 1);
                        GameLoop.Encounter(player, (Snake)element);
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

    public (int, int) DamageDefenseRolls()
    {
        Dice playerDamage = new Dice(damageDices, dmgDiceSides, dmgDiceModifier);
        Dice playerDefense = new Dice(defenseDices, defDiceSides, defDiceModifier);
        int pDmg = playerDamage.Throw();
        int pDef = playerDefense.Throw();

        return (pDmg, pDef);
    }
}
