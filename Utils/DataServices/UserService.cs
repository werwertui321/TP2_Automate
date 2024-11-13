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
        UserModel Authenticate(string? username, string? password);

        void RegisterUser(UserModel user);
    }

    public class UserService : IUserService
    {
        private readonly IMongoCollection<UserModel> _users;
        private const string COLLECTION_NAME = "Users";

        public UserService()
        {
            MongoDBService database = new MongoDBService();
            _users = database.GetCollection<UserModel>(COLLECTION_NAME);
        }

        public UserModel Authenticate(string? username, string? password)
        {
            var user = _users.Find(userFromDB => userFromDB.Username == username && userFromDB.Password == password).FirstOrDefault();
            return user;
        }
        public void RegisterUser(UserModel user)
        {
            _users.InsertOne(user);
        }
    }
}
