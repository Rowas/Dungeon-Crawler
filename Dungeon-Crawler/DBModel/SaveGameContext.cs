﻿using Microsoft.EntityFrameworkCore;

namespace Dungeon_Crawler.DBModel
{
    internal class SaveGameContext : DbContext
    {
        public DbSet<GameSave> SaveGames { get; set; }
        public DbSet<Highscore> Highscores { get; set; }
        public DbSet<CombatLog> CombatLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // ?serverSelectionTimeoutMS=10000
            var connectionString = "mongodb://localhost:27017";
            var collection = "AndreasLindSahlin";

            optionsBuilder.UseMongoDB(connectionString, collection);
        }
    }


}
