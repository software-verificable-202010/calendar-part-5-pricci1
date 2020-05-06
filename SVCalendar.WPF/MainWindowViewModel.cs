using System;
using System.Collections.Generic;
using System.Text;
using SVCalendar.WPF.View;

namespace SVCalendar.WPF
{
    class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
            CurrentViewModel = _monthGridViewModel;
            ChangeCalendarModeCommand = new RelayCommand<CalendarMode>(OnChangeCalendarMode);
        }
        private readonly MonthGridViewModel _monthGridViewModel = new MonthGridViewModel();
        private readonly WeekGridViewModel _weekGridViewModel = new WeekGridViewModel();

        public RelayCommand<CalendarMode> ChangeCalendarModeCommand { get; private set; }


        private BindableBase _currentViewModel;
        public BindableBase CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        private void OnChangeCalendarMode(CalendarMode mode)
        {
            switch (mode)
            {
                case CalendarMode.Monthly:
                    CurrentViewModel = _monthGridViewModel;
                    break;
                case CalendarMode.Weekly:
                    CurrentViewModel = _weekGridViewModel;
                    break;
            }
        }
    }

    enum CalendarMode
    {
        Monthly,
        Weekly
    }
}
