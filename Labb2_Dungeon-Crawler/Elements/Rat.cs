using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb2_Dungeon_Crawler.Elements
{
    internal class Rat : Enemy
    {
        private char _ratTile = 'r';
        public char RatTile { get; }

        public Rat()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(_ratTile);
            Console.ResetColor();
        }

        public override string ToString()
        {
            return $"{RatTile}";
        }
    }
}
