using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Labb2_Dungeon_Crawler.Elements
{
    internal class Wall : LevelElement
    {
        private char _wallTile = '█';
        public char WallTile { get; }

        public Wall()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(_wallTile);
            Console.ResetColor();
        }

        public override string ToString()
        {
            return $"{WallTile}";
        }
    }
}
