using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Labb2_Dungeon_Crawler.Elements;
using Labb2_Dungeon_Crawler;
using System.Security.Cryptography.X509Certificates;

namespace Labb2_Dungeon_Crawler
{
    internal class LevelData
    {
        

        public LevelData()
        {
            List<Position> positions = new List<Position>();
            var fileData = System.IO.File.ReadAllBytes("Levels\\Level1.txt");
            for (int i = 0; i < fileData.Length; i++)
            {
                switch (fileData[i])
                {
                    case 35:
                        Console.Write(new Wall());
                        positions.Add(new Position(Console.GetCursorPosition().Top, Console.GetCursorPosition().Left));
                        break;
                    case 32:
                        Console.Write(" ");
                        break;
                    case 115:
                        Console.Write(new Snake());
                        break;
                    case 114:
                        Console.Write(new Rat());
                        break;
                    case 64:
                        Player player = new Player();
                        break;
                    case 13:
                        Console.Write("\n");
                        break;
                    case 88:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write("X");
                        Console.ResetColor();
                        break;
                    case 66:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write("B");
                        Console.ResetColor();
                        break;
                    case 75:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("k");
                        Console.ResetColor();
                        break;
                    default:
                        Console.Write("");
                        continue;
                }
                //Thread.Sleep(5);
            }
        }
    }
}