using System;
using SVCalendar.Model;

namespace SVCalendar.WPF.View
{
    class AddEventViewModel : BindableBase
    {
        private readonly IEventsRepository _eventsRepository;
        private Event _newEvent;

        public AddEventViewModel(IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;

            SaveEventCommand = new RelayCommand(OnSaveEventSelected, CanSaveEvent);
            ResetEventCommand = new RelayCommand(OnResetEventSelected);
            InitializeNewEvent();
        }

        public RelayCommand SaveEventCommand { get; }
        public RelayCommand ResetEventCommand { get; }

        public Event NewEvent
        {
            get => _newEvent;
            set => SetProperty(ref _newEvent, value);
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

        public string EventDescription
        {
            get => NewEvent.Description;
            set
            {
                NewEvent.Description = value;
                OnPropertyChanged();
            }
        }

        public DateTime EventStartDate
        {
            get => NewEvent.StartDate;
            set
            {
                NewEvent.StartDate = value;
                OnPropertyChanged();
                SaveEventCommand.RaiseCanExecuteChanged();
            }
        }

        public DateTime EventEndDate
        {
            get => NewEvent.EndDate;
            set
            {
                NewEvent.EndDate = value;
                OnPropertyChanged();
                SaveEventCommand.RaiseCanExecuteChanged();
            }
        }

        private void InitializeNewEvent()
        {
            NewEvent = new Event();
            EventTitle = "";
            EventDescription = "";
            EventStartDate = DateTime.Now;
            EventEndDate = DateTime.Now;
        }

        private void OnResetEventSelected()
        {
            InitializeNewEvent();
            SaveEventCommand.RaiseCanExecuteChanged();
        }

        private bool CanSaveEvent()
        {
            if (string.IsNullOrWhiteSpace(NewEvent.Title)) return false;
            if (!NewEvent.StartDateIsEarlierThanEndDate()) return false;
            return true;
        }

        private void OnSaveEventSelected()
        {
            _eventsRepository.AddEvent(NewEvent);
            InitializeNewEvent();
            SaveEventCommand.RaiseCanExecuteChanged();
        }
    }
}