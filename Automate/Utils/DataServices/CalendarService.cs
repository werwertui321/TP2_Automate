using Automate.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using Automate.Interfaces;

namespace Automate.Utils.DataServices
{
    public class CalendarService : ICalendarService
    {
        private readonly IMongoCollection<AutomateTask> _tasks;
        private const string COLLECTION_NAME = "Tasks";
        private readonly MongoDBService database;

        public CalendarService(MongoDBService database)
        {
            this.database = database;
            _tasks = database.GetCollection<AutomateTask>(COLLECTION_NAME);
        }

        public List<AutomateTask>? GetTasksByDate(DateTime? date)
        {
            if (date is null)
                return null;

            var filter = Builders<AutomateTask>.Filter.Eq("Date", date);
            var tasks = _tasks.Find(filter).ToList();
            return tasks;
        }

        public void AddTask(AutomateTask task)
        {
            if (task is null)
                throw new ArgumentException("La tâche ne peut pas être null");

            if (task.Date is null)
                throw new ArgumentException("La date de la tâche ne doit pas être null.");

            if (string.IsNullOrEmpty(task.Name))
                throw new ArgumentException("Le type d'évènement ne doit pas être null.");

            _tasks.InsertOne(task);
        }

        public void UpdateTask(string newTaskName, ObjectId taskId)
        {
            if (taskId == ObjectId.Empty)
                throw new ArgumentException("L'identifiant de la tâche à modifier ne doit pas être vide");

            if (string.IsNullOrEmpty(newTaskName))
                throw new ArgumentException("La nouveau type d'évènement ne doit pas être vide.");

            bool important;
            if (newTaskName == "Arrosage" || newTaskName == "Semis")
                important = true;
            else
                important = false;

            var filter = Builders<AutomateTask>.Filter.Eq("_id", taskId);
            var update = Builders<AutomateTask>.Update.Set("Name", newTaskName).Set("Important", important);
            _tasks.UpdateOne(filter, update);
        }

        public void DeleteTask(ObjectId taskId)
        {
            if (taskId == ObjectId.Empty)
                throw new ArgumentException("L'identifiant de la tâche à supprimer ne doit pas être vide");

            var filter = Builders<AutomateTask>.Filter.Eq("_id", taskId);
            _tasks.DeleteOne(filter);
        }
    }
}
