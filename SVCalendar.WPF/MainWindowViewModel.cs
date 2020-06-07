using SVCalendar.WPF.View;

namespace SVCalendar.WPF
{
    using SVCalendar.Model;

    class MainWindowViewModel : BindableBase
    {
        private AddEventViewModel addEventViewModel;

        private BindableBase currentViewModel;

        private readonly IEventsRepository eventsRepository;
        private MonthGridViewModel monthGridViewModel;
        private WeekGridViewModel weekGridViewModel;

        private User currentUser;

        public MainWindowViewModel()
        {
            eventsRepository = new EventsRepository();
            currentUser = eventsRepository.GetUserByName("cal");
            SignInViewModel.ChangeUserCallback switchCurrentUserDelegate = SwitchCurrentUser;
            var signInViewModel = new SignInViewModel(eventsRepository, switchCurrentUserDelegate);
            WeekGridViewModel = new WeekGridViewModel(eventsRepository, currentUser);
            MonthGridViewModel = new MonthGridViewModel(eventsRepository, currentUser);
            addEventViewModel = new AddEventViewModel(eventsRepository, currentUser);
            CurrentViewModel = signInViewModel;

            ChangeCalendarModeCommand = new RelayCommand<CalendarMode>(OnChangeCalendarModeSelected);
            ShowAddEventCommand = new RelayCommand(OnShowAddEventSelected);
            RefreshCommand = new RelayCommand(OnRefreshSelected);
            EditEventsCommand = new RelayCommand(OnEditEventsSelected);
        }

        private void OnEditEventsSelected()
        {
            CurrentViewModel = new EditEventsViewModel(eventsRepository, currentUser);
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

        public RelayCommand<CalendarMode> ChangeCalendarModeCommand
        {
            get;
        }

        public BindableBase CurrentViewModel
        {
            get => currentViewModel;
            set => SetProperty(ref currentViewModel, value);
        }

        public RelayCommand ShowAddEventCommand
        {
            get;
        }

        public RelayCommand RefreshCommand
        {
            get;
        }

        public RelayCommand EditEventsCommand
        {
            get;
        }

        private void SwitchCurrentUser(User newSignedIn)
        {
            currentUser = newSignedIn;
            addEventViewModel = new AddEventViewModel(eventsRepository, currentUser);
            OnRefreshSelected();
            CurrentViewModel = MonthGridViewModel;
        }

        private void OnRefreshSelected()
        {
            WeekGridViewModel = new WeekGridViewModel(eventsRepository, currentUser);
            MonthGridViewModel = new MonthGridViewModel(eventsRepository, currentUser);
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