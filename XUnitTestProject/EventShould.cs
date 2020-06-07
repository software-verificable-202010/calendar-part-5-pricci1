namespace SVCalendar.Tests
{
    using System;
    using SVCalendar.Model;
    using Xunit;

    public class EventShould
    {
        [Fact]
        public void ReturnFalseWhenEndDateIsEarlierThanStartDate()
        {
            var anEvent = new Event { StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(-1) };
            bool sot = anEvent.StartDateIsEarlierThanEndDate();

            Assert.False(sot);
        }

        [Fact]
        public void ReturnFalseIfEventDoesNotIntersectDateRange()
        {
            DateTime now = DateTime.Now;
            Event anEvent = new Event { StartDate = now, EndDate = now.AddMinutes(30) };

            bool sot = anEvent.EventDoesNotIntersectDatesRange(now.AddMinutes(-30), now.AddMinutes(-20));

            Assert.False(sot);
        }
    }
}
