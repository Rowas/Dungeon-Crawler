using Labb2_Dungeon_Crawler.Elements;
using Labb2_Dungeon_Crawler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb2_Dungeon_Crawler
{
    internal class GameLoop
    {
        public GameLoop()
        {
            Movement();
        }

        public void Movement()
        {
            Console.SetCursorPosition(0, 20);
            ConsoleKeyInfo checkKey;
            Console.WriteLine("Move with arrow-keys on the keyboard. \nPressing Esc will cancel the program.");
            //int x = LevelData
            //int y = player.Position.Y;
            int x = 4;
            int y = 3;
            Console.SetCursorPosition(x, y);
            Console.CursorVisible = false;
            do
            {
                while (Console.KeyAvailable == false)
                {
                    Thread.Sleep(25);
                }
                checkKey = Console.ReadKey(true);
                switch (checkKey.Key)
                {
                    case ConsoleKey.RightArrow:
                        if (x != 51)
                        {
                            Console.SetCursorPosition(x++, y);
                            Console.Write(" ");
                            Console.SetCursorPosition(x, y);
                            Console.Write(new Player());
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (x != 1)
                        {
                            Console.SetCursorPosition(x--, y);
                            Console.Write(" ");
                            Console.SetCursorPosition(x, y);
                            Console.Write(new Player());
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (y != 1)
                        {
                            Console.SetCursorPosition(x, y--);
                            Console.Write(" ");
                            Console.SetCursorPosition(x, y);
                            Console.Write(new Player());
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (y != 16)
                        {
                            Console.SetCursorPosition(x, y++);
                            Console.Write(" ");
                            Console.SetCursorPosition(x, y);
                            Console.Write(new Player());
                        }
                        break;
                }
            } while (checkKey.Key != ConsoleKey.Escape);
            Console.SetCursorPosition(0, 23);
            Console.WriteLine("Goodbye!");
        }
    }
}
