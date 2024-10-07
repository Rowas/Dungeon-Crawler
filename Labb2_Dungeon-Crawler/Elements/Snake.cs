﻿using System;
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
        DefenseDice = 2;
        defDiceSides = 6;
        defDiceModifier = -1;
        IsDead = false;
        IsVisible = false;
        this.Draw();
    }

    public override void Update(List<LevelElements> elements)
    {

        IsVisible = false;

        Console.SetCursorPosition(Position.Item1, Position.Item2);
        Draw();
        
        PlayerCheck(elements);
    }

    public void PlayerCheck(List<LevelElements> elements)
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (elements.Any(b => b.Position == (Position.Item1 + i, Position.Item2 + j)) == true)
                {
                    foreach (var element in elements)
                    {
                        if (element.Position == (Position.Item1 + i, Position.Item2 + j))
                        {
                            if (element is Player)
                            {
                                Console.SetCursorPosition(0, 2);
                                Player player = (Player)element;
                                TakeStep(-i, -j, elements);
                            }
                        }
                    }
                }
            }
        }
    }

    public void TakeStep(int h, int v, List<LevelElements> elements)
    {
        (int, int) coords = TileCheck(h, v, elements);
        Console.SetCursorPosition(Position.Item1, Position.Item2);
        Position = (Position.Item1 + coords.Item1, Position.Item2 + coords.Item2);
        Draw();
    }

    public (int, int) TileCheck(int h, int v, List<LevelElements> elements)
    {

        if (elements.Any(b => b.Position == (Position.Item1 + h, Position.Item2 + v)) == true)
        {
            return (0, 0);
        }
        else
        {
            return (h, v);
        }
    }

}
