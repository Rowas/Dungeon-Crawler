using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;

namespace Dungeon_Crawler.DBModel
{
    [Collection("CombatLogs")]
    internal class CombatLog
    {
        public ObjectId Id { get; set; }
        public string PlayerName { get; set; }
        public string MapName { get; set; }
        public DateTime SaveDate { get; set; } = DateTime.Now;
        public LogMessage SavedCombatLog { get; set; }
    }
}
