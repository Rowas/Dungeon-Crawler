using System.Numerics;

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
