﻿using System.Numerics;
using System.Reflection.Metadata;

class Player : LevelElements
{
    public string Name = "Adventurer";
    public int maxHealth = 100;
    public int currentHealth = 100;

    public int damageDices = 2;
    public int dmgDiceSides = 6;
    public int dmgDiceModifier = 1;

    public int defenseDices = 1;
    public int defDiceSides = 8;
    public int defDiceModifier = 0;

    public bool IsDead = false;


    public Player(int x, int y)
    {
        IsVisible = true;
        Position = (x, y);
        objectTile = '@';
        objectColor = ConsoleColor.Yellow;
    }

    public void Movement(ConsoleKeyInfo checkKey, List<LevelElements> elements)
    {
        switch (checkKey.Key)
        {
            case ConsoleKey.RightArrow:
                {
                    DistanceCheck(elements);
                    TakeStep(1, 'H', elements);
                    break;
                }
            case ConsoleKey.LeftArrow:
                {
                    DistanceCheck(elements);
                    TakeStep(-1, 'H', elements);
                    break;
                }
            case ConsoleKey.UpArrow:
                {
                    DistanceCheck(elements);
                    TakeStep(-1, 'V', elements);
                    break;
                }
            case ConsoleKey.DownArrow:
                {
                    DistanceCheck(elements);
                    TakeStep(1, 'V', elements);
                    break;
                }
            case ConsoleKey.Spacebar:
                {
                    DistanceCheck(elements);
                    Draw();
                    break;
                }
            case ConsoleKey.Escape:
                {
                    Console.SetCursorPosition(0, 21);
                    Environment.Exit(0);
                    break;
                }
            default:
                {
                    break;
                }
        }
        Console.ResetColor();
    }

    public void DistanceCheck(List<LevelElements> elements)
    {
        for (int i = -5; i < 6; i++)
        {
            for (int j = -5; j < 6; j++)
            {
                if (elements.Any(b => b.Position == (Position.Item1 + i, Position.Item2 + j)) == true)
                {
                    foreach (var element in elements)
                    {
                        if (element.Position == (Position.Item1 + i, Position.Item2 + j))
                        {
                            if (element is Wall)
                            {
                                Wall wall = (Wall)element;
                                if (wall.IsVisible == false)
                                {
                                    wall.IsVisible = true;
                                    wall.DrawWall();
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else if (element is Enemy)
                            {
                                Enemy enemy = (Enemy)element;
                                if (enemy.IsVisible == false)
                                {
                                    enemy.IsVisible = true;
                                    enemy.Draw();
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void TakeStep(int d, char dir, List<LevelElements> elements)
    {
        if (dir == 'H')
        {
            
            d = TileCheck(d, dir, elements);
            Console.SetCursorPosition(Position.Item1, Position.Item2);
            Position = (Position.Item1 + d, Position.Item2);
            Draw();

        }
        else
        {
            d = TileCheck(d, dir, elements);
            Console.SetCursorPosition(Position.Item1, Position.Item2);
            Position = (Position.Item1, Position.Item2 + d);
            Draw();
        }
    }
    public int TileCheck(int d, char dir, List<LevelElements> elements)
    {
        int x = 0;
        int y = 0;
        switch (dir)
        {
            case 'H':
                x = d; break;
            case 'V':
                y = d; break;
        }

        if (elements.Any(b => b.Position == (Position.Item1 + x, Position.Item2 + y)) == true)
        {
            foreach (var element in elements)
            {
                if (element.Position == (Position.Item1 + x, Position.Item2 + y))
                {
                    if (element is Rat)
                    {
                        GameLoop.Encounter(this, (Rat)element, 'P');
                        return 0;
                    }
                    else if (element is Snake)
                    {
                        GameLoop.Encounter(this, (Snake)element, 'P');
                        return 0;
                    }
                }
            }
            return 0;
        }
        else
        {
            return d;
        }
    }

    public int Attack()
    {
        Dice playerDamage = new Dice(damageDices, dmgDiceSides, dmgDiceModifier);
        int pDmg = playerDamage.Throw();

        return pDmg;
    }

    public int Defend()
    {
        Dice playerDefense = new Dice(defenseDices, defDiceSides, defDiceModifier);
        int pDef = playerDefense.Throw();

        return pDef;
    }

    public void GameOver()
    {
        this.objectTile = ' ';
        Draw();
        Console.Clear();
        Console.SetCursorPosition(33, 12);
        Console.WriteLine("It's a sad thing that your adventures have ended here!!");
        Environment.Exit(0);
    }
}
