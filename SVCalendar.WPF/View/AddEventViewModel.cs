namespace SVCalendar.WPF.View
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using SVCalendar.Model;
    using SVCalendar.WPF.Annotations;

    public class AddEventViewModel : BindableBase
    {
        #region Constants, Fields

        private readonly User currentUser;

        private readonly IEventsRepository eventsRepository;

        private ObservableCollection<User> invitableUsers;

        private ObservableCollection<User> invitedUsers;

        private bool invitesNeedRefresh;

        private Event newEvent;

        [CanBeNull]
        private User selectedUser;

        #endregion

        public AddEventViewModel(IEventsRepository eventsRepository, User currentUser)
        {
            this.eventsRepository = eventsRepository;
            this.currentUser = currentUser;
            SaveEventCommand = new RelayCommand(OnSaveEventSelected, CanSaveEvent);
            ResetEventCommand = new RelayCommand(OnResetEventSelected);
            InviteUserCommand = new RelayCommand(OnInviteUserSelected, CanInviteUser);
            RefreshInvitesCommand = new RelayCommand(OnRefreshInvitesSelected);
            InitializeNewEvent();
            InitializeInvites();
        }

        #region Events, Interfaces, Properties

        public string EventDescription
        {
            get => NewEvent.Description;
            set
            {
                NewEvent.Description = value;
                OnPropertyChanged();
            }
        }

        public DateTime EventEndDate
        {
            get => NewEvent.EndDate;
            set
            {
                NewEvent.EndDate = value;
                InvitesNeedRefresh = true;
                OnPropertyChanged();
                SaveEventCommand.RaiseCanExecuteChanged();
            }
        }

        public DateTime EventStartDate
        {
            get => NewEvent.StartDate;
            set
            {
                NewEvent.StartDate = value;
                InvitesNeedRefresh = true;
                OnPropertyChanged();
                SaveEventCommand.RaiseCanExecuteChanged();
            }
        }

        public string EventTitle
        {
            get => NewEvent.Title;
            set
            {
                NewEvent.Title = value;
                OnPropertyChanged();
                SaveEventCommand.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<User> InvitableUsers
        {
            get => invitableUsers;
            set
            {
                invitableUsers?.Remove(currentUser);
                SetProperty(ref invitableUsers, value);
            }
        }

        public ObservableCollection<User> InvitedUsers
        {
            get => invitedUsers;
            set => SetProperty(ref invitedUsers, value);
        }

        public bool InvitesNeedRefresh
        {
            get => invitesNeedRefresh;
            set => SetProperty(ref invitesNeedRefresh, value);
        }

        public RelayCommand InviteUserCommand
        {
            get;
        }

        public Event NewEvent
        {
            get => newEvent;
            set => SetProperty(ref newEvent, value);
        }

        public RelayCommand RefreshInvitesCommand
        {
            get; set;
        }

        public RelayCommand ResetEventCommand
        {
            get;
        }

        public RelayCommand SaveEventCommand
        {
            get;
        }

        [CanBeNull]
        public User SelectedUser
        {
            get => selectedUser;
            set
            {
                if (value == currentUser)
                {
                    selectedUser = null;
                    return;
                }

                SetProperty(ref selectedUser, value);
                InviteUserCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Methods

        private void AddInvitedUsersToEvent()
        {
            foreach (User invited in InvitedUsers)
            {
                var userEvent = new UserEvent
                                    {
                                        Event = NewEvent, User = invited
                                    };
                NewEvent.UserEvents.Add(userEvent);
            }
        }

        private bool CanInviteUser()
        {
            return SelectedUser != null && !InvitesNeedRefresh;
        }

        private bool CanSaveEvent()
        {
            if (string.IsNullOrWhiteSpace(NewEvent.Title))
            {
                return false;
            }

            if (!NewEvent.StartDateIsEarlierThanEndDate())
            {
                return false;
            }

            if (InvitesNeedRefresh)
            {
                return false;
            }

            return true;
        }

        private void OnInviteUserSelected()
        {
            InvitedUsers.Add(SelectedUser);
            InvitableUsers.Remove(SelectedUser);
            SelectedUser = null;
        }

        private void OnRefreshInvitesSelected()
        {
            InitializeInvites();
            RefreshInvitesCommand.RaiseCanExecuteChanged();
            SaveEventCommand.RaiseCanExecuteChanged();
        }

        private void OnResetEventSelected()
        {
            InitializeInvites();
            InitializeNewEvent();
            SaveEventCommand.RaiseCanExecuteChanged();
        }

        private void OnSaveEventSelected()
        {
            AddInvitedUsersToEvent();

            eventsRepository.AddEvent(NewEvent);
            InitializeInvites();
            InitializeNewEvent();
            SaveEventCommand.RaiseCanExecuteChanged();
        }

        private void InitializeInvites()
        {
            SelectedUser = null;
            InvitedUsers = new ObservableCollection<User>();
            List<User> usersThatCanBeInvited =
                eventsRepository.GetAllUsersAvailableBetweenDates(EventStartDate, EventEndDate);
            InvitableUsers = new ObservableCollection<User>(usersThatCanBeInvited);
            InvitesNeedRefresh = false;
        }

        private void InitializeNewEvent()
        {
            NewEvent = new Event { Owner = currentUser };
            NewEvent.UserEvents = new List<UserEvent>
                                      {
                                          new UserEvent
                                              {
                                                  Event = NewEvent, User = currentUser
                                              }
                                      };
            EventTitle = "";
            EventDescription = "";
            EventStartDate = DateTime.Now;
            EventEndDate = DateTime.Now.AddMinutes(31);
        }

        #endregion
    }
}