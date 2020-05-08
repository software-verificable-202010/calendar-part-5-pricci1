using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SVCalendar.Model;

namespace SVCalendar.WPF.View
{
    class WeekGridViewModel : BindableBase
    {
        public WeekGridViewModel(IEventsRepository eventsRepository)
        {
            CurrentDay = DateTime.Today;
            MonthYearText = $"{_monthNames[CurrentDay.Month - 1]} {CurrentDay.Year}";
            Events = eventsRepository.GetEvents(); //TODO: do everything with SQL

            NextWeekCommand = new RelayCommand(OnNextWeekSelected);
            PreviousWeekCommand = new RelayCommand(OnPreviousWeekSelected);
            
            InitializeWeekDays();
        }

        public string MonthYearText
        {
            get => _monthYearText;
            set => SetProperty(ref _monthYearText, value);
        }

        private void InitializeWeekDays()
        {
            var tempWeekDays = new List<WeekDay>();
            DateTime monday = GetMondayOfCurrentWeek(CurrentDay);
            const int daysInWeek = 7;
            for (int i = 0; i < daysInWeek; i++)
            {
                var day = monday.AddDays(i);
                tempWeekDays.Add(new WeekDay(day, Events));
            }

            WeekDays = tempWeekDays;
        }

        public IEnumerable<Event> Events { get; set; }

        private DateTime GetMondayOfCurrentWeek(DateTime currentDay)
        {
            var currentDayOfWeek = currentDay.DayOfWeek;
            return currentDay.AddDays(-1 * (int) currentDayOfWeek + 1);
        }

        public DateTime CurrentDay
        {
            get => _currentDay;
            set
            {
                _currentDay = value;
                MonthYearText = $"{_monthNames[value.Month - 1]} {value.Year}";
            }
        }

        private void OnPreviousWeekSelected()
        {
            CurrentDay = CurrentDay.AddDays(-7);
            InitializeWeekDays();
        }

        private void OnNextWeekSelected()
        {
            CurrentDay = CurrentDay.AddDays(7);
            InitializeWeekDays();
        }

        public RelayCommand PreviousWeekCommand { get; set; }

        public RelayCommand NextWeekCommand { get; set; }

        private List<WeekDay> _weekDays;

        public List<WeekDay> WeekDays
        {
            get => _weekDays;
            set => SetProperty(ref _weekDays, value);
        }

        private readonly string[] _monthNames = {
            "January", "February", "March", "April", "May", "June", "July", "August", "September", "October",
            "November", "December"
        };

        private DateTime _currentDay;
        private string _monthYearText;
    }

    class WeekDay
    {
        public WeekDay(DateTime day, IEnumerable<Event> events)
        {
            CurrentDay = day;

            DayName = CurrentDay.DayOfWeek.ToString();
            DayNumber = CurrentDay.Day.ToString();

            SetCurrentDayEvents(events);
            InitializeHalfHours();
        }

        private void SetCurrentDayEvents(IEnumerable<Event> events)
        {
            DayEvents = events.Where(EventHappensInCurrentDay);
        }

        private bool EventHappensInCurrentDay(Event anEvent)
        {
            var eventStartsBefore = DateTime.Compare(anEvent.StartDate, CurrentDay) <= 0;
            var eventEndsAfter = DateTime.Compare(CurrentDay, anEvent.EndDate) <= 0;
            return eventStartsBefore && eventEndsAfter;
        }

        public DateTime CurrentDay { get; set; }

        public String DayName { get; set; }
        public String DayNumber { get; set; }

        public List<HalfHour> HalfHours { get; set; }
        public IEnumerable<Event> DayEvents { get; set; }

        private void InitializeHalfHours()
        {
            var tempHalfHours = new List<HalfHour>();
            const int halfHoursInDay = 48;
            for (int i = 0; i < halfHoursInDay; i++)
            {
                var halfHour = CurrentDay.AddMinutes(i * 30);
                tempHalfHours.Add(new HalfHour(DayEvents, halfHour));
            }

            HalfHours = tempHalfHours;
        }

    }

    public class HalfHour
    {
        private readonly IEnumerable<Event> _dayEvents;
        private DateTime CorrespondingHalfHour { get; set; }

        public HalfHour(IEnumerable<Event> dayEvents, DateTime correspondingHalfHour)
        {
            _dayEvents = dayEvents;
            CorrespondingHalfHour = correspondingHalfHour;
            TimeText = $"{CorrespondingHalfHour.Hour}:{CorrespondingHalfHour.Minute}";
            SetCorrespondingEvents();
        }

        public string TimeText { get; set; }
        public List<Event> Events { get; set; }

        private void SetCorrespondingEvents()
        {
            Events = new List<Event>();
            var events = _dayEvents.Where(IsHalfHourInsideEventTime);
            Events.AddRange(events);
        }

        private bool IsHalfHourInsideEventTime(Event anEvent)
        {
            var eventEndsAfter = DateTime.Compare(CorrespondingHalfHour, anEvent.EndDate) <= 0;
            var eventStartsBefore = DateTime.Compare(anEvent.StartDate, CorrespondingHalfHour) <= 0;
            return eventStartsBefore && eventEndsAfter;
        }
    }
}
