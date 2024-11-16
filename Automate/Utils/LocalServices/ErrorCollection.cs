using System.Collections.Generic;
using Automate.Interfaces;


namespace Automate.Utils.LocalServices
{
    public class ErrorCollection : IErrorCollection
    {
        public Dictionary<string, List<string>> Errors {  get; set; }
        public ErrorCollection()
        {
            Errors = new Dictionary<string, List<string>>();
        }

        public void AddError(string propertyName, string errorMessage)
        {
            if (!Errors.ContainsKey(propertyName))
            {
                Errors[propertyName] = new List<string>();
            }
            if (!Errors[propertyName].Contains(errorMessage))
            {
                Errors[propertyName].Add(errorMessage);
            }
        }

        public void RemoveError(string propertyName)
        {
            if (Errors.ContainsKey(propertyName))
            {
                Errors.Remove(propertyName);
            }
        }

        public string FormatErrorListIntoSingleString()
        {
            var allErrors = new List<string>();
            foreach (var errorList in Errors.Values)
            {
                allErrors.AddRange(errorList);
            }
            allErrors.RemoveAll(error => string.IsNullOrWhiteSpace(error));
            return string.Join("\n", allErrors);
        }
    }
}
