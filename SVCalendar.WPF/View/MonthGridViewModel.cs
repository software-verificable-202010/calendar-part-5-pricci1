using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Media;
using SVCalendar.WPF.Annotations;

namespace SVCalendar.WPF.View
{
    class MonthGridViewModel : INotifyPropertyChanged
    {

        public MonthGridViewModel()
        {
            CurrentDate = DateTime.Today;
            MonthDaysList = InitializeDays();
            NextMonthCommand = new RelayCommand(OnNextMonth);
            PreviousMonthCommand = new RelayCommand(OnPreviousMonth);
        }

        private void OnPreviousMonth()
        {
            CurrentDate = CurrentDate.AddMonths(-1);
            MonthDaysList = InitializeDays();
        }

        private void OnNextMonth()
        {
            CurrentDate = CurrentDate.AddMonths(1);
            MonthDaysList = InitializeDays();
        }

        private List<DayBlock> _monthDaysList;
        public List<DayBlock> MonthDaysList
        {
            get => _monthDaysList;
            set
            {
                _monthDaysList = value;
                OnPropertyChanged();
            }
        }

        private DateTime _currentDate;
        public DateTime CurrentDate
        {
            get => _currentDate;
            private set
            {
                _currentDate = value;
                OnPropertyChanged();
                MonthYearString = $"{_monthNames[value.Month - 1]} {value.Year}";
            }
        }

        public String MonthYearString
        {
            get => _monthYearString;
            private set
            {
                _monthYearString = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand NextMonthCommand { get; private set; }

        public RelayCommand PreviousMonthCommand { get; private set; }

        public List<DayBlock> InitializeDays()
        {
            var monthDaysList = new List<DayBlock>();
            int daysInCurrentMonth = DateTime.DaysInMonth(CurrentDate.Year, CurrentDate.Month);
            DateTime firstDayOfCurrentMonth = CurrentDate.AddDays(-1 * CurrentDate.Day + 1);
            int firstWeekDayOfCurrentMonth = (int)CurrentDate.AddDays(-1 * CurrentDate.Day + 1).DayOfWeek;
            // Make 0 = Monday, ... 6 = Sunday
            int adjustedFirstWeekDayOfCurrentMonth =
                firstWeekDayOfCurrentMonth == 0 ? 6 : firstWeekDayOfCurrentMonth - 1;

            for (int i = 1; i < adjustedFirstWeekDayOfCurrentMonth + 1; i++)
            {
                monthDaysList.Add(new DayBlock(null));
            }

            for (int i = 1; i <= daysInCurrentMonth; i++)
            {
                monthDaysList.Add(new DayBlock(firstDayOfCurrentMonth.AddDays(i - 1)));
            }

            return monthDaysList;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly string[] _monthNames = {
            "January", "February", "March", "April", "May", "June", "July", "August", "September", "October",
            "November", "December"
        };

        private string _monthYearString;
    }

    internal class DayBlock
    {
        public DayBlock(DateTime? date = null)
        {
            Date = date;
        }

        public DateTime? Date { get; set; }
        public int DayNumber => Date?.Day ?? -1;
        public String DayNumberString => DayNumber > 0 ? DayNumber.ToString() : "";
        public SolidColorBrush Color => Date?.DayOfWeek switch
        {
            DayOfWeek.Saturday => Brushes.LightCoral,
            DayOfWeek.Sunday => Brushes.LightCoral,
            _ => Brushes.LightBlue
        };
    }
}
