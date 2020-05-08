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
            _weekGridViewModel = new WeekGridViewModel(_eventsRepository);
            _monthGridViewModel = new MonthGridViewModel(_eventsRepository);
            _addEventViewModel = new AddEventViewModel(_eventsRepository);
            CurrentViewModel = _monthGridViewModel;
            
            ChangeCalendarModeCommand = new RelayCommand<CalendarMode>(OnChangeCalendarModeSelected);
            ShowAddEventCommand = new RelayCommand(OnShowAddEventSelected);
            RefreshCommand = new RelayCommand(OnRefreshSelected);
        }

        private IEventsRepository _eventsRepository;

        private void OnRefreshSelected()
        {
            _weekGridViewModel = new WeekGridViewModel(_eventsRepository);
            _monthGridViewModel = new MonthGridViewModel(_eventsRepository);
            OnPropertyChanged("_weekGridViewModel");
            OnPropertyChanged("_monthGridViewModel");
        }

        private void OnShowAddEventSelected() => CurrentViewModel = _addEventViewModel;

        private MonthGridViewModel _monthGridViewModel;
        private WeekGridViewModel _weekGridViewModel;
        private readonly AddEventViewModel _addEventViewModel;

        public RelayCommand<CalendarMode> ChangeCalendarModeCommand { get; private set; }
        


        private BindableBase _currentViewModel;
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
