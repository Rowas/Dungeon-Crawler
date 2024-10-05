using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Guard : Enemy
{
    public Guard(int x, int y)
    {
        Position = (x, y);
        objectTile = 'G';
        objectColor = ConsoleColor.Magenta;
        Name = "Guard";
        MaxHealth = 50;
        CurrentHealth = 50;
        DamageDices = 2;
        dmgDiceSides = 6;
        dmgDiceModifier = -1;
        DefenseDice = 2;
        defDiceSides = 4;
        defDiceModifier = 0;
        IsDead = false;
        IsVisible = false;
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
