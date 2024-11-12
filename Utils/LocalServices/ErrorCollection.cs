using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Automate.Utils.LocalServices
{
    public class ErrorCollection : INotifyDataErrorInfo, INotifyPropertyChanged
    {
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        public Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
        public event PropertyChangedEventHandler? PropertyChanged;

        public bool HasErrors => errors.Count > 0;

        public string ErrorMessages
        {
            get
            {
                return FormatErrorList(errors);
            }
        }

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
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            }
            NotifyOnPropertyChanged(nameof(ErrorMessages));
        }

        public void RemoveError(string propertyName)
        {
            if (errors.ContainsKey(propertyName))
            {
                errors.Remove(propertyName);
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            }
            NotifyOnPropertyChanged(ErrorMessages);
        }

        public void NotifyOnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string FormatErrorList(Dictionary<string, List<string>> errors)
        {
            var allErrors = new List<string>();
            foreach (var errorList in this.errors.Values)
            {
                allErrors.AddRange(errorList);
            }
            allErrors.RemoveAll(error => string.IsNullOrWhiteSpace(error));
            return string.Join("\n", allErrors);
        }

        public IEnumerable GetErrors(string? propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !errors.ContainsKey(propertyName))
            {
                return Enumerable.Empty<string>();
            }

            return errors[propertyName];
        }
    }
}
