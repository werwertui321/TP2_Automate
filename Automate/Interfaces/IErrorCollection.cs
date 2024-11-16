using System.Collections.Generic;

namespace Automate.Interfaces
{
    public interface IErrorCollection
    {
        abstract Dictionary<string, List<string>> Errors { get; set; }

        void AddError(string propertyName, string errorMessage);

        void RemoveError(string propertyName);

        string FormatErrorListIntoSingleString();
    }
}
