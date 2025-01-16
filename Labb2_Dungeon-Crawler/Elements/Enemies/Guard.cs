﻿class Guard : Enemy
{
    public Guard(int x, int y)
    {
        Position = (x, y);
        objectTile = 'G';
        objectColor = ConsoleColor.Magenta;
        Name = "Guard";
        PointModifier = 2;
        MaxHealth = 50;
        CurrentHealth = 50;
        DmgDice = 2;
        dmgDiceSides = 6;
        dmgDiceModifier = -1;
        DefDice = 2;
        defDiceSides = 4;
        defDiceModifier = 0;
        IsDead = false;
        IsVisible = false;
        this.Draw();
    }

    public Guard()
    {

    }

    public override void Update(List<LevelElements> elements)
    {

        IsVisible = false;

        Console.SetCursorPosition(Position.Item1, Position.Item2);
        Draw();

    }
}
