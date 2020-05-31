using System.Collections.Generic;
using SVCalendar.Model;

namespace SVCalendar.WPF
{
    public interface IEventsRepository
    {
        List<Event> GetEvents();

        void AddEvent(Event eventToAdd);
    }
}