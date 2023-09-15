using backend_test.Models;
using Microsoft.EntityFrameworkCore;

namespace backend_test.Data
{
    public class AirlineDbContext : DbContext, IDbContext
    {
        public AirlineDbContext(DbContextOptions<AirlineDbContext> options) : base(options)
        {
            Database.Migrate();
            // Or Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        public DbSet<Route> Routes { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Route>()
                .HasMany<Flight>(g => g.Flights)
                .WithOne(s => s.route)
                .HasForeignKey(s => s.RouteId);
            
            modelBuilder.Entity<Subscription>().ToTable("Subscription");


            //seed
            // modelBuilder.Entity<Route>().HasData(new Route(1));
            // modelBuilder.Entity<Route>().HasData(new Route(2));
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}