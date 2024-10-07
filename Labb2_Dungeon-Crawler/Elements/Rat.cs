using System.Numerics;

class Rat : Enemy
{
    public Rat(int x, int y)
    {
        Position = (x, y);
        objectTile = 'R';
        objectColor = ConsoleColor.Red;
        Name = "Rat";
        MaxHealth = 20;
        CurrentHealth = 20;
        DamageDices = 1;
        dmgDiceSides = 6;
        dmgDiceModifier = 0;
        DefenseDice = 2;
        defDiceSides = 4;
        defDiceModifier = 1;
        IsDead = false;
        this.Draw();
    }

    public override void Update(List<LevelElements> elements)
    {

        IsVisible = false;

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
        Console.ResetColor();
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
                        GameLoop.Encounter((Player)element, this, 'E');
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
}
