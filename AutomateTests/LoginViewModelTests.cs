using Automate.Interfaces;
using Automate.ViewModels;
using Moq;
using Automate.Models;
using System.Windows;

namespace AutomateTests
{
    [Apartment(ApartmentState.STA)]
    public class LoginViewModelTests
    {

        private Mock<Window>? windowMock;
        private Mock<IUserService>? userServiceMock;
        private Mock<INavigationUtils>? navigationUtilsMock;
        private const string PROPERTY_NAME = "Username";
        private const string ERROR_MESSAGE = "ERREUR!";
        private const string PASSWORD = "password";
        private const string USERNAME = "username";

        public LoginViewModel ArrangeLoginViewModelWithUserserviceSetup()
        {
            windowMock = new Mock<Window>();
            userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(new User());
            navigationUtilsMock = new Mock<INavigationUtils>();
            return new LoginViewModel(windowMock.Object, userServiceMock.Object, navigationUtilsMock.Object);
        }

        public LoginViewModel ArrangeLoginViewModel()
        {
            windowMock = new Mock<Window>();
            userServiceMock = new Mock<IUserService>();
            navigationUtilsMock = new Mock<INavigationUtils>();
            return new LoginViewModel(windowMock.Object, userServiceMock.Object, navigationUtilsMock.Object);
        }

        [Test]
        public void AddError_HasErrorIsTrue()
        {
            LoginViewModel loginViewModel = ArrangeLoginViewModel();

            loginViewModel.AddError(PROPERTY_NAME, ERROR_MESSAGE);

            Assert.That(loginViewModel.HasErrors, Is.True);
        }

        [Test]
        public void AddError_AddsErrorToErrorCollection()
        {
            LoginViewModel loginViewModel = ArrangeLoginViewModel();

            loginViewModel.AddError(PROPERTY_NAME, ERROR_MESSAGE);

            Assert.That(loginViewModel._errorCollection.Errors.ContainsKey(PROPERTY_NAME), Is.True);
        }

        [Test]
        public void RemoveError_HasErrorIsFalse()
        {
            LoginViewModel loginViewModel = ArrangeLoginViewModel();
            loginViewModel.AddError(PROPERTY_NAME, ERROR_MESSAGE);

            loginViewModel.RemoveError(PROPERTY_NAME);

            Assert.That(loginViewModel.HasErrors, Is.False);
        }

        [Test]
        public void RemoveError_RemovesErrorFromErrorCollection()
        {
            LoginViewModel loginViewModel = ArrangeLoginViewModel();
            loginViewModel.AddError(PROPERTY_NAME, ERROR_MESSAGE);

            loginViewModel.RemoveError(PROPERTY_NAME);

            Assert.That(loginViewModel._errorCollection.Errors.ContainsKey(PROPERTY_NAME), Is.False);
        }

        [Test]
        public void ValidateUsernameIsNullOrEmpty_AddsErrorOnEmptyUsername()
        {
            LoginViewModel loginViewModel = ArrangeLoginViewModel();

            loginViewModel.ValidateUsernameIsNullOrEmpty();

            Assert.That(loginViewModel._errorCollection.Errors.ContainsKey("Username"), Is.True);
        }

        [Test]
        public void ValidatePasswordIsNullOrEmpty_AddsErrorOnEmptyPassword()
        {
            LoginViewModel loginViewModel = ArrangeLoginViewModel();

            loginViewModel.ValidatePasswordIsNullOrEmpty();

            Assert.That(loginViewModel._errorCollection.Errors.ContainsKey("Password"), Is.True);
        }

        [Test]
        public void ValidateAuthentication_UsernameIsNull()
        {
            LoginViewModel loginViewModel = ArrangeLoginViewModel();
            loginViewModel.Password = PASSWORD;

            loginViewModel.ValidateAuthentication();

            Assert.That(loginViewModel._authenticatedUser, Is.Null);
        }

        [Test]
        public void ValidateAuthentication_PasswordIsNull()
        {
            LoginViewModel loginViewModel = ArrangeLoginViewModel();
            loginViewModel.Username = USERNAME;

            loginViewModel.ValidateAuthentication();

            Assert.That(loginViewModel._authenticatedUser, Is.Null);
        }

        [Test]
        public void ValidateAuthentication_Calls_AddError_OnNullUser()
        {
            LoginViewModel loginViewModel = ArrangeLoginViewModel();
            loginViewModel.Username = USERNAME;
            loginViewModel.Password = PASSWORD;

            loginViewModel.ValidateAuthentication();

            Assert.That(loginViewModel._errorCollection.Errors.ContainsKey("Username"), Is.True);
        }

        [Test]
        public void ValidateAuthentication_GoodCredentials_AffectsUserToAuthenticatedUser()
        {
            LoginViewModel loginViewModel = ArrangeLoginViewModelWithUserserviceSetup();
            loginViewModel.Username = USERNAME;
            loginViewModel.Password = PASSWORD;

            loginViewModel.ValidateAuthentication();

            Assert.That(loginViewModel._authenticatedUser, Is.Not.Null);
        }

        [Test]
        public void ValidateAuthentication_GoodCredentials_CallsNavigateFunction()
        {
            LoginViewModel loginViewModel = ArrangeLoginViewModelWithUserserviceSetup();
            loginViewModel.Username = USERNAME;
            loginViewModel.Password = PASSWORD;

            loginViewModel.ValidateAuthentication();

            navigationUtilsMock.Verify(x => x.OpenNewView<Window>());
        }
    }
}
