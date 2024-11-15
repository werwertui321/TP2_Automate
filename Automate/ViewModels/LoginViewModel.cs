using Automate.Utils;
using Automate.Utils.LocalServices;
using Automate.Views;
using Automate.Interfaces;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
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
        private readonly NavigationUtils _navigationService;
        private Window _window;
        private readonly ErrorCollection _errorCollection;
        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        public ICommand AuthenticateCommand { get; }
        public bool HasPasswordErrors => _errorCollection.errors.ContainsKey(nameof(Password)) && _errorCollection.errors[nameof(Password)].Any();
        public bool HasErrors => _errorCollection.errors.Count > 0;


        public LoginViewModel(Window openedWindow, IUserService userService, NavigationUtils navigationUtils, ErrorCollection errorCollection)
        {
            _userService = userService;
            _navigationService = navigationUtils;
            _errorCollection = errorCollection;
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
                ValidateProperty(nameof(Username));
            }
        }

        public string? Password
        {
            get => _password;
            set
            {
                _password = value;
                NotifyOnPropertyChanged(nameof(Password));
                ValidateProperty(nameof(Password));
            }
        }

        public string ErrorMessages
        {
            get { return _errorCollection.FormatErrorList(_errorCollection.errors); }
        }

        private void AddError(string propertyName,  string message)
        {
            _errorCollection.AddError(propertyName, message);
            NotifyOnPropertyChanged(nameof(ErrorMessages));
            NotifyOnPropertyChanged(nameof(HasPasswordErrors));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public void NotifyOnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        private void RemoveError(string propertyName)
        {
            _errorCollection.RemoveError(propertyName);
            NotifyOnPropertyChanged(nameof(ErrorMessages));
            NotifyOnPropertyChanged(nameof(HasPasswordErrors));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public void ValidateAuthentication()
        {
            ValidateProperty(nameof(Username));
            ValidateProperty(nameof(Password));

            if (!HasErrors)
            {
                Env.authenticatedUser = _userService.Authenticate(Username, Password);
                if (Env.authenticatedUser == null)
                {
                    AddError(nameof(Username), "Nom d'utilisateur ou mot de passe invalide");
                    AddError(nameof(Password), "");
                    Trace.WriteLine("invalid");
                }
                else
                {
                    _navigationService.OpenNewView<HomeWindow>();
                    _navigationService.CloseCurrentView(_window);
                    Trace.WriteLine("logged in");
                }
            }
        }

        private void ValidateProperty(string? propertyName)
        {
            switch (propertyName)
            {
                case nameof(Username):
                    if (string.IsNullOrEmpty(Username))
                    {
                        AddError(nameof(Username), "Le nom d'utilisateur ne peut pas être vide.");
                    }
                    else
                    {
                        RemoveError(nameof(Username));
                    }
                    break;

                case nameof(Password):
                    if (string.IsNullOrEmpty(Password))
                    {
                        AddError(nameof(Password), "Le mot de passe ne peut pas être vide.");
                    }
                    else
                    {
                        RemoveError(nameof(Password));
                    }
                    break;
            }
        }

        public IEnumerable GetErrors(string? propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !_errorCollection.errors.ContainsKey(propertyName))
            {
                return Enumerable.Empty<string>();
            }

            return _errorCollection.errors[propertyName];
        }

    }
}
