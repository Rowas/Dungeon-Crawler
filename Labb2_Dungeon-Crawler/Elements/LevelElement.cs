abstract class LevelElements
{
    public (int, int) Position { get; set; }
    public char objectTile { get; set; }
    public ConsoleColor objectColor { get; set; }
    public  bool isVisible {get; set; }
    public void Draw()
    {
        //if (isVisible == true)
        //{
            Console.Write(" ");
            Console.SetCursorPosition(Position.Item1, Position.Item2);
            Console.ForegroundColor = (ConsoleColor)objectColor;
            Console.Write(objectTile);
        //}
    }
    public void DrawWall()
    {
        //if (isVisible == false)
        //{
            Console.Write(" ");
            Console.SetCursorPosition(Position.Item1, Position.Item2);
            Console.ForegroundColor = (ConsoleColor)objectColor;
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write(objectTile);
        //}
    }
}
