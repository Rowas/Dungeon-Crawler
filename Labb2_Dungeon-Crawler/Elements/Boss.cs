using System.Numerics;

class Boss : Enemy
{
    public Boss(int x, int y)
    {
        Position = (x, y);
        objectTile = 'B';
        objectColor = ConsoleColor.DarkRed;
        Name = "Boss";
        MaxHealth = 75;
        CurrentHealth = 75;
        DamageDices = 4;
        dmgDiceSides = 4;
        dmgDiceModifier = 0;
        DefenseDice = 2;
        defDiceSides = 8;
        defDiceModifier = -1;
        IsDead = false;
        this.Draw();
    }

    public override void Update(List<LevelElements> elements)
    {

        IsVisible = false;

        Console.SetCursorPosition(Position.Item1, Position.Item2);
        Draw();

    }

    public static void YouWin()
    {
        Console.Clear();
        Console.SetCursorPosition(53, 11);
        Console.WriteLine("Congratulations!");
        Console.SetCursorPosition(33, 12);
        Console.WriteLine("You defeated the evil dungeon boss and saved the kingdom!");
        Environment.Exit(0);

    }
}
