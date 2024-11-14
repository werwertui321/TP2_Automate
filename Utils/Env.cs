using Automate.Models;
using Automate.Utils.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automate.Utils
{
    public static class Env
    {
        public readonly static MongoDBService mongoDBService = new MongoDBService();
        public readonly static UserService userService = new UserService(mongoDBService);
        public readonly static CalendarService calendarService = new CalendarService(mongoDBService);
        public static User authenticatedUser;
    }
}
