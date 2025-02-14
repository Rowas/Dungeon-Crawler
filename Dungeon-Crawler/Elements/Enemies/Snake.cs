﻿class Snake : Enemy
{
    public Snake(int x, int y)
    {
        Position = (x, y);
        ObjectTile = 'S';
        ObjectColor = ConsoleColor.Green;
        Name = "Snake";
        PointModifier = 2;
        MaxHealth = 35;
        CurrentHealth = 35;
        DmgDice = 2;
        DmgDiceSides = 4;
        DmgDiceModifier = 1;
        DefDice = 2;
        DefDiceSides = 6;
        DefDiceModifier = -1;
        IsDead = false;
        IsVisible = false;
        this.Draw();
    }

    public Snake()
    {

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
                    foreach (var element in from element in elements
                                            where element.Position == (Position.Item1 + i, Position.Item2 + j)
                                            where element is Player
                                            select element)
                    {
                        Console.SetCursorPosition(0, 2);
                        Player player = (Player)element;
                        TakeStep(-i, -j, elements);
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

        return elements.Any(b => b.Position == (Position.Item1 + h, Position.Item2 + v)) == true ? (0, 0) : (h, v);
    }

}
