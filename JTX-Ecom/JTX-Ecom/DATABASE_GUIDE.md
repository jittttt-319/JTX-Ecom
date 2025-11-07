# ?? Database Setup Guide for JTX Concert Ticketing System

## ? Database Successfully Created!

Your SQL Server database `JTXConcertDB` has been created with the following structure:

### ?? Database Tables

#### 1. **Venues** Table
Stores concert venue information
- VenueId (Primary Key)
- Name, Address, City, State, ZipCode, Country
- Capacity, PhoneNumber, ImageUrl
- IsActive, CreatedAt

**Seeded Data:** 6 venues including Madison Square Garden, Staples Center, Blue Note, Ultra Arena, Grand Ole Opry, and Barclays Center

#### 2. **Concerts** Table
Stores concert event information
- ConcertId (Primary Key)
- Title, Description, Artist, Genre
- EventDate, EventTime
- BasePrice, AvailableTickets, TotalTickets
- VenueId (Foreign Key to Venues)
- ImageUrl, IsActive, CreatedAt, UpdatedAt

**Seeded Data:** 6 concerts across different genres (Rock, Pop, Jazz, Electronic, Country, Hip Hop)

#### 3. **Tickets** Table
Stores individual ticket information
- TicketId (Primary Key)
- TicketNumber (Unique), TicketType
- Price, Status (Available, Sold, Reserved, Cancelled)
- SeatNumber, Section
- ConcertId (Foreign Key to Concerts)
- OrderId (Foreign Key to Orders, nullable)
- CreatedAt, SoldAt

#### 4. **Orders** Table
Stores customer order information
- OrderId (Primary Key)
- OrderNumber (Unique)
- CustomerName, CustomerEmail, CustomerPhone
- TotalAmount, Quantity
- PaymentStatus (Pending, Completed, Failed, Refunded)
- PaymentMethod, TransactionId
- OrderDate, PaymentDate

---

## ?? How to Connect with Beekeeper Studio

### Connection Details:

```
Connection Type: SQL Server
Host: (localdb)\mssqllocaldb
Port: 1433 (default)
Database: JTXConcertDB
Authentication: Windows Authentication
Trust Server Certificate: Yes
```

### Steps to Connect:

1. **Open Beekeeper Studio**

2. **Create New Connection**
   - Click "New Connection" or the "+" button
   - Select **"SQL Server"** as the database type

3. **Configure Connection Settings:**
   ```
   Server: (localdb)\mssqllocaldb
   Database: JTXConcertDB
   Authentication: Windows Authentication (or Integrated Security)
   ```
   
   **OR use this connection string:**
   ```
   Server=(localdb)\mssqllocaldb;Database=JTXConcertDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True
   ```

4. **Test Connection**
   - Click "Test" to verify the connection
   - If successful, click "Connect"

5. **Explore Your Data**
   - You should see 4 tables: Concerts, Venues, Tickets, Orders
   - The Venues and Concerts tables already have sample data!

---

## ?? Sample SQL Queries to Try

### View All Concerts with Venue Information:
```sql
SELECT 
    c.Title,
    c.Artist,
    c.Genre,
    c.EventDate,
    c.BasePrice,
    c.AvailableTickets,
    v.Name AS VenueName,
    v.City,
    v.State
FROM Concerts c
INNER JOIN Venues v ON c.VenueId = v.VenueId
WHERE c.IsActive = 1
ORDER BY c.EventDate;
```

### View All Venues:
```sql
SELECT * FROM Venues
ORDER BY City;
```

### Check Concert Availability:
```sql
SELECT 
    Title,
    Artist,
    EventDate,
    BasePrice,
    AvailableTickets,
    TotalTickets,
    CAST((AvailableTickets * 100.0 / TotalTickets) AS DECIMAL(5,2)) AS AvailabilityPercent
FROM Concerts
WHERE IsActive = 1
ORDER BY EventDate;
```

