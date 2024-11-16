using Automate.Interfaces;
using Automate.Models;
using Automate.ViewModels;
using Moq;
using NUnit.Framework.Internal;
using System.Windows;

namespace AutomateTests
{
    [Apartment(ApartmentState.STA)]
    public class CalendarViewModelTests
    {
        private Mock<ICalendarService>? _calendarService;
        private Mock<Window>? _mockWindow;
        private const string PROPERTY_NAME = "TaskName";
        private const string ERROR_MESSAGE = "ERREUR!";

        private CalendarViewModel SetUp()
        {
            _calendarService = new Mock<ICalendarService>();
            _mockWindow = new Mock<Window>();
            return new CalendarViewModel(_mockWindow.Object, _calendarService.Object, true);
        }

        [Test]
        public void AddTask_TaskNameVide_AddError()
        {
            CalendarViewModel _viewModel = SetUp();
            _viewModel.TaskName = string.Empty;

            _viewModel.AddTask();

            Assert.That(_viewModel.ErrorMessages, Does.Contain("Le type d'évènement ne peut pas être vide"));
        }

        [Test]
        public void AddTask_WithValidTaskName()
        {
            CalendarViewModel _viewModel = SetUp();

            _viewModel.TaskName = "Semis";

            _viewModel.AddTask();

            Assert.That(_viewModel.TaskName, Is.EqualTo(string.Empty));
        }

        [Test]
        public void ValidateUpdate_withValidInput()
        {
            CalendarViewModel _viewModel = SetUp();

            _viewModel.TaskName = "Semis";
            _viewModel.SelectedTask = new AutomateTask(DateTime.Today, "Rempotage");

            Assert.That(_viewModel.ValidateUpdate(), Is.EqualTo(true));
        }

        [Test]
        public void ValidateUpdate_withInvalidTaskName()
        {
            CalendarViewModel _viewModel = SetUp();

            _viewModel.TaskName = string.Empty;
            _viewModel.SelectedTask = new AutomateTask(DateTime.Today, "Rempotage");

            Assert.That(_viewModel.ValidateUpdate(), Is.EqualTo(false));
        }

        [Test]
        public void ValidateUpdate_withInvalidSelectedTask()
        {
            CalendarViewModel _viewModel = SetUp();

            _viewModel.TaskName = "Semis";
            _viewModel.SelectedTask = null;

            Assert.That(_viewModel.ValidateUpdate(), Is.EqualTo(false));
        }

        [Test]
        public void UpdateTask_withValidInput()
        {
            CalendarViewModel _viewModel = SetUp();

            _viewModel.TaskName = "Semis";
            _viewModel.SelectedTask = new AutomateTask(DateTime.Today, "Rempotage");

            _viewModel.UpdateTask();

            Assert.That(_viewModel.TaskName, Is.EqualTo(string.Empty));
        }


        [Test]
        public void UpdateTask_withInvalidTaskName()
        {
            CalendarViewModel _viewModel = SetUp();

            _viewModel.TaskName = string.Empty;
            _viewModel.SelectedTask = new AutomateTask(DateTime.Today, "Rempotage");

            _viewModel.UpdateTask();

            Assert.That(_viewModel.ErrorMessages, Does.Contain("Le type d'évènement ne peut pas être vide"));
        }

        [Test]
        public void UpdateTask_withInvalidSelectedTask()
        {
            CalendarViewModel _viewModel = SetUp();

            _viewModel.TaskName = "Semis";
            _viewModel.SelectedTask = null;

            _viewModel.UpdateTask();

            Assert.That(_viewModel.ErrorMessages, Does.Contain("Une tâche doit être sélectionner pour pouvoir modifier"));
        }

        [Test]
        public void DeleteTask_WithInvalidSelectedTask()
        {
            CalendarViewModel _viewModel = SetUp();

            _viewModel.SelectedTask = null;

            _viewModel.DeleteTask();

            Assert.That(_viewModel.ErrorMessages, Does.Contain("Une tâche doit être sélectionner pour pouvoir supprimer"));
        }


        [Test]
        public void AddError_HasErrorIsTrue()
        {
            CalendarViewModel _viewModel = SetUp();

            _viewModel.AddError(PROPERTY_NAME, ERROR_MESSAGE);

            Assert.That(_viewModel.HasErrors, Is.True);
        }

        [Test]
        public void AddError_AddsErrorToErrorCollection()
        {
            CalendarViewModel _viewModel = SetUp();

            _viewModel.AddError(PROPERTY_NAME, ERROR_MESSAGE);

            Assert.That(_viewModel.errorCollection.Errors.ContainsKey(PROPERTY_NAME), Is.True);
        }

        [Test]
        public void RemoveError_HasErrorIsFalse()
        {
            CalendarViewModel _viewModel = SetUp();
            _viewModel.AddError(PROPERTY_NAME, ERROR_MESSAGE);

            _viewModel.RemoveError(PROPERTY_NAME);

            Assert.That(_viewModel.HasErrors, Is.False);
        }

        [Test]
        public void RemoveError_RemovesErrorFromErrorCollection()
        {
            CalendarViewModel _viewModel = SetUp();
            _viewModel.AddError(PROPERTY_NAME, ERROR_MESSAGE);

            _viewModel.RemoveError(PROPERTY_NAME);

            Assert.That(_viewModel.errorCollection.Errors.ContainsKey(PROPERTY_NAME), Is.False);
        }

    }
}