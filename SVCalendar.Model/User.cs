using System.Collections.Generic;

namespace SVCalendar.Model
{
    public class User
    {
        #region Events, Interfaces, Properties

        public string Name { get; set; }

        public List<UserEvent> UserEvents { get; set; }

        public int UserId { get; set; }

        #endregion
    }
}
