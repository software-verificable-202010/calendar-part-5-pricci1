using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using SVCalendar.Model;
using SVCalendar.WPF.View;

namespace SVCalendar.WPF
{
    class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
            _repo = new EventsRepository();
            _weekGridViewModel = new WeekGridViewModel(_repo);
            _monthGridViewModel = new MonthGridViewModel(_repo);
            _addEventViewModel = new AddEventViewModel(_repo);
            CurrentViewModel = _monthGridViewModel;
            
            ChangeCalendarModeCommand = new RelayCommand<CalendarMode>(OnChangeCalendarModeSelected);
            ShowAddEventCommand = new RelayCommand(OnShowAddEventSelected);

            Trace.WriteLine(_repo.GetEvents().Count);
        }

        private void OnShowAddEventSelected() => CurrentViewModel = _addEventViewModel;

        private EventsRepository _repo;
        private readonly MonthGridViewModel _monthGridViewModel;
        private readonly WeekGridViewModel _weekGridViewModel;
        private readonly AddEventViewModel _addEventViewModel;

        public RelayCommand<CalendarMode> ChangeCalendarModeCommand { get; private set; }
        


        private BindableBase _currentViewModel;
        public BindableBase CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public RelayCommand ShowAddEventCommand { get; }

        private void OnChangeCalendarModeSelected(CalendarMode mode)
        {
            CurrentViewModel = mode switch
            {
                CalendarMode.Monthly => _monthGridViewModel,
                CalendarMode.Weekly => _weekGridViewModel,
                _ => CurrentViewModel
            };
        }
    }

    enum CalendarMode
    {
        Monthly,
        Weekly
    }
}
