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
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

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

            // Configure UserProfile entity
            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.HasKey(p => p.ProfileId);
                
                entity.HasOne(p => p.User)
                    .WithOne(u => u.Profile)
                    .HasForeignKey<UserProfile>(p => p.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasIndex(p => p.UserId).IsUnique();
            });

            // Configure Cart entity
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(c => c.CartId);
                
                entity.HasOne(c => c.User)
                    .WithOne(u => u.Cart)
                    .HasForeignKey<Cart>(c => c.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasIndex(c => c.UserId).IsUnique();
            });

            // Configure CartItem entity
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(ci => ci.CartItemId);
                entity.Property(ci => ci.PricePerTicket).HasPrecision(18, 2);
                
                entity.HasOne(ci => ci.Cart)
                    .WithMany(c => c.CartItems)
                    .HasForeignKey(ci => ci.CartId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(ci => ci.Concert)
                    .WithMany()
                    .HasForeignKey(ci => ci.ConcertId)
                    .OnDelete(DeleteBehavior.Restrict);
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

            // Seed Venues - Malaysian venues
            modelBuilder.Entity<Venue>().HasData(
                new Venue
                {
                    VenueId = 1,
                    Name = "Bukit Jalil National Stadium",
                    Address = "Bukit Jalil",
                    City = "Kuala Lumpur",
                    State = "Wilayah Persekutuan",
                    ZipCode = "57000",
                    Country = "Malaysia",
                    Capacity = 87411,
                    PhoneNumber = "03-8992 9000",
                    CreatedAt = seedDate
                },
                new Venue
                {
                    VenueId = 2,
                    Name = "Axiata Arena",
                    Address = "Jalan Barat, Bukit Jalil",
                    City = "Kuala Lumpur",
                    State = "Wilayah Persekutuan",
                    ZipCode = "57000",
                    Country = "Malaysia",
                    Capacity = 16000,
                    PhoneNumber = "03-8992 8833",
                    CreatedAt = seedDate
                },
                new Venue
                {
                    VenueId = 3,
                    Name = "Mega Star Arena",
                    Address = "Jalan Lagenda",
                    City = "Kuala Lumpur",
                    State = "Wilayah Persekutuan",
                    ZipCode = "59200",
                    Country = "Malaysia",
                    Capacity = 3500,
                    PhoneNumber = "03-7784 8000",
                    CreatedAt = seedDate
                },
                new Venue
                {
                    VenueId = 4,
                    Name = "Sepang International Circuit",
                    Address = "Jalan Pekeliling",
                    City = "Sepang",
                    State = "Selangor",
                    ZipCode = "64000",
                    Country = "Malaysia",
                    Capacity = 130000,
                    PhoneNumber = "03-8778 2222",
                    CreatedAt = seedDate
                },
                new Venue
                {
                    VenueId = 5,
                    Name = "Borneo Convention Centre Kuching",
                    Address = "Jalan Tunku Abdul Rahman",
                    City = "Kuching",
                    State = "Sarawak",
                    ZipCode = "93100",
                    Country = "Malaysia",
                    Capacity = 5000,
                    PhoneNumber = "082-423600",
                    CreatedAt = seedDate
                },
                new Venue
                {
                    VenueId = 6,
                    Name = "Penang International Sports Arena",
                    Address = "Jalan Pemancar",
                    City = "Bayan Lepas",
                    State = "Pulau Pinang",
                    ZipCode = "11900",
                    Country = "Malaysia",
                    Capacity = 40000,
                    PhoneNumber = "04-643 6688",
                    CreatedAt = seedDate
                }
            );

            // Seed Concerts - with Malaysian pricing (RM)
            modelBuilder.Entity<Concert>().HasData(
                new Concert
                {
                    ConcertId = 1,
                    Title = "Malaysian Music Festival 2025",
                    Description = "The biggest music festival in Malaysia featuring top local and international artists",
                    EventDate = new DateTime(2025, 3, 15),
                    EventTime = new DateTime(2025, 3, 15, 19, 0, 0),
                    Artist = "Various Artists",
                    Genre = "Rock",
                    BasePrice = 250.00m, // RM250
                    AvailableTickets = 80000,
                    TotalTickets = 87000,
                    VenueId = 1,
                    ImageUrl = "/assets/images/concerts/concert1.jpg",
                    CreatedAt = seedDate
                },
                new Concert
                {
                    ConcertId = 2,
                    Title = "Pop Extravaganza KL",
                    Description = "International pop stars live in Kuala Lumpur",
                    EventDate = new DateTime(2025, 4, 5),
                    EventTime = new DateTime(2025, 4, 5, 20, 0, 0),
                    Artist = "Top Pop Stars",
                    Genre = "Pop",
                    BasePrice = 350.00m, // RM350
                    AvailableTickets = 14500,
                    TotalTickets = 16000,
                    VenueId = 2,
                    ImageUrl = "/assets/images/concerts/concert2.jpg",
                    CreatedAt = seedDate
                },
                new Concert
                {
                    ConcertId = 3,
                    Title = "Jazz Night KL",
                    Description = "An intimate evening of smooth jazz featuring Malaysian and international artists",
                    EventDate = new DateTime(2025, 4, 20),
                    EventTime = new DateTime(2025, 4, 20, 21, 0, 0),
                    Artist = "Jazz Ensemble Malaysia",
                    Genre = "Jazz",
                    BasePrice = 180.00m, // RM180
                    AvailableTickets = 3200,
                    TotalTickets = 3500,
                    VenueId = 3,
                    ImageUrl = "/assets/images/concerts/concert3.jpg",
                    CreatedAt = seedDate
                },
                new Concert
                {
                    ConcertId = 4,
                    Title = "Rainforest World Music Festival",
                    Description = "Experience world music in the heart of Borneo",
                    EventDate = new DateTime(2025, 6, 14),
                    EventTime = new DateTime(2025, 6, 14, 18, 0, 0),
                    Artist = "World Music Artists",
                    Genre = "World Music",
                    BasePrice = 220.00m, // RM220
                    AvailableTickets = 4500,
                    TotalTickets = 5000,
                    VenueId = 5,
                    ImageUrl = "/assets/images/concerts/concert4.jpg",
                    CreatedAt = seedDate
                },
                new Concert
                {
                    ConcertId = 5,
                    Title = "Good Vibes Festival 2025",
                    Description = "Malaysia's premier indie music festival",
                    EventDate = new DateTime(2025, 7, 19),
                    EventTime = new DateTime(2025, 7, 19, 14, 0, 0),
                    Artist = "Indie Bands Malaysia",
                    Genre = "Alternative",
                    BasePrice = 199.00m, // RM199
                    AvailableTickets = 35000,
                    TotalTickets = 40000,
                    VenueId = 6,
                    ImageUrl = "/assets/images/concerts/concert5.jpg",
                    CreatedAt = seedDate
                },
                new Concert
                {
                    ConcertId = 6,
                    Title = "Electronic Dance Music Festival",
                    Description = "The ultimate EDM experience with world-class DJs",
                    EventDate = new DateTime(2025, 8, 9),
                    EventTime = new DateTime(2025, 8, 9, 22, 0, 0),
                    Artist = "Top EDM DJs",
                    Genre = "Electronic",
                    BasePrice = 420.00m, // RM420
                    AvailableTickets = 120000,
                    TotalTickets = 130000,
                    VenueId = 4,
                    ImageUrl = "/assets/images/concerts/concert6.jpg",
                    CreatedAt = seedDate
                }
            );
        }
    }
}
