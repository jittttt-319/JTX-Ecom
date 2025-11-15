using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using JTX_Ecom.Models;

namespace JTX_Ecom.Data
{
    /// <summary>
    /// Database Context for JTX Concert Ticketing System
    /// This class manages the database connection and entity configurations
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets - Represent tables in the database
        public DbSet<Concert> Concerts { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Order> Orders { get; set; }

        /// <summary>
        /// Configure entity relationships and constraints
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Concert entity
            modelBuilder.Entity<Concert>(entity =>
            {
                entity.HasKey(c => c.ConcertId);
                entity.Property(c => c.Title).IsRequired().HasMaxLength(200);
                entity.Property(c => c.BasePrice).HasPrecision(18, 2);
                
                // Relationship: Concert belongs to one Venue
                entity.HasOne(c => c.Venue)
                    .WithMany(v => v.Concerts)
                    .HasForeignKey(c => c.VenueId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Index for performance
                entity.HasIndex(c => c.EventDate);
                entity.HasIndex(c => c.Genre);
            });

            // Configure Venue entity
            modelBuilder.Entity<Venue>(entity =>
            {
                entity.HasKey(v => v.VenueId);
                entity.Property(v => v.Name).IsRequired().HasMaxLength(200);
                entity.HasIndex(v => v.City);
            });

            // Configure Ticket entity
            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(t => t.TicketId);
                entity.Property(t => t.TicketNumber).IsRequired().HasMaxLength(50);
                entity.Property(t => t.Price).HasPrecision(18, 2);
                
                // Relationship: Ticket belongs to one Concert
                entity.HasOne(t => t.Concert)
                    .WithMany(c => c.Tickets)
                    .HasForeignKey(t => t.ConcertId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Relationship: Ticket belongs to one Order (optional)
                entity.HasOne(t => t.Order)
                    .WithMany(o => o.Tickets)
                    .HasForeignKey(t => t.OrderId)
                    .OnDelete(DeleteBehavior.SetNull);

                // Unique constraint on ticket number
                entity.HasIndex(t => t.TicketNumber).IsUnique();
                entity.HasIndex(t => t.Status);
            });

            // Configure Order entity
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.OrderId);
                entity.Property(o => o.OrderNumber).IsRequired().HasMaxLength(50);
                entity.Property(o => o.TotalAmount).HasPrecision(18, 2);
                
