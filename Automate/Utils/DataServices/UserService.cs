using Automate.Models;
using MongoDB.Driver;
using System.Linq;
using BC = BCrypt.Net.BCrypt;
using Automate.Interfaces;

namespace Automate.Utils.DataServices
{
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

            Env.authenticatedUser = user;
            return user;
        }

        private bool VerifyPassword(string? password, string? hashedPassword) => BC.Verify(password, hashedPassword);
    }
}
