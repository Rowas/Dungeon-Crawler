using Dungeon_Crawler.GeneralMethods;

class Rat : Enemy
{
    public Rat(int x, int y)
    {
        Position = (x, y);
        ObjectTile = 'R';
        ObjectColor = ConsoleColor.Red;
        Name = "Rat";
        PointModifier = 1;
        MaxHealth = 20;
        CurrentHealth = 20;
        DmgDice = 1;
        DmgDiceSides = 6;
        DmgDiceModifier = 0;
        DefDice = 2;
        DefDiceSides = 4;
        DefDiceModifier = 1;
        IsDead = false;
        this.Draw();
    }

    public Rat()
    {

    }

    public override void Update(List<LevelElements> elements, Dictionary<int, string> combatLog, int logPosition)
    {
        IsVisible = false;

        int rand = new Random().Next(1, 5);
        switch (rand)
        {
            case 1:
                TakeStep(1, 'H', elements, combatLog, logPosition);
                break;
            case 2:
                TakeStep(-1, 'H', elements, combatLog, logPosition);
                break;
            case 3:
                TakeStep(-1, 'V', elements, combatLog, logPosition);
                break;
            case 4:
                TakeStep(1, 'V', elements, combatLog, logPosition);
                break;
        }
        Console.ResetColor();
    }

    public void TakeStep(int d, char direction, List<LevelElements> elements, Dictionary<int, string> combatLog, int logPosition)
    {
        switch (direction)
        {
            case 'H':
                d = TileCheck(d, direction, elements, combatLog, logPosition);
                Console.SetCursorPosition(Position.Item1, Position.Item2);
                Position = (Position.Item1 + d, Position.Item2);
                Draw();
                break;
            default:
                d = TileCheck(d, direction, elements, combatLog, logPosition);
                Console.SetCursorPosition(Position.Item1, Position.Item2);
                Position = (Position.Item1, Position.Item2 + d);
                Draw();
                break;
        }
    }

    public int TileCheck(int d, char direction, List<LevelElements> elements, Dictionary<int, string> combatLog, int logPosition)
    {
        CombatMethods combat = new();
        int x = 0;
        int y = 0;
        switch (direction)
        {
            case 'H':
                x = d; break;
            case 'V':
                y = d; break;
        }

        if (elements.Any(b => b.Position == (Position.Item1 + x, Position.Item2 + y)) == true)
        {
            foreach (var element in from element in elements
                                    where element.Position == (Position.Item1 + x, Position.Item2 + y)
                                    where element is Player
                                    select element)
            {
                combat.Encounter((Player)element, this, 'E', elements, combatLog, logPosition);
                return 0;
            }

            return 0;
        }
        else
        {
            return d;
        }
    }
}