                // Relationship: Order belongs to one User
                entity.HasOne(o => o.User)
                    .WithMany(u => u.Orders)
                    .HasForeignKey(o => o.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasIndex(o => o.OrderNumber).IsUnique();
                entity.HasIndex(o => o.OrderDate);
                entity.HasIndex(o => o.CustomerEmail);
            });

            // Seed initial data
            SeedData(modelBuilder);
        }

        /// <summary>
        /// Seed initial data for testing and demonstration
        /// </summary>
        private void SeedData(ModelBuilder modelBuilder)
        {
            // Use a static date for seed data
            var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // Seed Venues
            modelBuilder.Entity<Venue>().HasData(
                new Venue
                {
                    VenueId = 1,
                    Name = "Madison Square Garden",
                    Address = "4 Pennsylvania Plaza",
                    City = "New York",
                    State = "NY",
                    ZipCode = "10001",
                    Country = "USA",
                    Capacity = 20000,
                    PhoneNumber = "(212) 465-6741",
                    CreatedAt = seedDate
                },
                new Venue
                {
                    VenueId = 2,
                    Name = "Staples Center",
                    Address = "1111 S Figueroa St",
                    City = "Los Angeles",
                    State = "CA",
                    ZipCode = "90015",
                    Country = "USA",
                    Capacity = 19000,
                    PhoneNumber = "(213) 742-7100",
                    CreatedAt = seedDate
                },
                new Venue
                {
                    VenueId = 3,
                    Name = "Blue Note",
                    Address = "131 W 3rd St",
                    City = "Chicago",
                    State = "IL",
                    ZipCode = "60601",
                    Country = "USA",
                    Capacity = 500,
                    PhoneNumber = "(312) 555-0123",
                    CreatedAt = seedDate
                },
                new Venue
                {
                    VenueId = 4,
                    Name = "Ultra Arena",
                    Address = "1 Brickell City Centre",
                    City = "Miami",
                    State = "FL",
                    ZipCode = "33131",
                    Country = "USA",
                    Capacity = 10000,
                    PhoneNumber = "(305) 555-0199",
                    CreatedAt = seedDate
                },
                new Venue
                {
                    VenueId = 5,
                    Name = "Grand Ole Opry",
                    Address = "2804 Opryland Dr",
                    City = "Nashville",
                    State = "TN",
                    ZipCode = "37214",
                    Country = "USA",
                    Capacity = 4400,
                    PhoneNumber = "(615) 871-6779",
                    CreatedAt = seedDate
                },
                new Venue
                {
                    VenueId = 6,
                    Name = "Barclays Center",
                    Address = "620 Atlantic Ave",
                    City = "Brooklyn",
                    State = "NY",
                    ZipCode = "11217",
                    Country = "USA",
                    Capacity = 19000,
                    PhoneNumber = "(917) 618-6100",
                    CreatedAt = seedDate
                }
            );

            // Seed Concerts
            modelBuilder.Entity<Concert>().HasData(
                new Concert
                {
                    ConcertId = 1,
                    Title = "Rock Festival 2025",
                    Description = "The biggest rock festival of the year featuring top rock bands",
                    EventDate = new DateTime(2025, 3, 15),
                    EventTime = new DateTime(2025, 3, 15, 19, 0, 0),
                    Artist = "Various Rock Artists",
                    Genre = "Rock",
                    BasePrice = 89.00m,
                    AvailableTickets = 15000,
                    TotalTickets = 20000,
                    VenueId = 1,
                    ImageUrl = "/assets/images/concerts/concert1.jpg",
                    CreatedAt = seedDate
                },
                new Concert
                {
                    ConcertId = 2,
                    Title = "Pop Extravaganza",
                    Description = "A spectacular pop music experience with chart-topping artists",
                    EventDate = new DateTime(2025, 4, 5),
                    EventTime = new DateTime(2025, 4, 5, 20, 0, 0),
                    Artist = "Top Pop Stars",
                    Genre = "Pop",
                    BasePrice = 125.00m,
                    AvailableTickets = 18000,
                    TotalTickets = 19000,
                    VenueId = 2,
                    ImageUrl = "/assets/images/concerts/concert2.jpg",
                    CreatedAt = seedDate
                },
                new Concert
                {
                    ConcertId = 3,
                    Title = "Jazz Night Live",
                    Description = "An intimate evening of smooth jazz and soul",
                    EventDate = new DateTime(2025, 4, 20),
                    EventTime = new DateTime(2025, 4, 20, 21, 0, 0),
                    Artist = "Jazz Ensemble",
                    Genre = "Jazz",
                    BasePrice = 65.00m,
                    AvailableTickets = 450,
                    TotalTickets = 500,
                    VenueId = 3,
                    ImageUrl = "/assets/images/concerts/concert3.jpg",
                    CreatedAt = seedDate
                },
                new Concert
                {
                    ConcertId = 4,
                    Title = "Electronic Dance Night",
                    Description = "Ultimate EDM experience with world-class DJs",
                    EventDate = new DateTime(2025, 5, 10),
                    EventTime = new DateTime(2025, 5, 10, 22, 0, 0),
                    Artist = "Top EDM DJs",
                    Genre = "Electronic",
                    BasePrice = 149.00m,
                    AvailableTickets = 9500,
                    TotalTickets = 10000,
                    VenueId = 4,
                    ImageUrl = "/assets/images/concerts/concert4.jpg",
                    CreatedAt = seedDate
                },
                new Concert
                {
                    ConcertId = 5,
                    Title = "Country Music Fest",
                    Description = "Celebrating country music heritage with legendary performers",
                    EventDate = new DateTime(2025, 6, 2),
                    EventTime = new DateTime(2025, 6, 2, 19, 30, 0),
                    Artist = "Country Legends",
                    Genre = "Country",
                    BasePrice = 79.00m,
                    AvailableTickets = 4200,
                    TotalTickets = 4400,
                    VenueId = 5,
                    ImageUrl = "/assets/images/concerts/concert5.jpg",
                    CreatedAt = seedDate
                },
                new Concert
                {
                    ConcertId = 6,
                    Title = "Hip Hop Summit",
                    Description = "The hottest hip hop artists live on stage",
                    EventDate = new DateTime(2025, 6, 21),
                    EventTime = new DateTime(2025, 6, 21, 20, 0, 0),
                    Artist = "Hip Hop All-Stars",
                    Genre = "Hip Hop",
                    BasePrice = 95.00m,
                    AvailableTickets = 17500,
                    TotalTickets = 19000,
                    VenueId = 6,
                    ImageUrl = "/assets/images/concerts/concert6.jpg",
                    CreatedAt = seedDate
                }
            );
        }
    }
}
