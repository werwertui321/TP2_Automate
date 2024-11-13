using Automate.Models;
using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Automate.Utils.Services
{
    public interface ICalendarService
    {
        public List<Models.TaskModel> GetTasksByDate(DateTime? date);

        public void AddTask(Models.TaskModel task);
        
        public void UpdateTask(string newTaskDescription, string taskId);

        public void DeleteTask(string taskId);
    }
    public class CalendarService : ICalendarService
    {
        private readonly IMongoCollection<TaskModel> _tasks;
        private const string COLLECTION_NAME = "Tasks";
        private readonly MongoDBService database;

        public CalendarService(MongoDBService database)
        {
            this.database = database;
            _tasks = database.GetCollection<TaskModel>(COLLECTION_NAME);
        }

        public List<TaskModel> GetTasksByDate(DateTime? date)
        {
            var filter = Builders<TaskModel>.Filter.Eq("Date", date);
            var tasks = _tasks.Find(filter).ToList();
            return tasks;
        }

        public void AddTask(TaskModel task)
        {
            _tasks.InsertOne(task);
        }

        public void UpdateTask(string newTaskDescription, string taskId)
        {
            var filter = Builders<TaskModel>.Filter.Eq("_id", taskId);
            var update = Builders<TaskModel>.Update.Set("Description", newTaskDescription);
            _tasks.UpdateOne(filter, update);
        }

        public void DeleteTask(string taskId)
        {
            var filter = Builders<TaskModel>.Filter.Eq("_id", taskId);
            _tasks.DeleteOne(filter);
        }
    }
}
