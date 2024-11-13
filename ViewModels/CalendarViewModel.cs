using Automate.Models;
using Automate.Utils.LocalServices;
using Automate.Utils.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Automate.ViewModels
{
    public class CalendarViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private DateTime _selectedDate;
        private string? _taskDescription;
        private TaskModel? _selectedTask;
        private List<TaskModel> _tasks;
        private readonly ErrorCollection errorCollection;
        private readonly CalendarService _calendarService;
        private readonly MongoDBService _database;
        private Window _window;

        public ICommand AddTaskCommand { get; }
        public bool HasErrors => errorCollection.errors.Count > 0;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        

        public CalendarViewModel(Window openedWindow)
        {
            _selectedDate = DateTime.Today;
            _tasks = new List<TaskModel>();
            _database = new MongoDBService();
            _calendarService = new CalendarService(_database);
            AddTaskCommand = new RelayCommand(AddTask);
            Tasks = _calendarService.GetTasksByDate(SelectedDate);
            errorCollection = new ErrorCollection();
            _window = openedWindow;
        }

        public List<TaskModel> Tasks
        {
            get => _tasks;
            set
            {
                _tasks = value;
                NotifyOnPropertyChanged(nameof(Tasks));
            }
        }

        public TaskModel SelectedTask
        {
            get => _selectedTask;
            set
            {
                _selectedTask = value;
                NotifyOnPropertyChanged(nameof(SelectedDate));
            }
        }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                NotifyOnPropertyChanged(nameof(SelectedDate));
                Tasks = _calendarService.GetTasksByDate(SelectedDate);
            }
        }

        public string? TaskDescription
        {
            get => _taskDescription;
            set
            {
                _taskDescription = value;
                NotifyOnPropertyChanged(nameof(TaskDescription));
            }
        }

        public string ErrorMessages
        {
            get { return errorCollection.FormatErrorList(errorCollection.errors); }
        }


        public void AddTask()
        {
            if(string.IsNullOrEmpty(TaskDescription))
            {
                AddError(nameof(TaskDescription), "La description ne peut pas être vide");
            }
            else
            {
                RemoveError(nameof(TaskDescription));
                TaskModel task = new TaskModel(SelectedDate, TaskDescription.Trim());
                _calendarService.AddTask(task);
                TaskDescription = "";
                Tasks = _calendarService.GetTasksByDate(SelectedDate);
            }

        }

        public void DeleteTask()
        {
            if(_selectedTask is not null)
            {
                _calendarService.DeleteTask(SelectedTask.Id.ToString());
            }
        }

        protected void NotifyOnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AddError(string propertyName, string message)
        {
            errorCollection.AddError(propertyName, message);
            NotifyOnPropertyChanged(nameof(ErrorMessages));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private void RemoveError(string propertyName)
        {
            errorCollection.RemoveError(propertyName);
            NotifyOnPropertyChanged(nameof(ErrorMessages));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public IEnumerable GetErrors(string? propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !errorCollection.errors.ContainsKey(propertyName))
            {
                return Enumerable.Empty<string>();
            }

            return errorCollection.errors[propertyName];
        }
    }
}
