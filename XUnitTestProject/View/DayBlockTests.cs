using System;
using System.Collections.Generic;
using System.Text;

namespace SVCalendar.Tests.View
{
    using System.Windows.Media;

    using SVCalendar.WPF.View;

    using Xunit;

    public class DayBlockTests
    {
        [Fact]
        public void DayBlockHasDateShouldBeNullIfNoDatetimePassedInConstruction()
        {
            var dayBlock = new DayBlock();

            Assert.True(dayBlock.DayNumber == -1);
            Assert.True(dayBlock.DayNumberText == "");
            Assert.True(dayBlock.Color == Brushes.LightBlue);
        }
    }
}
