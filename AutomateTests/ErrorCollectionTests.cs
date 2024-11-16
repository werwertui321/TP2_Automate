using Automate.Utils.LocalServices;

namespace AutomateTests
{
    public class ErrorCollectionTests
    {
        private const string PROPERTY_NAME = "Username";
        private const string ERROR_MESSAGE = "ERREUR!";

        private ErrorCollection InitializeErrorCollectionAndAddError()
        {
            ErrorCollection errorCollection = new ErrorCollection();
            errorCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE);
            return errorCollection;
        }


        [Test]
        public void AddError_AddsErrorToCollection()
        {
            ErrorCollection errorCollection = InitializeErrorCollectionAndAddError();

            Assert.That(errorCollection.Errors, Has.Count.EqualTo(1));
        }

        [Test]
        public void AddError_AddedErrorHasSameMessage()
        {
            ErrorCollection errorCollection = InitializeErrorCollectionAndAddError();

            Assert.That(errorCollection.Errors[PROPERTY_NAME], Does.Contain(ERROR_MESSAGE));
        }

        [Test]
        public void AddError_AddsNothingIfKeyAlreadyExists()
        {
            ErrorCollection errorCollection = InitializeErrorCollectionAndAddError();

            errorCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE);

            Assert.That(errorCollection.Errors, Has.Count.EqualTo(1));
        }

        [Test]
        public void AddError_DoesntAddMessageToKeyIfMessageAlreadyExists()
        {
            ErrorCollection errorCollection = InitializeErrorCollectionAndAddError();

            errorCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE);

            Assert.That(errorCollection.Errors[PROPERTY_NAME], Has.Count.EqualTo(1));
        }

        [Test]
        public void RemoveError_RemovesErrorFromCollection()
        {
            ErrorCollection errorCollection = InitializeErrorCollectionAndAddError();

            errorCollection.RemoveError(PROPERTY_NAME);

            Assert.That(errorCollection.Errors, Is.Empty);
        }

        [Test]
        public void RemoveError_OnlyRemovesErrorWithPropertyName()
        {
            ErrorCollection errorCollection = InitializeErrorCollectionAndAddError();
            errorCollection.AddError("", "");

            errorCollection.RemoveError(PROPERTY_NAME);

            Assert.That(errorCollection.Errors, Has.Count.EqualTo(1));
        }
    }
}
