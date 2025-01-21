class Guard : Enemy
{
    public Guard(int x, int y)
    {
        Position = (x, y);
        ObjectTile = 'G';
        ObjectColor = ConsoleColor.Magenta;
        Name = "Guard";
        PointModifier = 2;
        MaxHealth = 50;
        CurrentHealth = 50;
        DmgDice = 2;
        DmgDiceSides = 6;
        DmgDiceModifier = -1;
        DefDice = 2;
        DefDiceSides = 4;
        DefDiceModifier = 0;
        IsDead = false;
        IsVisible = false;
        this.Draw();
    }

    public Guard()
    {

    }

    public override void Update(List<LevelElements> elements, Dictionary<int, string> combatLog, int logPosition)
    {

        IsVisible = false;

        Console.SetCursorPosition(Position.Item1, Position.Item2);
        Draw();

    }
}
