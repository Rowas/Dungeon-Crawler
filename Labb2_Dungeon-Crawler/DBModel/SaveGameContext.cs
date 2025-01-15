using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;

namespace Labb2_Dungeon_Crawler.DBModel
{
    internal class SaveGameContext : DbContext
    {
        public DbSet<GameSave> SaveGames { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "mongodb://localhost:27017";
            var collection = "AndreasLindSahlin";

            Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;

            optionsBuilder.UseMongoDB(connectionString, collection);
        }
    }

    [Collection("DungeonCrawlerSaveGames")]
    class GameSave
    {
        public ObjectId Id { get; set; }
        public DateTime SaveDate { get; set; } = DateTime.Now;
        public string PlayerName { get; set; }
        public GameState gameState { get; set; }

    }

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
