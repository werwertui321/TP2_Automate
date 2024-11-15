using Automate.Models;
using Automate.Utils.LocalServices;
using Automate.Utils.DataServices;
using Automate.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Automate.Interfaces;

namespace Automate.ViewModels
{
    public class CalendarViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private DateTime _selectedDate;
        private string? _taskName;
        private AutomateTask? _selectedTask;
        private List<AutomateTask>? _tasks;
        private readonly ErrorCollection errorCollection;
        private readonly ICalendarService _calendarService;
        private readonly bool _isAdmin;
        private Window _window;

        public ICommand AddTaskCommand { get; }
        public ICommand UpdateTaskCommand { get; }
        public ICommand DeleteTaskCommand { get; }

        public bool HasErrors => errorCollection.errors.Count > 0;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public CalendarViewModel(Window openedWindow, ICalendarService calendarService, bool isAdmin)
        {
            _calendarService = calendarService;
            _isAdmin = isAdmin;
            _selectedDate = DateTime.Today;
            AddTaskCommand = new RelayCommand(AddTask);
            UpdateTaskCommand = new RelayCommand(UpdateTask);
            DeleteTaskCommand = new RelayCommand(DeleteTask);
            errorCollection = new ErrorCollection();
            GetTaskForSelectedDay();
            _window = openedWindow;
        }

        public bool IsAdmin { get =>  _isAdmin; }

        public List<bool> Important
        {
            get => CreateImportantList();
        }

        public List<AutomateTask> Tasks
        {
            get => _tasks;
            set
            {
                _tasks = value;
                NotifyOnPropertyChanged(nameof(Tasks));
            }
        }

        public AutomateTask SelectedTask
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
                GetTaskForSelectedDay();
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
            try
            {
                if (string.IsNullOrEmpty(TaskName))
                {
                    AddError(nameof(TaskName), "Le type d'évènement ne peut pas être vide");
                }
                else
                {
                    RemoveError(nameof(TaskName));
                    AutomateTask task = new AutomateTask(SelectedDate, TaskName.Trim());
                    _calendarService.AddTask(task);
                    TaskName = "";
                    GetTaskForSelectedDay();
                }
            }
            catch(Exception exception)
            {
                AddError(nameof(AddTask), exception.Message);
            }
        }

        public void UpdateTask()
        {
            try
            {
                if (ValidateUpdate())
                {
                    _calendarService.UpdateTask(TaskName, SelectedTask.Id);
                    GetTaskForSelectedDay();
                    TaskName = "";
                }
            }
            catch (Exception exception)
            {
                AddError(nameof(UpdateTask), exception.Message);
            }
        }

        public bool ValidateUpdate()
        {
            RemoveError(nameof(TaskName));
            RemoveError(nameof(SelectedTask));
            if (string.IsNullOrEmpty(TaskName))
            {
                AddError(nameof(TaskName), "Le type d'évènement ne peut pas être vide");
                return false;
            }

            if (SelectedTask is null)
            {
                AddError(nameof(SelectedTask), "Une tâche doit être sélectionner pour pouvoir modifier");
                return false;
            }

            return true;
        }

        public void DeleteTask()
        {
            try
            {
                if (_selectedTask is not null)
                {
                    RemoveError(nameof(TaskName));
                    _calendarService.DeleteTask(SelectedTask.Id);
                    GetTaskForSelectedDay();
                }
                else
                {
                    AddError(nameof(SelectedTask), "Une tâche doit être sélectionner pour pouvoir supprimer");
                }
            }
            catch (Exception exception)
            {
                AddError(nameof(DeleteTask), exception.Message);
            }
        }

        public void GetTaskForSelectedDay()
        {
            Tasks = _calendarService.GetTasksByDate(SelectedDate);
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
