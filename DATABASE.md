# ??? Database Schema Documentation

**JTX Concert Ticketing System - Database Structure**

---

## ?? Table of Contents

- [Overview](#-overview)
- [Entity Relationship Diagram](#-entity-relationship-diagram)
- [Tables](#-tables)
  - [Identity Tables](#identity-tables)
  - [Core Tables](#core-tables)
  - [Transaction Tables](#transaction-tables)
- [Relationships](#-relationships)
- [Indexes](#-indexes)
- [Constraints](#-constraints)
- [Sample Queries](#-sample-queries)
- [Data Seeding](#-data-seeding)
- [Backup & Restore](#-backup--restore)

---

## ?? Overview

The JTX Concert Ticketing System uses **SQL Server** as its database engine with **Entity Framework Core 9.0** as the ORM. The database follows a **relational model** with proper normalization (3NF) and includes ASP.NET Core Identity tables for authentication.

### Database Statistics

- **Total Tables:** 12
- **Total Relationships:** 8
- **Identity Tables:** 6 (AspNet*)
- **Application Tables:** 6
- **Database Size:** ~50MB (with sample data)

---

## ?? Entity Relationship Diagram

```
???????????????????
?   AspNetUsers   ?
?  (Identity)     ?
???????????????????
         ?
         ? 1:N
         ?
    ?????????????????????????????????????????????????
    ?              ?                  ?             ?
    ?              ?                  ?             ?
?????????    ??????????         ??????????   ????????????
?Orders ?    ? Cart   ?         ?Profile ?   ?UserRoles ?
?????????    ??????????         ??????????   ????????????
    ?             ?
    ? 1:N         ? 1:N
    ?             ?
    ?             ?
??????????   ????????????
?Tickets ?   ?CartItems ?
??????????   ????????????
    ?             ?
    ?             ?
    ? N:1         ? N:1
    ?             ?
    ???????????????
           ?
           ?
      ??????????         ?????????
      ?Concert ??????????? Venue ?
      ?        ?   N:1   ?       ?
      ??????????         ?????????
```

---

## ?? Tables

### Identity Tables

ASP.NET Core Identity provides built-in user management tables:

#### 1. AspNetUsers
Stores user account information.

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| Id | nvarchar(450) | No | Primary key (GUID) |
| UserName | nvarchar(256) | Yes | Username (email) |
| NormalizedUserName | nvarchar(256) | Yes | Normalized username |
| Email | nvarchar(256) | Yes | User email |
| NormalizedEmail | nvarchar(256) | Yes | Normalized email |
| EmailConfirmed | bit | No | Email verified? |
| PasswordHash | nvarchar(MAX) | Yes | Hashed password |
| SecurityStamp | nvarchar(MAX) | Yes | Security token |
| ConcurrencyStamp | nvarchar(MAX) | Yes | Concurrency token |
| PhoneNumber | nvarchar(MAX) | Yes | Phone number |
| PhoneNumberConfirmed | bit | No | Phone verified? |
| TwoFactorEnabled | bit | No | 2FA enabled? |
| LockoutEnd | datetimeoffset(7) | Yes | Lockout end date |
| LockoutEnabled | bit | No | Can be locked out? |
| AccessFailedCount | int | No | Failed login count |
| FirstName | nvarchar(50) | No | User first name |
| LastName | nvarchar(50) | No | User last name |
| CreatedAt | datetime2(7) | No | Account created |

**Indexes:**
- IX_AspNetUsers_NormalizedEmail
- IX_AspNetUsers_NormalizedUserName (Unique)

---

#### 2. AspNetRoles
Stores user roles (Admin, User, etc.).

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| Id | nvarchar(450) | No | Primary key (GUID) |
| Name | nvarchar(256) | Yes | Role name |
| NormalizedName | nvarchar(256) | Yes | Normalized name |
| ConcurrencyStamp | nvarchar(MAX) | Yes | Concurrency token |

**Default Roles:**
- `Admin` - Full system access
- `User` - Standard user access

---

#### 3. AspNetUserRoles
Maps users to roles (Many-to-Many).

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| UserId | nvarchar(450) | No | FK to AspNetUsers |
| RoleId | nvarchar(450) | No | FK to AspNetRoles |

**Composite Primary Key:** (UserId, RoleId)

---

#### 4. AspNetUserClaims
Stores additional user claims.

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| Id | int | No | Primary key (Identity) |
| UserId | nvarchar(450) | No | FK to AspNetUsers |
| ClaimType | nvarchar(MAX) | Yes | Claim type |
| ClaimValue | nvarchar(MAX) | Yes | Claim value |

---

#### 5. AspNetUserLogins
External login providers (Google, Facebook, etc.).

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| LoginProvider | nvarchar(450) | No | Provider name |
| ProviderKey | nvarchar(450) | No | Provider key |
| ProviderDisplayName | nvarchar(MAX) | Yes | Display name |
| UserId | nvarchar(450) | No | FK to AspNetUsers |

**Composite Primary Key:** (LoginProvider, ProviderKey)

---

#### 6. AspNetUserTokens
Stores authentication tokens.

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| UserId | nvarchar(450) | No | FK to AspNetUsers |
| LoginProvider | nvarchar(450) | No | Provider name |
| Name | nvarchar(450) | No | Token name |
| Value | nvarchar(MAX) | Yes | Token value |

**Composite Primary Key:** (UserId, LoginProvider, Name)

---

### Core Tables

#### 7. Concerts
Stores concert event information.

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| ConcertId | int | No | Primary key (Identity) |
| Title | nvarchar(200) | No | Concert title |
| Description | nvarchar(MAX) | Yes | Event description |
| Artist | nvarchar(200) | No | Artist/band name |
| Genre | nvarchar(50) | No | Music genre |
| EventDate | datetime2(7) | No | Event date |
| EventTime | TimeSpan | No | Event time |
| VenueId | int | No | FK to Venues |
| BasePrice | decimal(18,2) | No | Starting price (RM) |
| TotalTickets | int | No | Total available |
| AvailableTickets | int | No | Currently available |
| ImageUrl | nvarchar(500) | Yes | Image path/URL |
| IsActive | bit | No | Published? |
| CreatedAt | datetime2(7) | No | Created timestamp |
| UpdatedAt | datetime2(7) | Yes | Last updated |

**Indexes:**
- IX_Concerts_VenueId
- IX_Concerts_EventDate
- IX_Concerts_Genre
- IX_Concerts_IsActive

**Constraints:**
- `CK_Concerts_AvailableTickets` - AvailableTickets <= TotalTickets
- `CK_Concerts_BasePrice` - BasePrice >= 0

**Sample Data:**
```sql
INSERT INTO Concerts (Title, Artist, Genre, EventDate, EventTime, VenueId, BasePrice, TotalTickets, AvailableTickets, IsActive, CreatedAt)
VALUES 
('Rock Night Live', 'The Rockers', 'Rock', '2024-12-31', '20:00', 1, 250.00, 500, 325, 1, GETDATE()),
('Jazz Evening', 'Jazz Masters', 'Jazz', '2024-12-15', '19:30', 2, 180.00, 300, 300, 1, GETDATE());
```

---

#### 8. Venues
Stores concert venue information.

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| VenueId | int | No | Primary key (Identity) |
| Name | nvarchar(200) | No | Venue name |
| Address | nvarchar(500) | No | Street address |
| City | nvarchar(100) | No | City name |
| State | nvarchar(100) | No | State/Province |
| ZipCode | nvarchar(20) | No | Postal code |
| Capacity | int | No | Max capacity |
| PhoneNumber | nvarchar(20) | Yes | Contact phone |
| Email | nvarchar(100) | Yes | Contact email |
| CreatedAt | datetime2(7) | No | Created timestamp |

**Indexes:**
- IX_Venues_City
- IX_Venues_State

**Constraints:**
- `CK_Venues_Capacity` - Capacity > 0

**Sample Data:**
```sql
INSERT INTO Venues (Name, Address, City, State, ZipCode, Capacity, PhoneNumber, CreatedAt)
VALUES 
('Kuala Lumpur Convention Centre', 'Jalan Pinang', 'Kuala Lumpur', 'Wilayah Persekutuan', '50088', 5000, '03-2333-2888', GETDATE()),
('Penang International Convention Centre', 'Jalan Tun Dr Awang', 'Penang', 'Pulau Pinang', '11900', 3000, '04-261-8888', GETDATE());
```

---

### Transaction Tables

#### 9. Orders
Stores customer orders.

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| OrderId | int | No | Primary key (Identity) |
| OrderNumber | nvarchar(50) | No | Unique order ref |
| UserId | nvarchar(450) | No | FK to AspNetUsers |
| OrderDate | datetime2(7) | No | Order timestamp |
| TotalAmount | decimal(18,2) | No | Total price (RM) |
| Quantity | int | No | Total tickets |
| PaymentStatus | nvarchar(50) | No | Payment state |
| PaymentMethod | nvarchar(50) | Yes | Payment type |
| TransactionId | nvarchar(100) | Yes | Payment ref |
| BillingName | nvarchar(200) | No | Billing name |
| BillingEmail | nvarchar(100) | No | Billing email |
| BillingPhone | nvarchar(20) | Yes | Contact phone |

**Indexes:**
- IX_Orders_UserId
- IX_Orders_OrderNumber (Unique)
- IX_Orders_OrderDate
- IX_Orders_PaymentStatus

**Payment Status Values:**
- `Pending` - Awaiting payment
- `Completed` - Payment successful
- `Failed` - Payment failed
- `Refunded` - Order refunded

**Sample Data:**
```sql
INSERT INTO Orders (OrderNumber, UserId, OrderDate, TotalAmount, Quantity, PaymentStatus, BillingName, BillingEmail)
VALUES 
('ORD-2024-000001', 'user-guid-here', GETDATE(), 500.00, 2, 'Completed', 'John Doe', 'john@example.com');
```

---

#### 10. Tickets
Individual tickets for orders.

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| TicketId | int | No | Primary key (Identity) |
| OrderId | int | No | FK to Orders |
| ConcertId | int | No | FK to Concerts |
| TicketNumber | nvarchar(50) | No | Unique ticket ref |
| TicketType | nvarchar(50) | No | Ticket tier |
| Price | decimal(18,2) | No | Ticket price (RM) |
| SeatNumber | nvarchar(20) | Yes | Seat assignment |
| QRCode | nvarchar(500) | Yes | QR code data |
| PurchaseDate | datetime2(7) | Yes | Purchase timestamp |
| IsUsed | bit | No | Ticket scanned? |
| UsedDate | datetime2(7) | Yes | Scan timestamp |

**Indexes:**
- IX_Tickets_OrderId
- IX_Tickets_ConcertId
- IX_Tickets_TicketNumber (Unique)

**Ticket Types:**
- `General` - Standard admission (1x BasePrice)
- `VIP` - VIP access (2x BasePrice)
- `VVIP` - Premium VIP (3.5x BasePrice)

**Constraints:**
- `CK_Tickets_Price` - Price >= 0

**Sample Data:**
```sql
INSERT INTO Tickets (OrderId, ConcertId, TicketNumber, TicketType, Price, PurchaseDate, IsUsed)
VALUES 
(1, 1, 'TKT-2024-000001', 'General', 250.00, GETDATE(), 0),
(1, 1, 'TKT-2024-000002', 'General', 250.00, GETDATE(), 0);
```

---

#### 11. CartItems
Shopping cart items (temporary).

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| CartItemId | int | No | Primary key (Identity) |
| CartId | int | No | FK to Cart |
| ConcertId | int | No | FK to Concerts |
| TicketType | nvarchar(50) | No | Ticket tier |
| Quantity | int | No | Number of tickets |
| PricePerTicket | decimal(18,2) | No | Unit price (RM) |
| Subtotal | decimal(18,2) | No | Line total (RM) |
| AddedAt | datetime2(7) | No | Added timestamp |

**Indexes:**
- IX_CartItems_CartId
- IX_CartItems_ConcertId

**Constraints:**
- `CK_CartItems_Quantity` - Quantity > 0
- `CK_CartItems_PricePerTicket` - PricePerTicket >= 0

---

#### 12. Cart
Shopping cart (one per user).

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| CartId | int | No | Primary key (Identity) |
| UserId | nvarchar(450) | No | FK to AspNetUsers (Unique) |
| CreatedAt | datetime2(7) | No | Created timestamp |
| UpdatedAt | datetime2(7) | Yes | Last updated |

**Indexes:**
- IX_Cart_UserId (Unique)

---

#### 13. UserProfiles
Extended user profile information.

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| ProfileId | int | No | Primary key (Identity) |
| UserId | nvarchar(450) | No | FK to AspNetUsers |
| PhoneNumber | nvarchar(20) | Yes | Phone number |
| Address | nvarchar(200) | Yes | Street address |
| City | nvarchar(100) | Yes | City name |
| State | nvarchar(100) | Yes | State/Province |
| PostalCode | nvarchar(10) | Yes | Postal code |
| Country | nvarchar(50) | No | Country (default: Malaysia) |
| DateOfBirth | datetime2(7) | Yes | Birth date |
| ICNumber | nvarchar(20) | Yes | IC number (Malaysian) |
| ProfilePicture | nvarchar(500) | Yes | Profile image URL |
| ReceiveNewsletter | bit | No | Newsletter opt-in? |
| CreatedAt | datetime2(7) | No | Created timestamp |
| UpdatedAt | datetime2(7) | Yes | Last updated |

**Indexes:**
- IX_UserProfiles_UserId (Unique)

---

## ?? Relationships

### One-to-Many (1:N)

1. **AspNetUsers ? Orders**
   - One user can have many orders
   - `Orders.UserId` ? `AspNetUsers.Id`
   - ON DELETE: Cascade

2. **AspNetUsers ? Cart**
   - One user has one cart
   - `Cart.UserId` ? `AspNetUsers.Id` (Unique)
   - ON DELETE: Cascade

3. **AspNetUsers ? UserProfile**
   - One user has one profile
   - `UserProfiles.UserId` ? `AspNetUsers.Id`
   - ON DELETE: Cascade

4. **Venues ? Concerts**
   - One venue hosts many concerts
   - `Concerts.VenueId` ? `Venues.VenueId`
   - ON DELETE: Restrict

5. **Concerts ? Tickets**
   - One concert has many tickets
   - `Tickets.ConcertId` ? `Concerts.ConcertId`
   - ON DELETE: Restrict

6. **Concerts ? CartItems**
   - One concert can be in many carts
   - `CartItems.ConcertId` ? `Concerts.ConcertId`
   - ON DELETE: Cascade

7. **Orders ? Tickets**
   - One order contains many tickets
   - `Tickets.OrderId` ? `Orders.OrderId`
   - ON DELETE: Cascade

8. **Cart ? CartItems**
   - One cart contains many items
   - `CartItems.CartId` ? `Cart.CartId`
   - ON DELETE: Cascade

### Many-to-Many (N:M)

1. **AspNetUsers ? AspNetRoles**
   - Through: `AspNetUserRoles`
   - A user can have multiple roles
   - A role can be assigned to multiple users

---

## ?? Indexes

### Clustered Indexes
All tables have clustered indexes on their primary keys.

### Non-Clustered Indexes

**Performance-Critical Indexes:**

```sql
-- Concert searches
CREATE NONCLUSTERED INDEX IX_Concerts_Genre_IsActive_EventDate
ON Concerts (Genre, IsActive, EventDate);

-- User lookups
CREATE NONCLUSTERED INDEX IX_AspNetUsers_Email
ON AspNetUsers (NormalizedEmail)
INCLUDE (FirstName, LastName);

-- Order queries
CREATE NONCLUSTERED INDEX IX_Orders_UserId_OrderDate
ON Orders (UserId, OrderDate DESC);

-- Ticket lookups
CREATE NONCLUSTERED INDEX IX_Tickets_ConcertId_IsUsed
ON Tickets (ConcertId, IsUsed);
```

---

## ?? Constraints

### Primary Keys
All tables have `int IDENTITY` or `nvarchar(450)` primary keys.

### Foreign Keys
- All FK constraints with appropriate CASCADE/RESTRICT rules
- Ensures referential integrity

### Check Constraints

```sql
-- Ensure tickets don't exceed capacity
ALTER TABLE Concerts
ADD CONSTRAINT CK_Concerts_AvailableTickets
CHECK (AvailableTickets >= 0 AND AvailableTickets <= TotalTickets);

-- Ensure positive prices
ALTER TABLE Concerts
ADD CONSTRAINT CK_Concerts_BasePrice
CHECK (BasePrice >= 0);

-- Ensure venue capacity
ALTER TABLE Venues
ADD CONSTRAINT CK_Venues_Capacity
CHECK (Capacity > 0);

-- Ensure positive quantities
ALTER TABLE CartItems
ADD CONSTRAINT CK_CartItems_Quantity
CHECK (Quantity > 0);
```

### Unique Constraints

```sql
-- Unique order numbers
CREATE UNIQUE INDEX IX_Orders_OrderNumber
ON Orders (OrderNumber);

-- Unique ticket numbers
CREATE UNIQUE INDEX IX_Tickets_TicketNumber
ON Tickets (TicketNumber);

-- One cart per user
CREATE UNIQUE INDEX IX_Cart_UserId
ON Cart (UserId);

-- Unique usernames
CREATE UNIQUE INDEX IX_AspNetUsers_NormalizedUserName
ON AspNetUsers (NormalizedUserName)
WHERE NormalizedUserName IS NOT NULL;
```

---

## ?? Sample Queries

### User Queries

```sql
-- Get user with profile
SELECT u.*, p.*
FROM AspNetUsers u
LEFT JOIN UserProfiles p ON u.Id = p.UserId
WHERE u.Email = 'john@example.com';

-- Get user's order history
SELECT o.*, COUNT(t.TicketId) as TicketCount
FROM Orders o
LEFT JOIN Tickets t ON o.OrderId = t.OrderId
WHERE o.UserId = 'user-guid'
GROUP BY o.OrderId, o.OrderNumber, o.OrderDate, o.TotalAmount, o.PaymentStatus
ORDER BY o.OrderDate DESC;
```

### Concert Queries

```sql
-- Get active concerts with venue info
SELECT c.*, v.Name as VenueName, v.City
FROM Concerts c
INNER JOIN Venues v ON c.VenueId = v.VenueId
WHERE c.IsActive = 1 AND c.EventDate > GETDATE()
ORDER BY c.EventDate;

-- Search concerts by genre and date
SELECT c.*, v.Name as VenueName
FROM Concerts c
INNER JOIN Venues v ON c.VenueId = v.VenueId
WHERE c.Genre = 'Rock'
  AND c.IsActive = 1
  AND c.EventDate BETWEEN '2024-12-01' AND '2024-12-31'
ORDER BY c.EventDate;

-- Get concert availability
SELECT 
    ConcertId,
    Title,
    TotalTickets,
    AvailableTickets,
    (CAST(AvailableTickets AS FLOAT) / TotalTickets * 100) as AvailabilityPercent
FROM Concerts
WHERE IsActive = 1;
```

### Order & Ticket Queries

```sql
-- Get order details with tickets
SELECT 
    o.OrderNumber,
    o.OrderDate,
    o.TotalAmount,
    t.TicketNumber,
    t.TicketType,
    c.Title as ConcertTitle,
    c.EventDate
FROM Orders o
INNER JOIN Tickets t ON o.OrderId = t.OrderId
INNER JOIN Concerts c ON t.ConcertId = c.ConcertId
WHERE o.OrderId = 1;

-- Get user's upcoming tickets
SELECT 
    t.*,
    c.Title,
    c.Artist,
    c.EventDate,
    c.EventTime,
    v.Name as VenueName
FROM Tickets t
INNER JOIN Concerts c ON t.ConcertId = c.ConcertId
INNER JOIN Venues v ON c.VenueId = v.VenueId
INNER JOIN Orders o ON t.OrderId = o.OrderId
WHERE o.UserId = 'user-guid'
  AND c.EventDate > GETDATE()
  AND t.IsUsed = 0
ORDER BY c.EventDate;
```

### Shopping Cart Queries

```sql
-- Get user's cart
SELECT 
    ci.*,
    c.Title,
    c.Artist,
    c.EventDate,
    c.ImageUrl
FROM Cart ct
INNER JOIN CartItems ci ON ct.CartId = ci.CartId
INNER JOIN Concerts c ON ci.ConcertId = c.ConcertId
WHERE ct.UserId = 'user-guid';

-- Calculate cart total
SELECT 
    ct.CartId,
    COUNT(ci.CartItemId) as ItemCount,
    SUM(ci.Quantity) as TotalTickets,
    SUM(ci.Subtotal) as CartTotal
FROM Cart ct
LEFT JOIN CartItems ci ON ct.CartId = ci.CartId
WHERE ct.UserId = 'user-guid'
GROUP BY ct.CartId;
```

### Admin Queries

```sql
-- Get concert sales summary
SELECT 
    c.Title,
    c.Artist,
    c.TotalTickets,
    c.AvailableTickets,
    (c.TotalTickets - c.AvailableTickets) as TicketsSold,
    COUNT(DISTINCT o.OrderId) as TotalOrders,
    SUM(t.Price) as TotalRevenue
FROM Concerts c
LEFT JOIN Tickets t ON c.ConcertId = t.ConcertId
LEFT JOIN Orders o ON t.OrderId = o.OrderId
WHERE o.PaymentStatus = 'Completed'
GROUP BY c.ConcertId, c.Title, c.Artist, c.TotalTickets, c.AvailableTickets
ORDER BY TotalRevenue DESC;

-- Get venue utilization
SELECT 
    v.Name,
    v.City,
    v.Capacity,
    COUNT(c.ConcertId) as TotalConcerts,
    SUM(c.TotalTickets - c.AvailableTickets) as TotalTicketsSold
FROM Venues v
LEFT JOIN Concerts c ON v.VenueId = c.VenueId
GROUP BY v.VenueId, v.Name, v.City, v.Capacity
ORDER BY TotalConcerts DESC;
```

---

## ?? Data Seeding

### Admin User Seeding

```csharp
// Program.cs
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    
    // Create Admin role
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }
    
    // Create Admin user
    var adminEmail = "admin@jtxconcert.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    
    if (adminUser == null)
    {
        adminUser = new User
        {
            UserName = adminEmail,
            Email = adminEmail,
            FirstName = "Admin",
            LastName = "User",
            EmailConfirmed = true
        };
        
        await userManager.CreateAsync(adminUser, "Admin@123");
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}
```

### Sample Data Seeding

```csharp
// Create sample venues
var venues = new List<Venue>
{
    new Venue
    {
        Name = "Kuala Lumpur Convention Centre",
        Address = "Jalan Pinang",
        City = "Kuala Lumpur",
        State = "Wilayah Persekutuan",
        ZipCode = "50088",
        Capacity = 5000,
        PhoneNumber = "03-2333-2888"
    },
    // ... more venues
};

context.Venues.AddRange(venues);
await context.SaveChangesAsync();

// Create sample concerts
var concerts = new List<Concert>
{
    new Concert
    {
        Title = "Rock Night Live",
        Artist = "The Rockers",
        Genre = "Rock",
        Description = "An electrifying rock concert...",
        EventDate = DateTime.Now.AddMonths(2),
        EventTime = new TimeSpan(20, 0, 0),
        VenueId = 1,
        BasePrice = 250.00m,
        TotalTickets = 500,
        AvailableTickets = 500,
        IsActive = true,
        ImageUrl = "/assets/images/concerts/rock-night.jpg"
    },
    // ... more concerts
};

context.Concerts.AddRange(concerts);
await context.SaveChangesAsync();
```

---

## ?? Backup & Restore

### Backup Database

```sql
-- Full backup
BACKUP DATABASE JTXConcertDB
TO DISK = 'C:\Backups\JTXConcertDB_Full.bak'
WITH FORMAT, INIT, NAME = 'Full Database Backup';

-- Differential backup
BACKUP DATABASE JTXConcertDB
TO DISK = 'C:\Backups\JTXConcertDB_Diff.bak'
WITH DIFFERENTIAL, INIT, NAME = 'Differential Database Backup';
```

### Restore Database

```sql
-- Restore from backup
RESTORE DATABASE JTXConcertDB
FROM DISK = 'C:\Backups\JTXConcertDB_Full.bak'
WITH REPLACE;
```

### Entity Framework Migrations

```bash
# Create migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update

# Rollback to specific migration
dotnet ef database update PreviousMigrationName

# Generate SQL script
dotnet ef migrations script > migration.sql
```

---

## ?? Performance Tips

1. **Use Indexes Wisely**
   - Index frequently queried columns
   - Avoid over-indexing (slows writes)

2. **Optimize Queries**
   - Use `Include()` for eager loading
   - Avoid N+1 queries
   - Use pagination for large datasets

3. **Database Maintenance**
   - Regular index rebuilding
   - Update statistics
   - Monitor query performance

4. **Connection Pooling**
   - Enabled by default in EF Core
   - Adjust max pool size if needed

---

## ?? Monitoring

### Key Metrics to Monitor

- Query execution time
- Index fragmentation
- Database size growth
- Connection pool usage
- Deadlocks and blocking

### Useful Queries

```sql
-- Find slow queries
SELECT TOP 10
    qs.execution_count,
    qs.total_worker_time / qs.execution_count AS avg_cpu_time,
    SUBSTRING(qt.text, (qs.statement_start_offset/2)+1,
        ((CASE qs.statement_end_offset
            WHEN -1 THEN DATALENGTH(qt.text)
            ELSE qs.statement_end_offset
        END - qs.statement_start_offset)/2) + 1) AS query_text
FROM sys.dm_exec_query_stats qs
CROSS APPLY sys.dm_exec_sql_text(qs.sql_handle) qt
ORDER BY avg_cpu_time DESC;

-- Check index fragmentation
SELECT 
    OBJECT_NAME(ips.object_id) AS TableName,
    i.name AS IndexName,
    ips.avg_fragmentation_in_percent
FROM sys.dm_db_index_physical_stats(DB_ID(), NULL, NULL, NULL, 'LIMITED') ips
INNER JOIN sys.indexes i ON ips.object_id = i.object_id AND ips.index_id = i.index_id
WHERE ips.avg_fragmentation_in_percent > 10
ORDER BY ips.avg_fragmentation_in_percent DESC;
```

---

## ?? Additional Resources

- [Entity Framework Core Documentation](https://docs.microsoft.com/ef/core/)
- [SQL Server Performance Tuning](https://docs.microsoft.com/sql/relational-databases/performance/)
- [ASP.NET Core Identity](https://docs.microsoft.com/aspnet/core/security/authentication/identity)

---

<div align="center">

**?? Database Schema v1.0**

Last Updated: December 2024

</div>
