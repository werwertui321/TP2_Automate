using MongoDB.Bson;
using System;

namespace Automate.Interfaces
{
    public interface IAutomateTask
    {
        abstract ObjectId Id { get; set; }

        abstract string? Name { get; set; }

        abstract DateTime? Date { get; set; }

        abstract bool Important { get; set; }
    }
}
