class Sword : Equipment
{
    public Sword(int x, int y)
    {
        Position = (x, y);
        ObjectTile = 'W';
        ObjectColor = ConsoleColor.DarkYellow;
        Name = "Magic Sword";
        PointModifier = -1;
        DamageDices = 2;
        DmgDiceSides = 10;
        DmgDiceModifier = 2;
        IsDead = false;
        this.Draw();
    }

    public Sword()
    {

    }

    public override void Update(List<LevelElements> elements)
    {

        IsVisible = false;

        Console.SetCursorPosition(Position.Item1, Position.Item2);
        Draw();

    }
}
