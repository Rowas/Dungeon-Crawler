namespace Dungeon_Crawler.DBModel
{
    class GameState
    {
        public int CurrentTurn { get; set; }
        public Player? Player { get; set; }
        public List<Wall> Walls { get; set; } = new List<Wall>();
        public List<Boss> Bosses { get; set; } = new List<Boss>();
        public List<Guard> Guards { get; set; } = new List<Guard>();
        public List<Rat> Rats { get; set; } = new List<Rat>();
        public List<Snake> Snakes { get; set; } = new List<Snake>();
        public List<Armor> Armors { get; set; } = new List<Armor>();
        public List<Sword> Swords { get; set; } = new List<Sword>();
        public List<Food> Foods { get; set; } = new List<Food>();
        public List<Potion> Potions { get; set; } = new List<Potion>();
        public List<Grue> Grues { get; set; } = new List<Grue>();
    }
}