### Find Concerts by Genre:
```sql
SELECT 
    c.Title,
    c.Artist,
    c.Genre,
    c.EventDate,
    v.Name AS VenueName,
    v.City
FROM Concerts c
INNER JOIN Venues v ON c.VenueId = v.VenueId
WHERE c.Genre = 'Rock' -- Change to: Pop, Jazz, Electronic, Country, Hip Hop
ORDER BY c.EventDate;
```

---

## ?? Database Connection String

Your application uses this connection string (configured in `appsettings.json`):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=JTXConcertDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

---

## ??? Entity Framework Core Commands

### View All Migrations:
```bash
dotnet ef migrations list
```

### Add New Migration (after model changes):
```bash
cd "C:\Users\Jit\Downloads\GitHub\JTX-Ecom\JTX-Ecom\JTX-Ecom"
dotnet ef migrations add YourMigrationName
```

### Update Database:
```bash
cd "C:\Users\Jit\Downloads\GitHub\JTX-Ecom\JTX-Ecom\JTX-Ecom"
dotnet ef database update
```

### Remove Last Migration:
```bash
dotnet ef migrations remove
```

### Drop Database (BE CAREFUL!):
```bash
dotnet ef database drop
```

---

## ?? How the Database Works in Your Application

### 1. **DbContext (ApplicationDbContext.cs)**
   - Acts as a bridge between your C# code and SQL Server
   - Manages database connections
   - Tracks entity changes
   - Handles queries and updates

### 2. **Models (Entity Classes)**
   - `Concert.cs` - Represents a concert event
   - `Venue.cs` - Represents a venue/location
   - `Ticket.cs` - Represents individual tickets
   - `Order.cs` - Represents customer orders

### 3. **Relationships**
   - **One-to-Many:** One Venue can have many Concerts
   - **One-to-Many:** One Concert can have many Tickets
   - **One-to-Many:** One Order can have many Tickets

### 4. **Data Access in Controllers**

Example of how to use the database in your controllers:

```csharp
public class ConcertsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ConcertsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Get all concerts
    public async Task<IActionResult> Index()
    {
        var concerts = await _context.Concerts
            .Include(c => c.Venue)
            .Where(c => c.IsActive)
            .OrderBy(c => c.EventDate)
            .ToListAsync();
        
        return View(concerts);
    }

    // Get concert by ID
    public async Task<IActionResult> Details(int id)
    {
        var concert = await _context.Concerts
            .Include(c => c.Venue)
            .FirstOrDefaultAsync(c => c.ConcertId == id);
        
        if (concert == null)
            return NotFound();
        
        return View(concert);
    }
}
```

---

## ?? Next Steps

1. ? **Database Created** - Your SQL Server database is ready!
2. ?? **Connect with Beekeeper** - Use the connection details above
3. ?? **Explore Data** - Run the sample queries to see your seeded data
4. ??? **Build Controllers** - Create controllers to interact with the database
5. ?? **Update Views** - Display real data from the database on your homepage

---

## ?? Troubleshooting

### Can't Connect to LocalDB?
```bash
# Check if SQL Server LocalDB is installed
SqlLocalDB info

# Start LocalDB instance
SqlLocalDB start MSSQLLocalDB
```

### Connection Refused in Beekeeper?
- Make sure LocalDB is running
- Try using `localhost\mssqllocaldb` instead of `(localdb)\mssqllocaldb`
- Enable TCP/IP in SQL Server Configuration Manager

### Need to Reset Database?
```bash
cd "C:\Users\Jit\Downloads\GitHub\JTX-Ecom\JTX-Ecom\JTX-Ecom"
dotnet ef database drop --force
dotnet ef database update
```

---

## ?? Learn More

- [Entity Framework Core Documentation](https://docs.microsoft.com/ef/core/)
- [SQL Server LocalDB](https://docs.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb)
- [LINQ Queries](https://docs.microsoft.com/dotnet/csharp/linq/)
