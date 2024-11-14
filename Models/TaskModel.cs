using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automate.Models
{
    public class TaskModel
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("Name")]
        public string? Name { get; set; }

        [BsonElement("Date")]
        public DateTime? Date { get; set; }

        [BsonElement("Important")]
        public bool Important { get; set; }

        public TaskModel(DateTime date, string name)
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
