using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;

namespace Dungeon_Crawler.DBModel
{
    [Collection("Highscore")]
    class Highscore
    {
        public ObjectId Id { get; set; }
        public string PlayerName { get; set; }
        public string MapName { get; set; }
        public DateTime SaveDate { get; set; } = DateTime.Now;
        public double Score { get; set; }
    }
}
