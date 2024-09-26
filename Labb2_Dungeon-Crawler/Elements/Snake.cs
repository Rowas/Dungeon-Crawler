using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb2_Dungeon_Crawler.Elements
{
    internal class Snake : Enemy
    {
        private char _snakeTile = 's';
        public char SnakeTile { get; }

        public Snake()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(_snakeTile);
            Console.ResetColor();
        }

        public override string ToString()
        {
            return $"{SnakeTile}";
        }
    }
}
