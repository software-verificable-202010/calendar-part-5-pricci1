using System.Collections.Generic;
using System.Linq;
using SVCalendar.Model;

namespace SVCalendar.WPF
{
    internal class EventsRepository : IEventsRepository
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
            db.Add(eventToAdd);
            db.SaveChanges();
        }
    }
}