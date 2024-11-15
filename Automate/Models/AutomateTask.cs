using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using Automate.Interfaces;

namespace Automate.Models
{
    public class AutomateTask : IAutomateTask
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("Name")]
        public string? Name { get; set; }

        [BsonElement("Date")]
        public DateTime? Date { get; set; }

        [BsonElement("Important")]
        public bool Important { get; set; }

        public AutomateTask(DateTime date, string name)
        {
            Name = name;

            Date = date;

            if (name == "Arrosage" || name == "Semis")
                Important = true;
            else
                Important = false;
        }
    }
    
}
