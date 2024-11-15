using System.Collections.Generic;
using Automate.Interfaces;


namespace Automate.Utils.LocalServices
{
    public class ErrorCollection : IErrorCollection
    {
        public Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
        public ErrorCollection() { }

        public void AddError(string propertyName, string errorMessage)
        {
            if (!errors.ContainsKey(propertyName))
            {
                errors[propertyName] = new List<string>();
            }
            if (!errors[propertyName].Contains(errorMessage))
            {
                errors[propertyName].Add(errorMessage);
            }
        }

        public void RemoveError(string propertyName)
        {
            if (errors.ContainsKey(propertyName))
            {
                errors.Remove(propertyName);
            }
        }

        public string FormatErrorList(Dictionary<string, List<string>> errors)
        {
            var allErrors = new List<string>();
            foreach (var errorList in this.errors.Values)
            {
                allErrors.AddRange(errorList);
            }
            allErrors.RemoveAll(error => string.IsNullOrWhiteSpace(error));
            return string.Join("\n", allErrors);
        }
    }
}
