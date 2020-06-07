namespace SVCalendar.WPF.View
{
    using System;
    using System.Collections.ObjectModel;

    using SVCalendar.Model;

    internal class EditEventsViewModel : BindableBase
    {
        private readonly IEventsRepository eventsRepository;

        private readonly User currentUser;

        private Event selectedEvent;

        public EditEventsViewModel(IEventsRepository eventsRepository, User currentUser)
        {
            this.eventsRepository = eventsRepository;
            this.currentUser = currentUser;

            InitializeEvents();

            SaveEventCommand = new RelayCommand(OnSaveEventSelected, CanSaveEvent);
            DeleteEventCommand = new RelayCommand(OnDeleteEventSelected, CanDeleteEvent);

            SelectedEvent = null;
        }

        public string SelectedEventTitle
        {
            get => SelectedEvent?.Title ?? string.Empty;
            set
            {
                SelectedEvent.Title = value;
                OnPropertyChanged();
                SaveEventCommand.RaiseCanExecuteChanged();
            }
        }

        public string SelectedEventDescription
        {
            get => SelectedEvent?.Description ?? string.Empty;
            set
            {
                SelectedEvent.Description = value;
                OnPropertyChanged();
                SaveEventCommand.RaiseCanExecuteChanged();
            }
        }

        public DateTime SelectedEventStartDate
        {
            get => SelectedEvent?.StartDate ?? DateTime.Now;
            set
            {
                SelectedEvent.StartDate = value;
                OnPropertyChanged();
                SaveEventCommand.RaiseCanExecuteChanged();
            }
        }

        public DateTime SelectedEventEndDate
        {
            get => SelectedEvent?.EndDate ?? DateTime.Now.AddMinutes(30);
            set
            {
                SelectedEvent.EndDate = value;
                OnPropertyChanged();
                SaveEventCommand.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<Event> Events
        {
            get; set;
        }

        public Event SelectedEvent
        {
            get => selectedEvent;
            set
            {
                SetProperty(ref selectedEvent, value);
                RefreshEditEventFields();
                DeleteEventCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand DeleteEventCommand
        {
            get; set;
        }

        public RelayCommand SaveEventCommand
        {
            get; set;
        }

        private void InitializeEvents()
        {
            var events = eventsRepository.GetUserOwnedEvents(currentUser);
            Events = new ObservableCollection<Event>(events);
        }

        private bool CanDeleteEvent()
        {
            return SelectedEvent != null;
        }

        private void OnDeleteEventSelected()
        {
            eventsRepository.DeleteEvent(SelectedEvent);
            Events.Remove(SelectedEvent);
            SelectedEvent = null;
        }

        private bool CanSaveEvent()
        {
            return true;
        }

        private void OnSaveEventSelected()
        {
            eventsRepository.UpdateEvent(SelectedEvent);
        }

        private void RefreshEditEventFields()
        {
            if (SelectedEvent != null)
            {
                SelectedEventTitle = SelectedEvent.Title;
                SelectedEventDescription = SelectedEvent.Description;
                SelectedEventStartDate = SelectedEvent.StartDate;
                SelectedEventEndDate = SelectedEvent.EndDate;
            }
        }
    }
}