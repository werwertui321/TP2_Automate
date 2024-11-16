using Automate.Utils.LocalServices;
using Automate.Views;
using Automate.Interfaces;
using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Automate.ViewModels
{
    public class LoginViewModel : INotifyDataErrorInfo, INotifyPropertyChanged
    {
        private string? _username;
        private string? _password;
        private readonly IUserService _userService;
        private readonly INavigationUtils _navigationService;
        private Window _window;
        public IUser? _authenticatedUser;
        public readonly IErrorCollection _errorCollection;
        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        public ICommand AuthenticateCommand { get; }
        public bool HasPasswordErrors => _errorCollection.Errors.ContainsKey(nameof(Password)) && _errorCollection.Errors[nameof(Password)].Any();
        public bool HasErrors => _errorCollection.Errors.Count > 0;


        public LoginViewModel(Window openedWindow, IUserService userService, INavigationUtils navigationUtils)
        {
            _userService = userService;
            _navigationService = navigationUtils;
            _errorCollection = new ErrorCollection();
            _window = openedWindow;
            AuthenticateCommand = new RelayCommand(ValidateAuthentication);
        }

        public string? Username
        {
            get => _username;
            set
            {
                _username = value;
                NotifyOnPropertyChanged(nameof(Username));
                ValidateUsernameIsNullOrEmpty();
            }
        }

        public string? Password
        {
            get => _password;
            set
            {
                _password = value;
                NotifyOnPropertyChanged(nameof(Password));
                ValidatePasswordIsNullOrEmpty();
            }
        }

        public string ErrorMessages
        {
            get { return _errorCollection.FormatErrorListIntoSingleString(); }
        }

        public void AddError(string propertyName, string message)
        {
            _errorCollection.AddError(propertyName, message);
            NotifyOnPropertyChanged(nameof(ErrorMessages));
            NotifyOnPropertyChanged(nameof(HasPasswordErrors));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public void RemoveError(string propertyName)
        {
            _errorCollection.RemoveError(propertyName);
            NotifyOnPropertyChanged(nameof(ErrorMessages));
            NotifyOnPropertyChanged(nameof(HasPasswordErrors));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public void NotifyOnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ValidateAuthentication()
        {
            ValidateUsernameIsNullOrEmpty();
            ValidatePasswordIsNullOrEmpty();

            if (HasErrors)
                return;

            _authenticatedUser = _userService.Authenticate(Username, Password);
            if (_authenticatedUser == null)
            {
                AddError(nameof(Username), "Nom d'utilisateur ou mot de passe invalide");
                AddError(nameof(Password), "");
                return;
            }

            NavigateToHomeWindow();
        }

        public void NavigateToHomeWindow()
        {
            _navigationService.OpenNewView<HomeWindow>();
            _navigationService.CloseCurrentView(_window);
        }

        public void ValidateUsernameIsNullOrEmpty()
        {
            if (string.IsNullOrEmpty(Username))
            {
                AddError(nameof(Username), "Le nom d'utilisateur ne peut pas être vide.");
            }
            else
            {
                RemoveError(nameof(Username));
            }
        }

        public void ValidatePasswordIsNullOrEmpty()
        {
            if (string.IsNullOrEmpty(Password))
            {
                AddError(nameof(Password), "Le mot de passe ne peut pas être vide.");
            }
            else
            {
                RemoveError(nameof(Password));
            }
        }

        public IEnumerable GetErrors(string? propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !_errorCollection.Errors.ContainsKey(propertyName))
            {
                return Enumerable.Empty<string>();
            }

            return _errorCollection.Errors[propertyName];
        }

    }
}
