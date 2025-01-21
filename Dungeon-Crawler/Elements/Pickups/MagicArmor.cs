class Armor : Equipment
{
    public Armor(int x, int y)
    {
        Position = (x, y);
        ObjectTile = 'A';
        ObjectColor = ConsoleColor.DarkCyan;
        Name = "Magic Armor";
        PointModifier = -1;
        DefenseDice = 2;
        DefDiceSides = 8;
        DefDiceModifier = 2;
        IsDead = false;
        this.Draw();
    }

    public Armor()
    {

    }

    public override void Update(List<LevelElements> elements)
    {

        IsVisible = false;

        Console.SetCursorPosition(Position.Item1, Position.Item2);
        Draw();

    }
}
