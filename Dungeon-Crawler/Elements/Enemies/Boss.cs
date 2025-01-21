class Boss : Enemy
{
    public Boss(int x, int y)
    {
        Position = (x, y);
        ObjectTile = 'B';
        ObjectColor = ConsoleColor.DarkRed;
        Name = "Boss";
        PointModifier = 6;
        MaxHealth = 75;
        CurrentHealth = 75;
        DmgDice = 4;
        DmgDiceSides = 4;
        DmgDiceModifier = 0;
        DefDice = 2;
        DefDiceSides = 8;
        DefDiceModifier = -1;
        IsDead = false;
        this.Draw();
    }
    public Boss()
    {

    }

    public override void Update(List<LevelElements> elements, Dictionary<int, string> combatLog, int logPosition)
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
