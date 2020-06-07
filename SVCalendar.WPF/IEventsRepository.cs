using System.Collections.Generic;
using SVCalendar.Model;

namespace SVCalendar.WPF
{
    using System;
    using System.Collections.ObjectModel;

    public interface IEventsRepository
    {
        List<Event> GetEvents();

        void AddEvent(Event eventToAdd);

        public User GetUserByName(string userName);

        public List<Event> GetUserEvents(User user);

        public List<User> GetAllUsersAvailableBetweenDates(DateTime startDate, DateTime endDate);

        public void AddUser(User user);

        List<Event> GetUserOwnedEvents(User user);

        public void DeleteEvent(Event anEvent);

        public void UpdateEvent(Event anEvent);

    }
}