using System.Collections.Generic;

namespace Automate.Interfaces
{
    public interface IErrorCollection
    {
        void AddError(string propertyName, string errorMessage);

        void RemoveError(string propertyName);

        string FormatErrorList(Dictionary<string, List<string>> errors);
    }
}
