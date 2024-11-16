using Automate.Interfaces;
using Automate.ViewModels;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutomateTests
{
    public class CalendarServiceTest
    {
        private Mock<IMongoDatabase> _database;
        private Mock<ICalendarService> _calendarService;
        private Mock<Window> _mockWindow;
        private CalendarViewModel _viewModel;

        [SetUp]
        public void Setup()
        {
            _calendarService = new CalendarService();
            _mockWindow = new Mock<Window>();
            _viewModel = new CalendarViewModel(_mockWindow.Object, _calendarService.Object, true);
        }
    }
}
