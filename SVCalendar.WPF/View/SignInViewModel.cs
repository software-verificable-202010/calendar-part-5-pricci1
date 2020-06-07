namespace SVCalendar.WPF.View
{
    using SVCalendar.Model;

    internal class SignInViewModel : BindableBase
    {
        private readonly IEventsRepository eventsRepository;

        private readonly ChangeUserCallback changeUserCallback;

        private string usernameText;

        public SignInViewModel(IEventsRepository eventsRepository, ChangeUserCallback changeUser)
        {
            this.eventsRepository = eventsRepository;
            changeUserCallback = changeUser;

            SignInCommand = new RelayCommand(OnSignInSelected, CanSignIn);
            UsernameText = null;
        }

        public delegate void ChangeUserCallback(User user);

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
            }

            changeUserCallback(user);
        }
    }
}