using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb2_Dungeon_Crawler
{
    internal class Dice
    {
        private static Random random = new Random();

        public static int Roll(int sides)
        {
            return random.Next(1, sides + 1);
        }
    }
}
