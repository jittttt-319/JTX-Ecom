# ?? Beekeeper Studio - Quick Connection Guide

## Connection Configuration

Open Beekeeper Studio and use these settings:

### Option 1: Basic Connection
```
Connection Type: SQL Server
Host: (localdb)\mssqllocaldb
Database: JTXConcertDB
Authentication: Windows Authentication
```

### Option 2: Connection String (Recommended)
```
Server=(localdb)\mssqllocaldb;Database=JTXConcertDB;Trusted_Connection=True;TrustServerCertificate=True
```

### Option 3: Alternative Host Names (if above doesn't work)
- Try: `localhost`
- Try: `.\MSSQLSERVER`
- Try: `localhost\mssqllocaldb`

## ? What You Should See After Connecting

**Tables:**
1. ? Concerts (6 rows)
2. ? Venues (6 rows)
3. ? Tickets (0 rows - empty for now)
4. ? Orders (0 rows - empty for now)

## ?? Quick Test Query

After connecting, run this query to verify:

```sql
SELECT 
    c.Title AS Concert,
    v.Name AS Venue,
    c.EventDate,
    c.BasePrice,
    c.AvailableTickets
FROM Concerts c
INNER JOIN Venues v ON c.VenueId = v.VenueId
ORDER BY c.EventDate;
```

You should see all 6 concerts with their venue information!

## ?? Expected Results:

| Concert | Venue | EventDate | BasePrice | AvailableTickets |
|---------|-------|-----------|-----------|------------------|
| Rock Festival 2025 | Madison Square Garden | 2025-03-15 | $89.00 | 15,000 |
| Pop Extravaganza | Staples Center | 2025-04-05 | $125.00 | 18,000 |
| Jazz Night Live | Blue Note | 2025-04-20 | $65.00 | 450 |
| Electronic Dance Night | Ultra Arena | 2025-05-10 | $149.00 | 9,500 |
| Country Music Fest | Grand Ole Opry | 2025-06-02 | $79.00 | 4,200 |
| Hip Hop Summit | Barclays Center | 2025-06-21 | $95.00 | 17,500 |

---

## ?? Common Beekeeper Actions

### View All Data in a Table:
1. Click on table name (e.g., "Concerts")
2. Click "Content" tab
3. You'll see all rows

### Run Custom Query:
1. Click "Query" tab (or press Ctrl+T)
2. Type your SQL query
3. Press F5 or click "Run" button

### Export Data:
1. Right-click on table
2. Select "Export"
3. Choose format (CSV, JSON, etc.)

### Import Data:
1. Right-click on table
2. Select "Import"
3. Choose your file

---

## ?? Troubleshooting Beekeeper Connection

### Error: "Cannot connect to server"
**Solution:**
1. Make sure SQL Server LocalDB is running:
   ```bash
   SqlLocalDB info
   SqlLocalDB start MSSQLLocalDB
   ```

2. In Beekeeper, check "Trust Server Certificate"

### Error: "Login failed for user"
**Solution:**
- Use **Windows Authentication** (not SQL Server Authentication)
- Leave username/password fields empty

### Error: "Database does not exist"
**Solution:**
Run this to verify database exists:
```bash
sqlcmd -S "(localdb)\mssqllocaldb" -Q "SELECT name FROM sys.databases"
```

You should see `JTXConcertDB` in the list.

---

## ?? Beekeeper Tips

1. **Dark Mode:** Settings ? Appearance ? Dark Theme
2. **Auto-complete:** Start typing SQL and press Ctrl+Space
3. **Format SQL:** Right-click ? Format SQL (or Ctrl+Shift+F)
4. **Multiple Tabs:** Open multiple query tabs with Ctrl+T
5. **Save Queries:** Click "Save" to save frequently used queries

---

## ?? Useful Queries for Your Concert App

### Get Upcoming Concerts:
```sql
SELECT * FROM Concerts 
WHERE EventDate >= GETDATE() 
ORDER BY EventDate;
```

### Get Concerts by City:
```sql
SELECT 
    c.Title,
    c.EventDate,
    v.Name AS Venue,
    v.City
FROM Concerts c
INNER JOIN Venues v ON c.VenueId = v.VenueId
WHERE v.City = 'New York'
ORDER BY c.EventDate;
```

### Check Ticket Availability:
```sql
SELECT 
    Title,
    AvailableTickets,
    TotalTickets,
    (AvailableTickets * 100.0 / TotalTickets) AS AvailabilityPercent
FROM Concerts
WHERE IsActive = 1;
```

---

## ?? Ready to Use!

Your database is fully set up and ready to connect with Beekeeper Studio!

**Database Name:** `JTXConcertDB`  
**Tables:** 4 (Concerts, Venues, Tickets, Orders)  
**Sample Data:** 6 concerts and 6 venues pre-loaded
