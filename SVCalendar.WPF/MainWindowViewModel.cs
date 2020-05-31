using SVCalendar.WPF.View;

namespace SVCalendar.WPF
{
    class MainWindowViewModel : BindableBase
    {
        private readonly AddEventViewModel addEventViewModel;

        private BindableBase currentViewModel;

        private readonly IEventsRepository eventsRepository;
        private MonthGridViewModel monthGridViewModel;
        private WeekGridViewModel weekGridViewModel;

        public MainWindowViewModel()
        {
            eventsRepository = new EventsRepository();
            WeekGridViewModel = new WeekGridViewModel(eventsRepository);
            MonthGridViewModel = new MonthGridViewModel(eventsRepository);
            addEventViewModel = new AddEventViewModel(eventsRepository);
            CurrentViewModel = MonthGridViewModel;

            ChangeCalendarModeCommand = new RelayCommand<CalendarMode>(OnChangeCalendarModeSelected);
            ShowAddEventCommand = new RelayCommand(OnShowAddEventSelected);
            RefreshCommand = new RelayCommand(OnRefreshSelected);
        }

        private MonthGridViewModel MonthGridViewModel
        {
            get => monthGridViewModel;
            set => SetProperty(ref monthGridViewModel, value);
        }

        private WeekGridViewModel WeekGridViewModel
        {
            get => weekGridViewModel;
            set => SetProperty(ref weekGridViewModel, value);
        }

        public RelayCommand<CalendarMode> ChangeCalendarModeCommand { get; }

        public BindableBase CurrentViewModel
        {
            get => currentViewModel;
            set => SetProperty(ref currentViewModel, value);
        }

        public RelayCommand ShowAddEventCommand { get; }

        public RelayCommand RefreshCommand { get; }

        private void OnRefreshSelected()
        {
            WeekGridViewModel = new WeekGridViewModel(eventsRepository);
            MonthGridViewModel = new MonthGridViewModel(eventsRepository);
        }

        private void OnShowAddEventSelected()
        {
            CurrentViewModel = addEventViewModel;
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