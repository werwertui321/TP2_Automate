using MongoDB.Bson;

namespace Automate.Interfaces
{
    public interface IUser
    {
        abstract ObjectId Id { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }

        abstract bool IsAdmin { get; set; }
    }
}
