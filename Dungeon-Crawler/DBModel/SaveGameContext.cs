﻿using Microsoft.EntityFrameworkCore;

namespace Dungeon_Crawler.DBModel
{
    internal class SaveGameContext : DbContext
    {
        public DbSet<GameSave> SaveGames { get; set; }
        public DbSet<Highscore> Highscores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "mongodb://localhost:27017";
            var collection = "AndreasLindSahlin";

            Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;

            optionsBuilder.UseMongoDB(connectionString, collection);
        }
    }


}
