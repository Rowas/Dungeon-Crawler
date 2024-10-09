
using System.Collections.Generic;
using System.Data;

class Grue : Enemy
{
    public bool IsSpawned { get; set; }
    public bool IsWarned{ get; set; }
    public Grue(int x, int y)
    {
        var rand = new Random();
        rand.NextDouble();
        if (rand.NextDouble() > 0.05)
        {
            IsSpawned = true;
        }
        Position = (x, y);
        objectTile = 'g';
        objectColor = ConsoleColor.Magenta;
        Name = "Grue";
        MaxHealth = 250;
        CurrentHealth = 250;
        DamageDices = 4;
        dmgDiceSides = 20;
        dmgDiceModifier = 6;
        DefenseDice = 4;
        defDiceSides = 6;
        defDiceModifier = 3;
        IsDead = false;
        IsVisible = false;
        this.Draw();
        IsWarned = false;
    }

    public override void Update(List<LevelElements> elements)
    {
        if (IsSpawned == true)
        {
            if (IsWarned == false)
            {
                Console.SetCursorPosition(0, 17);
                Console.WriteLine("It's dark in here ... I hope I don't get eaten by a Grue...");
                IsWarned = true;
            }
            IsVisible = false;

            Console.SetCursorPosition(Position.Item1, Position.Item2);
            Draw();
        }
        else
        {
            Die(elements);
        }
    }
}