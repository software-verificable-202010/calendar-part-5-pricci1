using System;
using System.Collections.Generic;
using System.Text;

namespace SVCalendar.Model
{
    public class Event
    {
        public int EventId { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool StartDateIsEarlierThanEndDate() => DateTime.Compare(StartDate, EndDate) <= 0;
    }
}
