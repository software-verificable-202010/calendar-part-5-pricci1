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
    }
}
