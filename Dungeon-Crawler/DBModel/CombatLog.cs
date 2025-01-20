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

        //public int LogKey { get; set; }
        //public string LogValue { get; set; }
    }

    class LogMessage
    {
        public ObjectId Id { get; set; }
        public List<int> Key { get; set; } = new List<int>();
        public List<string> Message { get; set; } = new List<string>();

    }
}
