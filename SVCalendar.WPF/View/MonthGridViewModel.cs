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
        private readonly string[] _monthNames =
        {
            "January", "February", "March", "April", "May", "June", "July", "August", "September", "October",
            "November", "December"
        };

        private DateTime _currentDate;

        private List<DayBlock> _monthDays;

        private string _monthYearText;

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
            get => _monthDays;
            set
            {
                _monthDays = value;
                OnPropertyChanged();
            }
        }

        public DateTime CurrentDate
        {
            get => _currentDate;
            private set
            {
                _currentDate = value;
                OnPropertyChanged();
                MonthYearText = $"{_monthNames[value.Month - 1]} {value.Year}";
            }
        }

        public string MonthYearText
        {
            get => _monthYearText;
            private set
            {
                _monthYearText = value;
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
            // Make 0 = Monday, ... 6 = Sunday
            var sundayOldValue = 0;
            var sundayNewValue = 6;
            return firstWeekDayOfCurrentMonth == sundayOldValue ? sundayNewValue : firstWeekDayOfCurrentMonth - 1;
        }
    }

    internal class DayBlock
    {
        public DayBlock(DateTime? date = null, [CanBeNull] List<Event> events = null)
        {
            Date = date;

            if (date != null && events != null)
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

        public SolidColorBrush Color => Date?.DayOfWeek switch
        {
            DayOfWeek.Saturday => Brushes.LightCoral,
            DayOfWeek.Sunday => Brushes.LightCoral,
            _ => Brushes.LightBlue
        };

        private void SetDayEventsCount(List<Event> events)
        {
            int numberEvents =
                events.Count(anEvent => Date != null && EventHappensInCurrentDay(anEvent, (DateTime) Date));
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