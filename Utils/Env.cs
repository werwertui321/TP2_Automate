using Automate.Models;
using Automate.Utils.Services;

namespace Automate.Utils
{
    public static class Env
    {
        public readonly static MongoDBService mongoDBService = new MongoDBService();
        public readonly static UserService userService = new UserService(mongoDBService);
        public readonly static CalendarService calendarService = new CalendarService(mongoDBService);
        public static User? authenticatedUser;
    }
}
