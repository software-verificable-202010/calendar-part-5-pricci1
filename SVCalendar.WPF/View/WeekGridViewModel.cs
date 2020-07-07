using System;
using System.Collections.Generic;
using System.Linq;
using SVCalendar.Model;

namespace SVCalendar.WPF.View
{
    class WeekGridViewModel : BindableBase
    {
        #region Constants, Fields

        private readonly string[] monthNames =
        {
            "January", "February", "March", "April", "May", "June", "July", "August", "September", "October",
            "November", "December"
        };

        private DateTime currentDay;

        private string monthYearText;

        private List<WeekDay> weekDays;

        #endregion

        public WeekGridViewModel(IEventsRepository eventsRepository, User currentUser)
        {
            CurrentDay = DateTime.Today;
            Events = eventsRepository.GetUserEvents(currentUser); // TODO: do everything with SQL.

            NextWeekCommand = new RelayCommand(OnNextWeekSelected);
            PreviousWeekCommand = new RelayCommand(OnPreviousWeekSelected);

            InitializeWeekDays();
        }

        #region Events, Interfaces, Properties

        public DateTime CurrentDay
        {
            get => currentDay;
            set
            {
                currentDay = value;
                MonthYearText = $"{monthNames[value.Month - 1]} {value.Year}";
            }
        }

        public IEnumerable<Event> Events
        {
            get; set;
        }

        public string MonthYearText
        {
            get => monthYearText;
            set => SetProperty(ref monthYearText, value);
        }

        public RelayCommand NextWeekCommand
        {
            get; set;
        }

        public RelayCommand PreviousWeekCommand
        {
            get; set;
        }

        public List<WeekDay> WeekDays
        {
            get => weekDays;
            set => SetProperty(ref weekDays, value);
        }

        #endregion

        #region Methods

        private void OnNextWeekSelected()
        {
            CurrentDay = CurrentDay.AddDays(7);
            InitializeWeekDays();
        }

        private void OnPreviousWeekSelected()
        {
            CurrentDay = CurrentDay.AddDays(-7);
            InitializeWeekDays();
        }

        private void InitializeWeekDays()
        {
            var tempWeekDays = new List<WeekDay>();
            DateTime monday = GetMondayOfCurrentWeek(CurrentDay);
            const int daysInWeek = 7;
            for (var i = 0; i < daysInWeek; i++)
            {
                DateTime day = monday.AddDays(i);
                tempWeekDays.Add(new WeekDay(day, Events));
            }

            WeekDays = tempWeekDays;
        }

        private static DateTime GetMondayOfCurrentWeek(DateTime currentDay)
        {
            DayOfWeek currentDayOfWeek = currentDay.DayOfWeek;
            return currentDay.AddDays(-1 * (int) currentDayOfWeek + 1);
        }

        #endregion
    }

    internal class WeekDay
    {
        public WeekDay(DateTime day, IEnumerable<Event> events)
        {
            CurrentDay = day;

            DayName = CurrentDay.DayOfWeek.ToString();
            DayNumber = CurrentDay.Day.ToString();

            SetCurrentDayEvents(events);
            InitializeHalfHours();
        }

        #region Events, Interfaces, Properties

        public DateTime CurrentDay
        {
            get; set;
        }

        public IEnumerable<Event> DayEvents
        {
            get; set;
        }

        public string DayName
        {
            get; set;
        }

        public string DayNumber
        {
            get; set;
        }

        public List<HalfHour> HalfHours
        {
            get; set;
        }

        #endregion

        #region Methods

        private void InitializeHalfHours()
        {
            var tempHalfHours = new List<HalfHour>();
            const int halfHoursInDay = 48;
            for (var i = 0; i < halfHoursInDay; i++)
            {
                DateTime halfHour = CurrentDay.AddMinutes(i * 30);
                tempHalfHours.Add(new HalfHour(DayEvents, halfHour));
            }

            HalfHours = tempHalfHours;
        }

        private void SetCurrentDayEvents(IEnumerable<Event> events)
        {
            DayEvents = events.Where(EventHappensInCurrentDay);
        }

        private bool EventHappensInCurrentDay(Event anEvent)
        {
            bool eventStartsBefore = DateTime.Compare(anEvent.StartDate.Date, CurrentDay.Date) <= 0;
            bool eventEndsAfter = DateTime.Compare(CurrentDay.Date, anEvent.EndDate.Date) <= 0;
            return eventStartsBefore && eventEndsAfter;
        }

        #endregion
    }

    public class HalfHour
    {
        #region Constants, Fields

        private readonly IEnumerable<Event> dayEvents;

        #endregion

        public HalfHour(IEnumerable<Event> dayEvents, DateTime correspondingHalfHour)
        {
            this.dayEvents = dayEvents;
            CorrespondingHalfHour = correspondingHalfHour;
            SetFormattedTimeText();
            SetCorrespondingEvents();
        }

        #region Events, Interfaces, Properties

        public DateTime CorrespondingHalfHour
        {
            get;
        }

        public List<Event> Events { get; set; }

        public string TimeText { get; set; }

        #endregion

        #region Methods

        private void SetCorrespondingEvents()
        {
            Events = new List<Event>();
            IEnumerable<Event> events = dayEvents.Where(IsHalfHourInsideEventTime);
            Events.AddRange(events);
        }

        private bool IsHalfHourInsideEventTime(Event anEvent)
        {
            bool eventEndsAfter = DateTime.Compare(CorrespondingHalfHour, anEvent.EndDate) <= 0;
            bool eventStartsBefore = DateTime.Compare(anEvent.StartDate.AddMinutes(-30), CorrespondingHalfHour) <= 0;
            return eventStartsBefore && eventEndsAfter;
        }

        private void SetFormattedTimeText()
        {
            // HH:MM
            string hourText = CorrespondingHalfHour.Hour.ToString().Length > 1
                                  ? CorrespondingHalfHour.Hour.ToString()
                                  : $"0{CorrespondingHalfHour.Hour}";
            string minuteText = CorrespondingHalfHour.Minute.ToString().Length > 1
                                    ? CorrespondingHalfHour.Minute.ToString()
                                    : $"0{CorrespondingHalfHour.Minute}";
            TimeText = $"{hourText}:{minuteText} ";
        }

        #endregion
    }
}