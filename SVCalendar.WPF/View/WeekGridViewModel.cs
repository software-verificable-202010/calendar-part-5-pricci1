using System;
using System.Collections.Generic;
using System.Text;

namespace SVCalendar.WPF.View
{
    class WeekGridViewModel : BindableBase
    {
        public WeekGridViewModel()
        {
            WeekDays = new List<WeekDay>
            {
                new WeekDay("Monday", "4"),
                new WeekDay("Tuesday", "5"),
                new WeekDay("Wednesday", "6"),
                new WeekDay("Thursday", "7"),
                new WeekDay("Friday", "8"),
                new WeekDay("Saturday", "9"),
                new WeekDay("Sunday", "10")
            };
        }
        public List<WeekDay> WeekDays { get; set; }

    }

    class WeekDay
    {
        public WeekDay(string dayName, string dayNumber)
        {
            DayName = dayName;
            DayNumber = dayNumber;
            HalfHours = new List<string>
            {
                " "," "," "," "," "," "," "," "," "," ",
                " "," "," "," "," "," "," "," "," "," ",
                " "," "," "," "
            };
        }

        public String DayName { get; set; }
        public String DayNumber { get; set; }

        public List<String> HalfHours { get; set; }

    }
}
