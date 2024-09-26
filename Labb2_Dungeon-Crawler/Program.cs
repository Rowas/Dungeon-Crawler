//Labb2 - Dungeon Crawler
//Labb for C# programming in .NET-developer edu program on ITHS 2024.

using Labb2_Dungeon_Crawler;
using Labb2_Dungeon_Crawler.Elements;
using System.Numerics;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.Write(new LevelData());
        new GameLoop();
        Console.WriteLine();
    }
}