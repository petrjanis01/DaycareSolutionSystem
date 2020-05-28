using DaycareSolutionSystem.Database.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace DaycareSolutionSystem.Database.DataContext
{
    public class DssDataContext : DbContext
    {
        public DssDataContext(DbContextOptions<DssDataContext> options) : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // No need to disable initializer as in previous versions of EF. EF core does not create/alter db schema automatically.
            // Use lazy loading by default. Handled by using proxies on entity class level
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Make user login name unique.
            builder.Entity<User>()
                .HasIndex(u => u.LoginName)
                .IsUnique();
        }

        public virtual DbSet<DaycareSolutionSystem.Database.Entities.Entities.Action> Actions { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<AgreedClientAction> AgreedClientActions { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Coordinates> Coordinates { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<IndividualPlan> IndividualPlans { get; set; }
        public virtual DbSet<Picture> Pictures { get; set; }
        public virtual DbSet<RegisteredClientAction> RegisteredClientActions { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
