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
            
            _eventsRepository = new EventsRepository();
            WeekGridViewModel = new WeekGridViewModel(_eventsRepository);
            MonthGridViewModel = new MonthGridViewModel(_eventsRepository);
            _addEventViewModel = new AddEventViewModel(_eventsRepository);
            CurrentViewModel = MonthGridViewModel;
            
            ChangeCalendarModeCommand = new RelayCommand<CalendarMode>(OnChangeCalendarModeSelected);
            ShowAddEventCommand = new RelayCommand(OnShowAddEventSelected);
            RefreshCommand = new RelayCommand(OnRefreshSelected);
        }

        private IEventsRepository _eventsRepository;

        private void OnRefreshSelected()
        {
            WeekGridViewModel = new WeekGridViewModel(_eventsRepository);
            MonthGridViewModel = new MonthGridViewModel(_eventsRepository);
        }

        private void OnShowAddEventSelected() => CurrentViewModel = _addEventViewModel;

        private MonthGridViewModel MonthGridViewModel
        {
            get => _monthGridViewModel;
            set => SetProperty(ref _monthGridViewModel, value);
        }

        private WeekGridViewModel WeekGridViewModel
        {
            get => _weekGridViewModel;
            set => SetProperty(ref _weekGridViewModel, value);
        }

        private readonly AddEventViewModel _addEventViewModel;

        public RelayCommand<CalendarMode> ChangeCalendarModeCommand { get; private set; }
        


        private BindableBase _currentViewModel;
        private MonthGridViewModel _monthGridViewModel;
        private WeekGridViewModel _weekGridViewModel;

        public BindableBase CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public RelayCommand ShowAddEventCommand { get; }

        public RelayCommand RefreshCommand { get; }

        private void OnChangeCalendarModeSelected(CalendarMode mode)
        {
            CurrentViewModel = mode switch
            {
                CalendarMode.Monthly => MonthGridViewModel,
                CalendarMode.Weekly => WeekGridViewModel,
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
