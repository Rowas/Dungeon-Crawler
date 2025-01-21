class Grue : Enemy
{
    public static bool IsWarned { get; set; }
    public Grue(int x, int y)
    {
        var rand = new Random();
        rand.NextDouble();
        if (rand.NextDouble() < 0.1)
        {
            GrueSpawned = true;
        }
        Position = (x, y);
        ObjectTile = 'g';
        ObjectColor = ConsoleColor.Magenta;
        Name = "Grue";
        PointModifier = 42;
        MaxHealth = 250;
        CurrentHealth = 250;
        DmgDice = 4;
        DmgDiceSides = 20;
        DmgDiceModifier = 6;
        DefDice = 4;
        DefDiceSides = 6;
        DefDiceModifier = 3;
        IsDead = false;
        IsVisible = false;
        this.Draw();
        IsWarned = false;
    }

    public Grue()
    {

    }

    public override void Update(List<LevelElements> elements, Dictionary<int, string> combatLog, int logPosition)
    {
        if (GrueSpawned == true)
        {
            IsVisible = false;

            Console.SetCursorPosition(Position.Item1, Position.Item2);
            Draw();
        }
        else
        {
            Die(elements);
        }
    }

    public static void Warning()
    {
        if (IsWarned == false)
        {
            Console.SetCursorPosition(0, 27);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("It's dark in here ... I hope I don't get eaten by a Grue...");
            Console.ResetColor();
            IsWarned = true;
        }
    }
}