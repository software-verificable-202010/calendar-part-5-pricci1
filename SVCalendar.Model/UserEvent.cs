namespace SVCalendar.Model
{
    public class UserEvent
    {
        #region Events, Interfaces, Properties

        public Event Event { get; set; }

        public int EventId { get; set; }

        public User User { get; set; }

        public int UserId { get; set; }

        #endregion
    }
}
