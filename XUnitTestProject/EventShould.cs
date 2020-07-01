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

        [Theory]
        [InlineData(-30, -20, true)]
        [InlineData(31, 40, true)]
        [InlineData(-30, 20, false)]
        [InlineData(10, 12, false)]
        public void EventDoesNotIntersectDateRange(int startOffset, int endOffset, bool expected)
        {
            DateTime now = DateTime.Now;
            var anEvent = new Event { StartDate = now, EndDate = now.AddMinutes(30) };

            DateTime start = now.AddMinutes(startOffset);
            DateTime end = now.AddMinutes(endOffset);

            bool sot = anEvent.EventDoesNotIntersectDatesRange(start, end);

            Assert.Equal(expected, sot);
        }
    }
}
