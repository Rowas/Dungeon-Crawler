class Food : Items
{
    public Food(int x, int y)
    {
        Position = (x, y);
        ObjectTile = 'F';
        ObjectColor = ConsoleColor.White;
        Name = "Food";
        PointModifier = -1;
        HealthRestore = 25;
        IsDead = false;
        this.Draw();
    }
    public Food()
    {

    }

    public override void Update(List<LevelElements> elements)
    {

        IsVisible = false;

        Console.SetCursorPosition(Position.Item1, Position.Item2);
        Draw();

    }
}
