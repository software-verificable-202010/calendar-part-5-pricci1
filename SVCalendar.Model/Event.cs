namespace SVCalendar.Model
{
    using System;
    using System.Collections.Generic;

    public class Event
    {
        #region Events, Interfaces, Properties

        public string Description { get; set; }

        public DateTime EndDate { get; set; }

        public int EventId { get; set; }

        public User Owner { get; set; }

        public DateTime StartDate { get; set; }

        public string Title { get; set; }

        public List<UserEvent> UserEvents { get; set; }

        #endregion

        #region Methods

        public bool EventDoesNotIntersectDatesRange(DateTime startDate, DateTime endDate)
        {
            bool eventEndsBeforeStartDate = DateTime.Compare(EndDate, startDate) < 0;
            bool eventStartsAfterEndDate = DateTime.Compare(StartDate, endDate) > 0;

            return eventEndsBeforeStartDate || eventStartsAfterEndDate;
        }

        public bool StartDateIsEarlierThanEndDate()
        {
            return DateTime.Compare(StartDate, EndDate) <= 0;
        }

        #endregion
    }
}