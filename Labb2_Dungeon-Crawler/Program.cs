//Labb2 - Dungeon Crawler
//Labb for C# programming in .NET-developer edu program on ITHS 2024.
using System.IO.Enumeration;
internal class Program
{
    private static void Main(string[] args)
    {
        GameLoop start = new GameLoop();
        start.StartUp();
        start.GameRunning();
    }
}