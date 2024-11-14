using Automate.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automate.Utils.Services
{
    public interface IUserService
    {
        void Authenticate(string? username, string? password);

        void RegisterUser(User user);
    }

    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _users;
        private readonly MongoDBService _db;
        private const string COLLECTION_NAME = "Users";

        public UserService(MongoDBService db)
        {
            _db = db;
            _users = _db.GetCollection<User>(COLLECTION_NAME);
        }

        public void Authenticate(string? username, string? password)
        {
            Env.authenticatedUser = _users.Find(userFromDB => userFromDB.Username == username && userFromDB.Password == password).FirstOrDefault();
        }
        public void RegisterUser(User user)
        {
            _users.InsertOne(user);
        }
    }
}
