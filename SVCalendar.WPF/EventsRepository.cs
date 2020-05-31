using System.Collections.Generic;
using System.Linq;
using SVCalendar.Model;

namespace SVCalendar.WPF
{
    class EventsRepository : IEventsRepository
    {
        private readonly CalendarDbContext _db;

        public EventsRepository()
        {
            _db = new CalendarDbContext();
        }

        public List<Event> GetEvents()
        {
            return _db.Events.ToList();
        }

        public void AddEvent(Event eventToAdd)
        {
            _db.Add(eventToAdd);
            _db.SaveChanges();
        }
    }
}