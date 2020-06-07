namespace SVCalendar.Model
{
    using System;
    using System.Collections.Generic;

    public class Event
    {
        public string Description { get; set; }

        public DateTime EndDate { get; set; }

        public int EventId { get; set; }

        public DateTime StartDate { get; set; }

        public string Title { get; set; }

        public User Owner { get; set; }

        public List<UserEvent> UserEvents { get; set; }

        public bool StartDateIsEarlierThanEndDate()
        {
            return DateTime.Compare(StartDate, EndDate) <= 0;
        }

        public bool EventDoesNotIntersectDatesRange(DateTime startDate, DateTime endDate)
        {
            bool eventEndsBeforeStartDate = DateTime.Compare(this.EndDate, startDate) < 0;
            bool eventStartsAfterEndDate = DateTime.Compare(this.StartDate, endDate) > 0;

            return eventEndsBeforeStartDate || eventStartsAfterEndDate;
        }
    }
}