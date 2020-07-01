using System;
using System.Collections.Generic;
using System.Text;

namespace SVCalendar.Tests.View
{
    using System.Windows.Input;

    using Moq;

    using SVCalendar.Model;
    using SVCalendar.WPF;
    using SVCalendar.WPF.View;

    using Xunit;

    public class SignInViewModelTests
    {
        private Mock<IEventsRepository> eventsRepositoryMock;

        public SignInViewModelTests()
        {
            eventsRepositoryMock = new Mock<IEventsRepository>();
        }

        [Fact]
        public void ShouldCallChangeUserDelegateAfterSignIn()
        {
            var changeUserCalled = false;
            eventsRepositoryMock.Setup(er => er.GetUserByName(It.IsAny<string>())).Returns(new User());
            var signInViewModel = new SignInViewModel(
                eventsRepositoryMock.Object,
                user => changeUserCalled = true);

            ((ICommand)signInViewModel.SignInCommand).Execute(null);

            Assert.True(changeUserCalled);
        }

        [Fact]
        public void ShouldCreateUserIfNoUserExistsWithGivenUserName()
        {
            var userName = "AUserNameThatDoesNotExist";
            eventsRepositoryMock.Setup(er => er.GetUserByName(userName)).Returns((User)null);
            var signInViewModel = new SignInViewModel(eventsRepositoryMock.Object, user => { });
            signInViewModel.UsernameText = userName;

            ((ICommand)signInViewModel.SignInCommand).Execute(null);

            var newUser = new User
                       {
                           Name = userName
                       };

            eventsRepositoryMock.Verify(er => er.AddUser(It.IsAny<User>()));
        }
    }
}
