using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;

namespace Dungeon_Crawler.DBModel
{
    [Collection("DungeonCrawlerSaveGames")]
    class GameSave
    {
        public ObjectId Id { get; set; }
        public DateTime SaveDate { get; set; } = DateTime.Now;
        public string MapName { get; set; }
        public string PlayerName { get; set; }
        public GameState gameState { get; set; }

    }
}
