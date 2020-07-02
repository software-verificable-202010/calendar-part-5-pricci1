using System.Collections.Generic;
using System.Linq;
using SVCalendar.Model;

namespace SVCalendar.WPF
{
    using System;

    using Microsoft.EntityFrameworkCore;

    using SVCalendar.WPF.Annotations;

    internal class EventsRepository : IEventsRepository, IDisposable
    {
        private readonly CalendarDbContext db;

        public EventsRepository()
        {
            db = new CalendarDbContext();
        }

        public List<Event> GetEvents()
        {
            return db.Events.ToList();
        }

        public void AddEvent(Event eventToAdd)
        {
            db.Events.Add(eventToAdd);
            db.SaveChanges();
        }


        public User GetUserByName(string userName)
        {
            return db.Users.FirstOrDefault(user => string.Equals(user.Name, userName));
        }

        public List<User> GetAllUsers()
        {
            return db.Users.ToList();
        }

        public List<User> GetAllUsersAvailableBetweenDates(DateTime startDate, DateTime endDate)
        {
            var unavailableUsersQuery = from userEvent in db.UserEvents
                                                     where userEvent.Event.StartDate > endDate && userEvent.Event.EndDate < startDate
                                                     select userEvent.User; // TODO fix
            List<User> unavailableUsers = unavailableUsersQuery.Distinct().ToList();
            List<User> allUsers = GetAllUsers();
            List<User> availableUsers = allUsers.Except(unavailableUsers).ToList();

            return availableUsers;
        }

        public List<Event> GetUserEvents(User user)
        {
            User retrivedUser = db.Users.Include(u => u.UserEvents).ThenInclude(ue => ue.Event).First(u => u.UserId == user.UserId);
            return retrivedUser.UserEvents.Select(ue => ue.Event).ToList();
        }

        public void AddUser(User user)
        {
            db.Add(user);
            db.SaveChanges();
        }

        public List<Event> GetUserOwnedEvents(User user)
        {
            return db.Events.Where(e => e.Owner == user).ToList();
        }

        public void DeleteEvent(Event anEvent)
        {
            db.Remove(anEvent);
            db.SaveChanges();
        }

        public void UpdateEvent(Event anEvent)
        {
            db.Update(anEvent);
            db.SaveChanges();
        }

        public void Dispose()
        {
            db?.Dispose();
        }
    }
}