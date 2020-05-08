using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SVCalendar.Model;

namespace SVCalendar.WPF
{
    class EventsRepository : IEventsRepository
    {
        private CalendarDbContext _db;

        public EventsRepository()
        {
            _db = new CalendarDbContext();
        }

        public List<Event> GetEvents() => _db.Events.ToList();

        public void AddEvent(Event eventToAdd)
        {
            _db.Add(eventToAdd);
            _db.SaveChanges();
        }

    }
}
