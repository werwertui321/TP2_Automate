﻿using Automate.Models;
using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace Automate.Utils.Services
{
    public interface ICalendarService
    {
        public List<TaskModel> GetTasksByDate(DateTime? date);

        public void AddTask(TaskModel task);
        
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
            try
            {
                if (date is null)
                    throw new ArgumentException("la date ne peut pas être null.");

                var filter = Builders<TaskModel>.Filter.Eq("Date", date);
                var tasks = _tasks.Find(filter).ToList();
                return tasks;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message,
                   "Erreur",
                   MessageBoxButton.OK,
                   MessageBoxImage.Error);
                return new List<TaskModel>();
            }
        }

        public void AddTask(TaskModel task)
        {
            try
            {
                if(task is null)
                    throw new ArgumentException("La tâche ne peut pas être null");

                if (task.Date is null)
                    throw new ArgumentException("La date de la tâche ne doit pas être null.");

                if (string.IsNullOrEmpty(task.Description))
                    throw new ArgumentException("La description de la tâche ne doit pas être null.");

                _tasks.InsertOne(task);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message,
                   "Erreur",
                   MessageBoxButton.OK,
                   MessageBoxImage.Error);
            }
        }

        public void UpdateTask(string newTaskDescription, string taskId)
        {
            try
            {
                if (string.IsNullOrEmpty(taskId))
                    throw new ArgumentException("L'identifiant de la tâche à modifier ne doit pas être vide");

                if (string.IsNullOrEmpty(newTaskDescription))
                    throw new ArgumentException("La nouvelle tâche ne doit pas être vide.");

                var filter = Builders<TaskModel>.Filter.Eq("_id", taskId);
                var update = Builders<TaskModel>.Update.Set("Description", newTaskDescription);
                _tasks.UpdateOne(filter, update);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message,
                    "Erreur",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public void DeleteTask(string taskId)
        {
            try
            {
                if (string.IsNullOrEmpty(taskId))
                    throw new ArgumentException("L'identifiant de la tâche à supprimer ne doit pas être vide");

                var filter = Builders<TaskModel>.Filter.Eq("_id", taskId);
                _tasks.DeleteOne(filter);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message,
                   "Erreur",
                   MessageBoxButton.OK,
                   MessageBoxImage.Error);
            }
        }
    }
}