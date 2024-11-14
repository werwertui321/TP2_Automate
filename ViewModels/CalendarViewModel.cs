using Automate.Models;
using Automate.Utils.LocalServices;
using Automate.Utils.Services;
using Automate.Utils;
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
using MongoDB.Driver.Core.Connections;

namespace Automate.ViewModels
{
    public class CalendarViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private DateTime _selectedDate;
        private string? _taskName;
        private TaskModel? _selectedTask;
        private List<TaskModel>? _tasks;
        private readonly ErrorCollection errorCollection;
        private readonly CalendarService _calendarService;
        private readonly bool _isAdmin;
        private Window _window;

        public ICommand AddTaskCommand { get; }
        public ICommand UpdateTaskCommand { get; }
        public ICommand DeleteTaskCommand { get; }

        public bool HasErrors => errorCollection.errors.Count > 0;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public CalendarViewModel(Window openedWindow, CalendarService calendarService)
        {
            _calendarService = calendarService;
            _isAdmin = Env.authenticatedUser.IsAdmin;
            _selectedDate = DateTime.Today;
            AddTaskCommand = new RelayCommand(AddTask);
            UpdateTaskCommand = new RelayCommand(UpdateTask);
            DeleteTaskCommand = new RelayCommand(DeleteTask);
            errorCollection = new ErrorCollection();
            Tasks = _calendarService.GetTasksByDate(SelectedDate);
            _window = openedWindow;
        }

        public bool IsAdmin { get =>  _isAdmin; }

        public List<bool> Important
        {
            get => CreateImportantList();
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

        public string? TaskName
        {
            get => _taskName;
            set
            {
                _taskName = value;
                NotifyOnPropertyChanged(nameof(TaskName));
            }
        }

        public void AddTask()
        {
            if(string.IsNullOrEmpty(TaskName))
            {
                AddError(nameof(TaskName), "La description ne peut pas être vide");
            }
            else
            {
                RemoveError(nameof(TaskName));
                TaskModel task = new TaskModel(SelectedDate, TaskName.Trim());
                _calendarService.AddTask(task);
                TaskName = "";
                Tasks = _calendarService.GetTasksByDate(SelectedDate);
            }
        }

        public void UpdateTask()
        {
            bool isValid = true;
            RemoveError(nameof(TaskName));
            RemoveError(nameof(SelectedTask));
            if (string.IsNullOrEmpty(TaskName))
            {
                AddError(nameof(TaskName), "La description ne peut pas être vide");
                isValid = false;
            }

            if (SelectedTask is null)
            {
                AddError(nameof(SelectedTask), "Un tâche doit être sélectionner pour pouvoir modifier");
                isValid = false;
            }

            if (isValid)
            {
                _calendarService.UpdateTask(TaskName, SelectedTask.Id);
                Tasks = _calendarService.GetTasksByDate(SelectedDate);
                TaskName = "";
            }
        }

        public void DeleteTask()
        {
            if(_selectedTask is not null)
            {
                RemoveError(nameof(TaskName));
                _calendarService.DeleteTask(SelectedTask.Id);
                Tasks = _calendarService.GetTasksByDate(SelectedDate);
            }
            else
            {
                AddError(nameof(SelectedTask), "Un tâche doit être sélectionner pour pouvoir supprimer");
            }
        }

        public List<bool> CreateImportantList()
        {
            List<bool> important = new List<bool>();
            for (int i = 0; i < _tasks.Count; i++)
            {
                important[i] = _tasks[i].Important;
            }
            return important;
        }

        public string ErrorMessages
        {
            get { return errorCollection.FormatErrorList(errorCollection.errors); }
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
