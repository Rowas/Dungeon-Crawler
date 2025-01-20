using MongoDB.Bson;

namespace Dungeon_Crawler.DBModel
{
    class LogMessage
    {
        public ObjectId Id { get; set; }
        public List<int> Key { get; set; } = new List<int>();
        public List<string> Message { get; set; } = new List<string>();

    }
}
