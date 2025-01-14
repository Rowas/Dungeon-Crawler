class Potion : Items
{
    public Potion(int x, int y)
    {
        Position = (x, y);
        objectTile = 'P';
        objectColor = ConsoleColor.DarkGreen;
        Name = "Potion";
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
