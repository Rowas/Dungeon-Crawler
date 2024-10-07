using System.Numerics;

class Armor : Equipment
{
    public Armor(int x, int y)
    {
        Position = (x, y);
        objectTile = 'A';
        objectColor = ConsoleColor.DarkCyan;
        Name = "Magic Armor";
        DefenseDice = 2;
        defDiceSides = 8;
        defDiceModifier = 2;
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
