using MongoDB.Driver;

namespace Automate.Utils.DataServices
{
    public class MongoDBService
    {
        private readonly IMongoDatabase _database;
        private const string MONGO_URL = "mongodb://localhost:27017";
        private const string DB_NAME = "AutomateDB";

        public MongoDBService()
        {
            var client = new MongoClient(MONGO_URL);
            _database = client.GetDatabase(DB_NAME);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
    }

}
