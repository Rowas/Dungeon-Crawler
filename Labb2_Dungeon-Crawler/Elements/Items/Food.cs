using System.Numerics;

class Food : Items
{
    public Food(int x, int y)
    {
        Position = (x, y);
        objectTile = 'F';
        objectColor = ConsoleColor.White;
        Name = "Food";
        HealthRestore = 25;
        IsDead = false;
    }

    public override void Update(List<LevelElements> elements)
    {
        if (this.IsDead == true)
        {
            objectTile = ' ';
            Draw();
            Die(elements);
        }

        IsVisible = false;

        Console.SetCursorPosition(Position.Item1, Position.Item2);
        Draw();

    }
}
