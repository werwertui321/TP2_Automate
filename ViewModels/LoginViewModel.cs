using Automate.Utils.LocalServices;
using Automate.Utils.Services;
using Automate.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Automate.ViewModels
{
    public class LoginViewModel
    {
        private string? _username;
        private string? _password;
        private readonly IUserService _userService;
        private readonly NavigationService _navigationService;
        private Window _window;

        private ErrorCollection errorCollection;
        private string ErrorMessages;

        public bool HasPasswordErrors => errorCollection.errors.ContainsKey(nameof(Password)) && errorCollection.errors[nameof(Password)].Any();


        public ICommand AuthenticateCommand { get; }
        

        public LoginViewModel(Window openedWindow, IUserService userService)
        {
            //instanciation de la BD
            _userService = userService;
            AuthenticateCommand = new RelayCommand(ValidateAuthentication);

            _navigationService = new NavigationService();
            errorCollection = new ErrorCollection();
            _window = openedWindow;

        }

        public string? Username
        {
            get => _username;
            set
            {
                _username = value;
                errorCollection.NotifyOnPropertyChanged(nameof(Username));
                ValidateProperty(nameof(Username));
            }
        }

        public string? Password
        {
            get => _password;
            set
            {
                _password = value;
                errorCollection.NotifyOnPropertyChanged(nameof(Password));
                ValidateProperty(nameof(Password));
            }
        }

        private void AddError(string propertyName,  string message)
        {
            errorCollection.AddError(propertyName, message);
            errorCollection.NotifyOnPropertyChanged(nameof(HasPasswordErrors));
        }

        private void RemoveError(string propertyName)
        {
            errorCollection.RemoveError(propertyName);
            errorCollection.NotifyOnPropertyChanged(nameof(HasPasswordErrors));
        }

        public void ValidateAuthentication()
        {
            ValidateProperty(nameof(Username));
            ValidateProperty(nameof(Password));

            if (!errorCollection.HasErrors)
            {
                var user = _userService.Authenticate(Username, Password);
                if (user == null)
                {
                    AddError(nameof(Username), "Nom d'utilisateur ou mot de passe invalide");
                    AddError(nameof(Password), "");
                    Trace.WriteLine("invalid");
                }
                else
                {
                    _navigationService.OpenNewView<AccueilWindow>();
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

        

        

        

    }
}
