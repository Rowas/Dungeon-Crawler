using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Snake : Enemy
{
    public Snake(int x, int y)
    {
        Position = (x, y);
        objectTile = 'S';
        objectColor = ConsoleColor.Green;
        Name = "Snake";
        MaxHealth = 35;
        CurrentHealth = 35;
        DamageDices = 2;
        dmgDiceSides = 4;
        dmgDiceModifier = 1;
        //DefenseDice = 2D6-1;
        DefenseDice = 2;
        defDiceSides = 6;
        defDiceModifier = -1;
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
        //Move();
        //Attack();
        //Defend();
        //Die();
    }

}
