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
        void Authenticate(string? username, string? password);

        void RegisterUser(User user);
    }

    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _users;
        private readonly MongoDBService _db;
        private const string COLLECTION_NAME = "Users";
        private const string SALT = "$2a$11$egT6RJKZt3HP.sYYf/Xdw.";

        public UserService(MongoDBService db)
        {
            _db = db;
            _users = _db.GetCollection<User>(COLLECTION_NAME);
        }

        public void Authenticate(string? username, string? password)
        {
            string hashedPassword = BC.HashPassword(password, SALT);
            Env.authenticatedUser = _users.Find(userFromDB => userFromDB.Username == username && userFromDB.Password == hashedPassword).FirstOrDefault();
        }
    }
}
