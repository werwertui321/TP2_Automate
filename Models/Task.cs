using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automate.Models
{
    public class Task
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("Description")]
        public string? Description { get; set; }

        [BsonElement("Date")]
        public DateTime? Date { get; set; }
    }
    
}
