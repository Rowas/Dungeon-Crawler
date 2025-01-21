using MongoDB.Bson.Serialization.Attributes;

abstract class LevelElements
{
    [BsonIgnore]
    public (int, int) Position { get; set; }
    public static string SaveGameName { get; set; } = "0";
    [BsonIgnore]
    public static string CombatLogName { get; set; } = "1";
    public double PointModifier { get; set; }
    public int XPos { get; set; }
    public int YPos { get; set; }
    public char ObjectTile { get; set; }
    public ConsoleColor ObjectColor { get; set; }
    public bool IsVisible { get; set; } = false;
    public static bool GrueSpawned { get; set; }
    public void Draw()
    {
        switch (IsVisible)
        {
            case true:
                Console.SetCursorPosition(Position.Item1, Position.Item2);
                Console.Write(" ");
                Console.SetCursorPosition(Position.Item1, Position.Item2);
                Console.ForegroundColor = ObjectColor;
                Console.Write(ObjectTile);
                Console.ResetColor();
                break;
            default:
                Console.Write(" ");
                Console.SetCursorPosition(Position.Item1, Position.Item2);
                break;
        }
    }
    public void DrawPlayer()
    {
        switch (IsVisible)
        {
            case true:
                Console.Write(" ");
                Console.SetCursorPosition(Position.Item1, Position.Item2);
                Console.ForegroundColor = ObjectColor;
                Console.Write(ObjectTile);
                Console.ResetColor();
                break;
            default:
                Console.Write(" ");
                Console.SetCursorPosition(Position.Item1, Position.Item2);
                break;
        }
    }
    public void DrawWall()
    {
        if (this.IsVisible == true)
        {
            Console.SetCursorPosition(Position.Item1, Position.Item2);
            Console.ForegroundColor = (ConsoleColor)ObjectColor;
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Write(ObjectTile);
            Console.ResetColor();
        }
    }
}
