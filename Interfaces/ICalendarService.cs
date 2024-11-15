using Automate.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

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
