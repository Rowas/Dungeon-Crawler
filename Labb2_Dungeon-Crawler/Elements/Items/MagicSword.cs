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
        dmgDiceSides = 10;
        dmgDiceModifier = 2;
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

        DistanceCheck(elements);

        Console.SetCursorPosition(Position.Item1, Position.Item2);
        Draw();

    }
}
