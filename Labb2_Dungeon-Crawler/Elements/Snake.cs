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
        MaxHealth = 20;
        CurrentHealth = 20;
        //DamageDice = 2D4;
        //DefenseDice = 2D6-1;
        IsDead = false;
    }

    public override void Update(List<LevelElements> elements)
    {
        int rand = new Random().Next(1, 5);
        //Move();
        //Attack();
        //Defend();
        //Die();
    }
}
