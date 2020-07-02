using SVCalendar.WPF.View;

namespace SVCalendar.WPF
{
    using SVCalendar.Model;

    internal class MainWindowViewModel : BindableBase
    {
        #region 

        private AddEventViewModel addEventViewModel;

        private BindableBase currentViewModel;

        private readonly IEventsRepository eventsRepository;
        private MonthGridViewModel monthGridViewModel;
        private WeekGridViewModel weekGridViewModel;

        private User currentUser;

        #endregion

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

        #region 

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

        #endregion

        #region 

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

        #endregion
    }

    internal enum CalendarMode
    {
        Monthly,
        Weekly
    }
}