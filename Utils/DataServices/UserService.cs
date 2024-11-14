using Automate.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace Automate.Utils.Services
{
    public interface IUserService
    {
        User? Authenticate(string? username, string? password);

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

        public User? Authenticate(string? username, string? password)
        {
            var filter = Builders<User>.Filter.Eq("Username", username);
            User user = _users.Find(filter).FirstOrDefault();

            if (user == null)
                return null;

            if (!VerifyPassword(password, user.Password))
            {
                return null;
            }

            return user;
        }

        private bool VerifyPassword(string? password, string? hashedPassword) => BC.Verify(password, hashedPassword);
    }
}
