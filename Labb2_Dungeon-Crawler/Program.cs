//Labb2 - Dungeon Crawler
//Labb for C# programming in .NET-developer edu program on ITHS 2024.

using Labb2_Dungeon_Crawler;
using System.IO.Enumeration;
internal class Program
{
    private static void Main(string[] args)
    {


        //LevelData level1 = new LevelData();
        //level1.Load("level1.txt");

        //foreach (LevelElements element in level1.elements)
        //{
        //    element.Draw();
        //}

        GameLoop start = new GameLoop();
        start.StartUp();
        start.GameRunning();
    }
}