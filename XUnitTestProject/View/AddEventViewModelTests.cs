using Xunit;
using SVCalendar.WPF.View;
using SVCalendar.WPF;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using SVCalendar.Model;
using System.Windows.Input;

namespace SVCalendar.View.Tests
{
    public class AddEventViewModelTests
    {
        private AddEventViewModel addEventViewModel;

        private User currentUser;

        private Mock<IEventsRepository> eventsRepositoryMock;

        public AddEventViewModelTests()
        {
            eventsRepositoryMock = new Mock<IEventsRepository>();
            eventsRepositoryMock
                .Setup(er => er.GetAllUsersAvailableBetweenDates(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new List<User>());

            currentUser = new User()
                           {
                               UserId = 1,
                               Name = "pedro"
                           };
            addEventViewModel = new AddEventViewModel(eventsRepositoryMock.Object, currentUser);
        }

        [Fact]
        public void SelectedUserShouldBeNullAtFirst()
        {
            Assert.Null(addEventViewModel.SelectedUser);
        }

        [Fact]
        public void ShouldNotAllowSettingSelectedUserAsCurrentUserSettingNullInstead()
        {
            addEventViewModel.SelectedUser = currentUser;
            Assert.Null(addEventViewModel.SelectedUser);
        }

        [Fact]
        public void ShouldClearAllFieldsWhenResetEventCommandIsSelected()
        {
            addEventViewModel.SelectedUser = new User();
            addEventViewModel.EventTitle = "something";
            addEventViewModel.EventDescription = "something";
            addEventViewModel.EventEndDate = addEventViewModel.EventStartDate.AddDays(1);
            
            addEventViewModel.SaveEventCommand.RaiseCanExecuteChanged();
             
            ((ICommand)addEventViewModel.SaveEventCommand).Execute(null);

            eventsRepositoryMock.Verify(er => er.AddEvent(It.IsAny<Event>()), Times.Once);
        }
    }
}