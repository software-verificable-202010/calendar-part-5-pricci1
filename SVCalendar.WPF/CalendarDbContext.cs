using Microsoft.EntityFrameworkCore;
using SVCalendar.Model;

namespace SVCalendar.WPF
{
    public class CalendarDbContext : DbContext
    {
        public DbSet<Event> Events { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserEvent> UserEvents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=events.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasAlternateKey(user => user.Name);
            modelBuilder.Entity<UserEvent>().HasKey(userEvent => new { userEvent.EventId, userEvent.UserId });
            modelBuilder.Entity<Event>().HasIndex(e => e.StartDate);
            modelBuilder.Entity<User>().HasData(new User() { Name = "cal", UserId = -1 });
        }
    }
}