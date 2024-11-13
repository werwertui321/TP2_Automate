using Automate.Models;
using Automate.Utils.Services;
using System;
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
    public class CalendarViewModel : INotifyPropertyChanged
    {
        private DateTime _selectedDate;
        private string? _taskDescription;
        private TaskModel? _selectedTask;
        private List<TaskModel> _tasks;
        private readonly CalendarService _calendarService;
        private readonly MongoDBService _database;
        private Window _window;

        public ICommand AddTaskCommand { get; }


        public event PropertyChangedEventHandler? PropertyChanged;

        public CalendarViewModel(Window openedWindow)
        {
            _tasks = new List<TaskModel>();
            _database = new MongoDBService();
            _calendarService = new CalendarService(_database);
            AddTaskCommand = new RelayCommand(AddTask);
            _selectedDate = DateTime.Today;
            Tasks = _calendarService.GetTasksByDate(SelectedDate);
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

        public void AddTask()
        {
            TaskModel task = new TaskModel(SelectedDate, TaskDescription);
            _calendarService.AddTask(task);
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

    }
}
