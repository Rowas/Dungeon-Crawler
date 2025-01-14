using MongoDB.Bson.Serialization.Attributes;

abstract class LevelElements
{
    [BsonIgnore]
    public (int, int) Position { get; set; }
    public int xPos { get; set; }
    public int yPos { get; set; }
    public char objectTile { get; set; }
    public ConsoleColor objectColor { get; set; }
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
                Console.ForegroundColor = objectColor;
                Console.Write(objectTile);
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
                Console.ForegroundColor = objectColor;
                Console.Write(objectTile);
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
            Console.ForegroundColor = (ConsoleColor)objectColor;
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Write(objectTile);
            Console.ResetColor();
        }
    }
}
