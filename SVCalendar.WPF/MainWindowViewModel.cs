using SVCalendar.WPF.View;

namespace SVCalendar.WPF
{
    class MainWindowViewModel : BindableBase
    {
        private readonly AddEventViewModel _addEventViewModel;

        private BindableBase _currentViewModel;

        private readonly IEventsRepository _eventsRepository;
        private MonthGridViewModel _monthGridViewModel;
        private WeekGridViewModel _weekGridViewModel;

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

        public RelayCommand<CalendarMode> ChangeCalendarModeCommand { get; }

        public BindableBase CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public RelayCommand ShowAddEventCommand { get; }

        public RelayCommand RefreshCommand { get; }

        private void OnRefreshSelected()
        {
            WeekGridViewModel = new WeekGridViewModel(_eventsRepository);
            MonthGridViewModel = new MonthGridViewModel(_eventsRepository);
        }

        private void OnShowAddEventSelected()
        {
            CurrentViewModel = _addEventViewModel;
        }

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