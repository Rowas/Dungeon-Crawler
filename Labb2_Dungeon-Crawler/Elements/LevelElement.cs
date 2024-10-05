abstract class LevelElements
{
    public (int, int) Position { get; set; }
    public char objectTile { get; set; }
    public ConsoleColor objectColor { get; set; }
    public bool IsVisible { get; set; } = false;
    public void Draw()
    {
        if (this.IsVisible == true)
        {
        Console.Write(" ");
        Console.SetCursorPosition(Position.Item1, Position.Item2);
        Console.ForegroundColor = (ConsoleColor)objectColor;
        Console.Write(objectTile);
        Console.ResetColor();
        }
        else
        {
            Console.Write(" ");
            Console.SetCursorPosition(Position.Item1, Position.Item2);
        }
    }
    public void DrawWall()
    {
        if (this.IsVisible == true)
        {
            Console.SetCursorPosition(Position.Item1, Position.Item2);
            Console.ForegroundColor = (ConsoleColor)objectColor;
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write(objectTile);
            Console.ResetColor();
        }
            else
        {
            Console.Write(" ");
            Console.SetCursorPosition(Position.Item1, Position.Item2);
        }
    }
}
