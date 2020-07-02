namespace SVCalendar.WPF.View
{
    using SVCalendar.Model;

    public class SignInViewModel : BindableBase
    {
        #region Constants, Fields

        private readonly ChangeUserCallback changeUserCallback;

        private readonly IEventsRepository eventsRepository;

        private string usernameText;

        #endregion

        public SignInViewModel(IEventsRepository eventsRepository, ChangeUserCallback changeUser)
        {
            this.eventsRepository = eventsRepository;
            changeUserCallback = changeUser;

            SignInCommand = new RelayCommand(OnSignInSelected, CanSignIn);
            UsernameText = null;
        }

        public delegate void ChangeUserCallback(User user);

        #region Events, Interfaces, Properties

        public RelayCommand SignInCommand
        {
            get;
        }

        public string UsernameText
        {
            get => usernameText;
            set
            {
                SetProperty(ref usernameText, value);
                SignInCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Methods

        private bool CanSignIn()
        {
            return !string.IsNullOrEmpty(UsernameText);
        }

        private void OnSignInSelected()
        {
            User user = eventsRepository.GetUserByName(UsernameText);
            if (user == null)
            {
                user = new User
                           {
                               Name = UsernameText
                           };
                eventsRepository.AddUser(user);
                user = eventsRepository.GetUserByName(UsernameText);
            }

            changeUserCallback(user);
        }

        #endregion
    }
}