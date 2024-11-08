using Automate.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automate.Utils
{
    public class MongoDBService
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<UserModel> _users;
        const string MONGO_URL = "mongodb://localhost:27017";

        public MongoDBService(string databaseName)
        {
            var client = new MongoClient(MONGO_URL);
            _database = client.GetDatabase(databaseName);
            _users = _database.GetCollection<UserModel>("Users");
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName) 
        {
            return _database.GetCollection<T>(collectionName);
        }

        public UserModel Authenticate(string? username, string? password)
        {
            var user = _users.Find(u => u.Username == username && u.Password == password).FirstOrDefault();
            return user;
        }
        public void RegisterUser(UserModel user)
        {
            _users.InsertOne(user);
        }

    }

}
