using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;

namespace Labb2_Dungeon_Crawler.DBModel
{
    internal class SaveGameContext : DbContext
    {
        public DbSet<SaveGame> SaveGames { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "mongodb://localhost:27017";
            var collection = "AndreasLindSahlin";

            optionsBuilder.UseMongoDB(connectionString, collection);
        }
    }

    [Collection("DungeonCrawler")]
    class SaveGame
    {
        public ObjectId Id { get; set; }

        public Player? Player { get; set; }
        public Wall? Wall { get; set; }
        public Rat? Rat { get; set; }
        public Guard? Guard { get; set; }
        public Snake? Snake { get; set; }
        public Boss? Boss { get; set; }
        public Sword? Sword { get; set; }
        public Armor? Armor { get; set; }
        public Food? Food { get; set; }
        public Potion? Potion { get; set; }
        public Grue? Grue { get; set; }
        public DateTime SaveDate { get; set; }
    }
}
