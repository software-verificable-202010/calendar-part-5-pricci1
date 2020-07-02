using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using SVCalendar.WPF.View;
using Moq;
using SVCalendar.Model;
using SVCalendar.WPF;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SVCalendar.Tests.View
{
    public class EditEventsViewModelTests
    {
        private readonly User currentUser;

        private readonly Mock<IEventsRepository> eventsRepositoryMock;

        public EditEventsViewModelTests()
        {
            eventsRepositoryMock = new Mock<IEventsRepository>();
            currentUser = new User()
                              {
                                  UserId = 1,
                                  Name = "pedro"
                              };
        }

        [Fact]
        public void ShouldGetCurrentUserEventsOnInitializeEvents()
        {
            var expected = new List<Event> { new Event() { Title = "Test" } };
            eventsRepositoryMock.Setup(er => er.GetUserOwnedEvents(currentUser)).Returns(expected);

            var editEventsViewModel = new EditEventsViewModel(eventsRepositoryMock.Object, currentUser);

            Assert.Equal(expected, editEventsViewModel.Events);
        }

        [Fact]
        public void ShouldDeleteSelectedEventWhenSelectingDeleteEventCommand()
        {
            var eventToDelete = new Event() { Title = "Test" };
            var events = new List<Event> { eventToDelete };
            eventsRepositoryMock.Setup(er => er.GetUserOwnedEvents(currentUser)).Returns(events);
            eventsRepositoryMock.Setup(er => er.DeleteEvent(eventToDelete));

            var editEventsViewModel = new EditEventsViewModel(eventsRepositoryMock.Object, currentUser);
            editEventsViewModel.Events = new ObservableCollection<Event>
                                             {
                                                 eventToDelete
                                             };
            editEventsViewModel.SelectedEvent = eventToDelete;

            ((ICommand)editEventsViewModel.DeleteEventCommand).Execute(null);

            Assert.DoesNotContain(eventToDelete, editEventsViewModel.Events);
            eventsRepositoryMock.Verify(er => er.DeleteEvent(eventToDelete), Times.Once);
        }
    }
}
