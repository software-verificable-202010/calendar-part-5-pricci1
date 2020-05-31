using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using SVCalendar.Model;
using SVCalendar.WPF.Annotations;

namespace SVCalendar.WPF.View
{
    class MonthGridViewModel : BindableBase
    {
        private readonly string[] monthNames =
        {
            "January", "February", "March", "April", "May", "June", "July", "August", "September", "October",
            "November", "December"
        };

        private DateTime currentDate;

        private List<DayBlock> monthDays;

        private string monthYearText;

        public MonthGridViewModel(IEventsRepository eventsRepository)
        {
            Events = eventsRepository.GetEvents();
            CurrentDate = DateTime.Today;
            MonthDays = InitializeDays();
            NextMonthCommand = new RelayCommand(OnNextMonthSelected);
            PreviousMonthCommand = new RelayCommand(OnPreviousMonthSelected);
        }

        public List<Event> Events { get; set; }

        public List<DayBlock> MonthDays
        {
            get => monthDays;
            set
            {
                monthDays = value;
                OnPropertyChanged();
            }
        }

        public DateTime CurrentDate
        {
            get => currentDate;
            private set
            {
                currentDate = value;
                OnPropertyChanged();
                MonthYearText = $"{monthNames[value.Month - 1]} {value.Year}";
            }
        }

        public string MonthYearText
        {
            get => monthYearText;
            private set
            {
                monthYearText = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand NextMonthCommand { get; }

        public RelayCommand PreviousMonthCommand { get; }

        private void OnPreviousMonthSelected()
        {
            CurrentDate = CurrentDate.AddMonths(-1);
            MonthDays = InitializeDays();
        }

        private void OnNextMonthSelected()
        {
            CurrentDate = CurrentDate.AddMonths(1);
            MonthDays = InitializeDays();
        }

        public List<DayBlock> InitializeDays()
        {
            var monthDays = new List<DayBlock>();
            int daysInCurrentMonth = DateTime.DaysInMonth(CurrentDate.Year, CurrentDate.Month);
            DateTime firstDayOfCurrentMonth = CurrentDate.AddDays(-1 * CurrentDate.Day + 1);
            var firstWeekDayOfCurrentMonth = (int) CurrentDate.AddDays(-1 * CurrentDate.Day + 1).DayOfWeek;

            int adjustedFirstWeekDayOfCurrentMonth =
                AdjustFirstWeekDayOfCurrentMonth(firstWeekDayOfCurrentMonth);

            for (var i = 1; i < adjustedFirstWeekDayOfCurrentMonth + 1; i++)
            {
                monthDays.Add(new DayBlock());
            }

            for (var i = 1; i <= daysInCurrentMonth; i++)
            {
                monthDays.Add(new DayBlock(firstDayOfCurrentMonth.AddDays(i - 1), Events));
            }

            return monthDays;
        }

        private int AdjustFirstWeekDayOfCurrentMonth(int firstWeekDayOfCurrentMonth)
        {
            const int SundayOldValue = 0;
            const int SundayNewValue = 6;
            return firstWeekDayOfCurrentMonth == SundayOldValue ? SundayNewValue : firstWeekDayOfCurrentMonth - 1;
        }
    }

    internal class DayBlock
    {
        private readonly bool dayBlockHasDate;

        public DayBlock(DateTime? date = null, [CanBeNull] List<Event> events = null)
        {
            Date = date;
            dayBlockHasDate = date != null;

            InitializeDayBlock(events);
        }

        private void InitializeDayBlock(List<Event> events)
        {
            if (dayBlockHasDate && events != null)
            {
                SetDayEventsCount(events);
            }
            else
            {
                EventsCountDisplay = "";
            }
        }

        public string EventsCountDisplay { get; set; }

        public DateTime? Date { get; set; }

        public int DayNumber => Date?.Day ?? -1;

        public string DayNumberText => DayNumber > 0 ? DayNumber.ToString() : "";

        public SolidColorBrush Color =>
            Date?.DayOfWeek switch
                {
                    DayOfWeek.Saturday => Brushes.LightCoral,
                    DayOfWeek.Sunday => Brushes.LightCoral,
                    _ => Brushes.LightBlue
                };

        private void SetDayEventsCount(List<Event> events)
        {
            int numberEvents =
                events.Count(anEvent => dayBlockHasDate && EventHappensInCurrentDay(anEvent, (DateTime)Date));
            EventsCountDisplay = numberEvents > 0 ? new string('•', numberEvents) : "";
        }

        private bool EventHappensInCurrentDay(Event anEvent, DateTime date)
        {
            {
                bool eventStartsBefore = DateTime.Compare(anEvent.StartDate.Date, date.Date) <= 0;
                bool eventEndsAfter = DateTime.Compare(date.Date, anEvent.EndDate.Date) <= 0;
                return eventStartsBefore && eventEndsAfter;
            }
        }
    }
}