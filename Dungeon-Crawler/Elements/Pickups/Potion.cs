class Potion : Items
{
    public Potion(int x, int y)
    {
        Position = (x, y);
        ObjectTile = 'P';
        ObjectColor = ConsoleColor.DarkGreen;
        Name = "Potion";
        PointModifier = -2;
        HealthRestore = 50;
        IsDead = false;
        this.Draw();
    }

    public Potion()
    {

    }

    public override void Update(List<LevelElements> elements)
    {

        IsVisible = false;

        Console.SetCursorPosition(Position.Item1, Position.Item2);
        Draw();

    }
}
