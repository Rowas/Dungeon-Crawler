using System.Numerics;

class Sword : Equipment
{
    public Sword(int x, int y)
    {
        Position = (x, y);
        objectTile = 'W';
        objectColor = ConsoleColor.DarkYellow;
        Name = "Magic Sword";
        DamageDices = 2;
        DmgDiceSides = 10;
        DmgDiceModifier = 2;
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
