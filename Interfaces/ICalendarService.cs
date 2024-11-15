using Automate.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automate.Interfaces
{
    public interface ICalendarService
    {
        public List<AutomateTask>? GetTasksByDate(DateTime? date);

        public void AddTask(AutomateTask task);

        public void UpdateTask(string newTaskName, ObjectId taskId);

        public void DeleteTask(ObjectId taskId);
    }
}
