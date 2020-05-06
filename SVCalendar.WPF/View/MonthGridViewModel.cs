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
            MonthDays = InitializeDays();
            NextMonthCommand = new RelayCommand(OnNextMonthSelected);
            PreviousMonthCommand = new RelayCommand(OnPreviousMonthSelected);
        }

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

        private List<DayBlock> _monthDays;
        public List<DayBlock> MonthDays
        {
            get => _monthDays;
            set
            {
                _monthDays = value;
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
                MonthYearText = $"{_monthNames[value.Month - 1]} {value.Year}";
            }
        }

        public String MonthYearText
        {
            get => _monthYearText;
            private set
            {
                _monthYearText = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand NextMonthCommand { get; private set; }

        public RelayCommand PreviousMonthCommand { get; private set; }

        public List<DayBlock> InitializeDays()
        {
            var monthDays = new List<DayBlock>();
            int daysInCurrentMonth = DateTime.DaysInMonth(CurrentDate.Year, CurrentDate.Month);
            DateTime firstDayOfCurrentMonth = CurrentDate.AddDays(-1 * CurrentDate.Day + 1);
            int firstWeekDayOfCurrentMonth = (int)CurrentDate.AddDays(-1 * CurrentDate.Day + 1).DayOfWeek;
            
            int adjustedFirstWeekDayOfCurrentMonth =
                AdjustFirstWeekDayOfCurrentMonth(firstWeekDayOfCurrentMonth);

            for (int i = 1; i < adjustedFirstWeekDayOfCurrentMonth + 1; i++)
            {
                monthDays.Add(new DayBlock(null));
            }

            for (int i = 1; i <= daysInCurrentMonth; i++)
            {
                monthDays.Add(new DayBlock(firstDayOfCurrentMonth.AddDays(i - 1)));
            }

            return monthDays;
        }

        private int AdjustFirstWeekDayOfCurrentMonth(int firstWeekDayOfCurrentMonth)
        {
            // Make 0 = Monday, ... 6 = Sunday
            int sundayOldValue = 0;
            int sundayNewValue = 6;
            return firstWeekDayOfCurrentMonth == sundayOldValue ? sundayNewValue : firstWeekDayOfCurrentMonth - 1;
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

        private string _monthYearText;
    }

    internal class DayBlock
    {
        public DayBlock(DateTime? date = null)
        {
            Date = date;
        }

        public DateTime? Date { get; set; }
        public int DayNumber => Date?.Day ?? -1;
        public String DayNumberText => DayNumber > 0 ? DayNumber.ToString() : "";
        public SolidColorBrush Color => Date?.DayOfWeek switch
        {
            DayOfWeek.Saturday => Brushes.LightCoral,
            DayOfWeek.Sunday => Brushes.LightCoral,
            _ => Brushes.LightBlue
        };
    }
}
