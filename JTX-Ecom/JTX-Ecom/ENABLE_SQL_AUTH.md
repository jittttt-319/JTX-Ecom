# ?? Enable SQL Server Authentication for LocalDB

## Current Issue
Beekeeper Studio doesn't support Windows Authentication for SQL Server LocalDB connections.

## ? Recommended Solutions

### Solution 1: Use Azure Data Studio (Easiest)
**Download**: https://aka.ms/azuredatastudio

**Connection Settings:**
- Server: `(localdb)\mssqllocaldb`
- Authentication: Windows Authentication
- Database: JTXConcertDB
- Trust Server Certificate: ?

**Why Azure Data Studio?**
- ? Free and modern
- ? Full Windows Authentication support
- ? Cross-platform
- ? Built by Microsoft for SQL Server
- ? Excellent LocalDB support

---

### Solution 2: Use SQL Server Management Studio (SSMS)
**Download**: https://aka.ms/ssmsfullsetup

**Connection Settings:**
- Server name: `(localdb)\mssqllocaldb`
- Authentication: Windows Authentication
- Click Connect

**Why SSMS?**
- ? Industry standard tool
- ? Most powerful SQL Server features
- ? Perfect for LocalDB
- ?? Larger download (~600MB)

---

### Solution 3: Enable SQL Authentication (Advanced)

If you must use Beekeeper Studio, follow these steps to enable SQL authentication:

#### Step 1: Connect via Command Line
```powershell
# Start LocalDB
SqlLocalDB start MSSQLLocalDB

# Connect to LocalDB
sqlcmd -S "(localdb)\mssqllocaldb" -d JTXConcertDB
```

#### Step 2: Enable Mixed Mode Authentication
```sql
-- Enable SQL Server and Windows Authentication
USE master;
GO

EXEC xp_instance_regwrite 
    N'HKEY_LOCAL_MACHINE', 
    N'Software\Microsoft\MSSQLServer\MSSQLServer',
    N'LoginMode', 
    REG_DWORD, 
    2;
GO
```

#### Step 3: Create SQL Login
```sql
-- Create a new SQL Server login
USE master;
GO

CREATE LOGIN jtx_admin WITH PASSWORD = 'YourStrongPassword123!';
GO

-- Grant access to JTXConcertDB
USE JTXConcertDB;
GO

CREATE USER jtx_admin FOR LOGIN jtx_admin;
GO

ALTER ROLE db_owner ADD MEMBER jtx_admin;
GO
```

#### Step 4: Restart LocalDB
```powershell
SqlLocalDB stop MSSQLLocalDB
SqlLocalDB start MSSQLLocalDB
```

#### Step 5: Connect with Beekeeper Studio
- Connection Type: SQL Server
- Host: `(localdb)\mssqllocaldb`
- Authentication: Username / Password
- Username: `jtx_admin`
- Password: `YourStrongPassword123!`
- Database: JTXConcertDB
- Trust Server Certificate: ?

#### Step 6: Update Your Connection String (Optional)
If you want your app to use SQL authentication instead:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=JTXConcertDB;User Id=jtx_admin;Password=YourStrongPassword123!;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

?? **Security Note**: Don't commit passwords to Git! Use User Secrets for development:

```powershell
cd "C:\Users\Jit\Downloads\GitHub\JTX-Ecom\JTX-Ecom\JTX-Ecom"
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=(localdb)\\mssqllocaldb;Database=JTXConcertDB;User Id=jtx_admin;Password=YourStrongPassword123!;MultipleActiveResultSets=true;TrustServerCertificate=True"
```

---

## ?? Quick Comparison

| Tool | Windows Auth | SQL Auth | Free | Best For |
|------|-------------|----------|------|----------|
| **Azure Data Studio** | ? | ? | ? | LocalDB, Modern UI |
| **SSMS** | ? | ? | ? | Full Features, Professional |
| **Beekeeper Studio** | ? | ?* | ? | Multi-DB, Requires Setup |

*Requires enabling SQL authentication first

---

## ?? Recommended Quick Start

**For LocalDB development, use Azure Data Studio:**

1. Download: https://aka.ms/azuredatastudio
2. Open Azure Data Studio
3. Click "New Connection"
4. Server: `(localdb)\mssqllocaldb`
5. Authentication: Windows Authentication
6. Database: JTXConcertDB
7. Click Connect

**You'll immediately see your tables and data!**

---

## ?? Verify Your Database

Once connected, run this query to see your concert data:

```sql
SELECT 
    c.Title,
    c.Artist,
    c.Genre,
    c.EventDate,
    c.BasePrice,
    v.Name AS VenueName,
    v.City
FROM Concerts c
INNER JOIN Venues v ON c.VenueId = v.VenueId
ORDER BY c.EventDate;
```

---

## ?? Still Having Issues?

### LocalDB Not Running?
```powershell
SqlLocalDB info
SqlLocalDB start MSSQLLocalDB
```

### Can't Find sqlcmd?
Install SQL Server Command Line Tools:
https://docs.microsoft.com/sql/tools/sqlcmd-utility

### Need to Test Connection from App?
```powershell
cd "C:\Users\Jit\Downloads\GitHub\JTX-Ecom\JTX-Ecom\JTX-Ecom"
dotnet run
```

Then visit: http://localhost:5000

---

## ?? Learn More

- [Azure Data Studio Docs](https://docs.microsoft.com/sql/azure-data-studio/)
- [LocalDB Documentation](https://docs.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb)
- [SQL Server Authentication Modes](https://docs.microsoft.com/sql/relational-databases/security/choose-an-authentication-mode)
