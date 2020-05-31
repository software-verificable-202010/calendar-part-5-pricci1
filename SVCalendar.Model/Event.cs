using System;

namespace SVCalendar.Model
{
    public class Event
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool StartDateIsEarlierThanEndDate()
        {
            return DateTime.Compare(StartDate, EndDate) <= 0;
        }
    }
}