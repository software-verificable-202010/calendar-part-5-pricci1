using System.Collections.Generic;

namespace SVCalendar.Model
{
    public class User
    {
        public int UserId { get; set; }

        public string Name { get; set; }

        public List<UserEvent> UserEvents { get; set; }
    }
}
