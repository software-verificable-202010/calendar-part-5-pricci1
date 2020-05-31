namespace SVCalendar.Model
{
    using System;

    public class Event
    {
        public string Description { get; set; }

        public DateTime EndDate { get; set; }

        public int EventId { get; set; }

        public DateTime StartDate { get; set; }

        public string Title { get; set; }

        public bool StartDateIsEarlierThanEndDate()
        {
            return DateTime.Compare(StartDate, EndDate) <= 0;
        }
    }
}