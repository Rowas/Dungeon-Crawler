using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Labb2_Dungeon_Crawler.Elements
{
    internal class Player : LevelElement
    {
        private char _playerTile = '@';
        public char PlayerTile { get; }     

        public Player()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(_playerTile);
            Console.ResetColor();
        }

        public override string ToString()
        {
            return $"{PlayerTile}";
        }
    }
}
