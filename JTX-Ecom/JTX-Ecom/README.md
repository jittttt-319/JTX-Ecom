# JTX Concert Ticketing System - Complete Guide

## ?? Overview
A complete ASP.NET Core 9.0 Razor Pages concert ticketing e-commerce platform with JWT authentication for both user and admin sides.

## ? Features

### User Features
- Browse and search concerts by genre, artist, or title
- View detailed concert information
- User registration and authentication
- JWT token-based security
- Responsive design with modern UI

### Admin Features (Token-based Authentication)
- **Admin Dashboard** - Overview of key metrics
- **Concert Management** - Full CRUD operations (Create, Read, Update, Delete)
- **Venue Management** - View all venues
- **Order Management** - Track all bookings
- **Secure Access** - Role-based authorization with JWT

## ?? Getting Started

### Prerequisites
- .NET 9.0 SDK
- SQL Server (LocalDB or full SQL Server)
- Visual Studio 2022 or VS Code

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/jittttt-319/JTX-Ecom.git
   cd JTX-Ecom
   ```

2. **Restore packages**
   ```bash
   cd JTX-Ecom
   dotnet restore
   ```

3. **Update database connection** (if needed)
   Edit `appsettings.json` to configure your SQL Server connection

4. **Apply database migrations**
   ```bash
   dotnet ef database update
   ```

5. **Run the application**
   ```bash
   dotnet run
   ```

6. **Access the application**
   - Website: `https://localhost:7096` or `http://localhost:5037`

## ?? Default Admin Credentials

The system automatically creates an admin user on first run:

- **Email:** `admin@jtxconcert.com`
- **Password:** `Admin@123`

## ?? Authentication System

### JWT Configuration
JWT settings are configured in `appsettings.json`:
```json
{
  "JwtSettings": {
    "SecretKey": "Your-Secret-Key-Here",
    "Issuer": "JTXConcertApp",
    "Audience": "JTXConcertUsers",
    "ExpirationHours": 24
  }
}
```

### User Roles
- **Admin**: Full access to admin panel and concert management
- **User**: Can browse concerts and make bookings

### Authentication Flow
1. User registers or logs in via `/Account/Login`
2. JWT token is generated and stored in HTTP-only cookie
3. Token is validated on each request
4. Admin users are redirected to admin dashboard
5. Regular users access public concert pages

## ?? Project Structure

```
JTX-Ecom/
??? Controllers/
?   ??? HomeController.cs         # Home page
?   ??? ConcertsController.cs     # Public concert listing
?   ??? AccountController.cs      # User login/register (Web)
?   ??? AuthController.cs         # API authentication
?   ??? AdminController.cs        # Admin panel (Protected)
??? Models/
?   ??? Concert.cs
?   ??? Venue.cs
?   ??? Ticket.cs
?   ??? Order.cs
?   ??? User.cs                   # Identity user model
?   ??? DTOs/
?       ??? AuthDtos.cs           # Login/Register DTOs
?       ??? ConcertDto.cs         # Concert form DTO
??? Services/
?   ??? IJwtService.cs
?   ??? JwtService.cs             # JWT token generation
??? Data/
?   ??? ApplicationDbContext.cs   # EF Core DbContext
??? Views/
?   ??? Home/
?   ??? Concerts/
?   ??? Account/                  # Login/Register views
?   ?   ??? Login.cshtml
?   ?   ??? Register.cshtml
?   ??? Admin/                    # Admin panel views
?   ?   ??? Index.cshtml          # Dashboard
?   ?   ??? Concerts.cshtml       # Concert list
?   ?   ??? CreateConcert.cshtml
?   ?   ??? EditConcert.cshtml
?   ?   ??? DeleteConcert.cshtml
?   ?   ??? Venues.cshtml
?   ?   ??? Orders.cshtml
?   ??? Shared/
?       ??? _Layout.cshtml        # Main layout
?       ??? _AdminLayout.cshtml   # Admin layout
??? Migrations/
```

## ?? Admin Panel Features

### Dashboard (`/Admin/Index`)
- Total concerts count
- Active concerts
- Total orders
- Total revenue
- Quick action buttons

### Concert Management (`/Admin/Concerts`)
- View all concerts in a table
- See ticket availability with progress bars
- Quick edit and delete actions
- Status badges (Active/Inactive)

### Create Concert (`/Admin/CreateConcert`)
Form fields:
- Title, Artist, Genre
- Description
- Event Date & Time
- Venue selection
- Price
- Total & Available Tickets
- Image URL
- Active status

### Edit Concert (`/Admin/EditConcert/{id}`)
- Pre-filled form with existing concert data
- Update any concert information
- Save changes with validation

### Delete Concert (`/Admin/DeleteConcert/{id}`)
- Confirmation page showing concert details
- Safe deletion with warning

### Venue Management (`/Admin/Venues`)
- View all venues
- See venue capacity and concert count

### Order Management (`/Admin/Orders`)
- View all customer orders
- Track payment status
- See order details and history

## ?? Security Features

1. **JWT Authentication**
   - Secure token-based authentication
   - HTTP-only cookies for web
   - Token expiration (24 hours)

2. **Role-Based Authorization**
   - Admin role required for admin panel
   - `[Authorize(Roles = "Admin")]` attribute

3. **Password Requirements**
   - Minimum 6 characters
   - Requires uppercase
   - Requires lowercase
   - Requires numbers

4. **HTTPS Enforcement**
   - Secure communication
   - HTTPS redirection enabled

## ?? Database Schema

### Main Tables
- **AspNetUsers** - User accounts (Identity)
- **AspNetRoles** - User roles
- **AspNetUserRoles** - User-Role mapping
- **Concerts** - Concert events
- **Venues** - Concert venues
- **Tickets** - Individual tickets
- **Orders** - Customer orders

### Relationships
- Concert ? Venue (Many-to-One)
- Concert ? Tickets (One-to-Many)
- Order ? User (Many-to-One)
- Order ? Tickets (One-to-Many)

## ?? API Endpoints

### Authentication API
```
POST /api/Auth/register       - Register new user
POST /api/Auth/login          - Login and get JWT token
POST /api/Auth/admin/register - Register admin (requires secret)
```

### Web Routes
```
GET  /                        - Home page
GET  /Concerts                - Browse concerts
GET  /Concerts/Details/{id}   - Concert details

GET  /Account/Login           - Login page
POST /Account/Login           - Process login
GET  /Account/Register        - Register page
POST /Account/Register        - Process registration
POST /Account/Logout          - Logout

GET  /Admin                   - Admin dashboard (Protected)
GET  /Admin/Concerts          - Manage concerts (Protected)
GET  /Admin/CreateConcert     - Create concert form (Protected)
POST /Admin/CreateConcert     - Create concert (Protected)
GET  /Admin/EditConcert/{id}  - Edit concert form (Protected)
POST /Admin/EditConcert/{id}  - Update concert (Protected)
GET  /Admin/DeleteConcert/{id}- Delete confirmation (Protected)
POST /Admin/DeleteConcert/{id}- Delete concert (Protected)
GET  /Admin/Venues            - View venues (Protected)
GET  /Admin/Orders            - View orders (Protected)
```

## ?? Usage Guide

### For Users
1. Visit the website
2. Click "Register" to create an account
3. Browse concerts on the home page or concerts page
4. Use search and filters to find concerts
5. View concert details

### For Admins
1. Login with admin credentials
2. Click "Admin Dashboard" in navigation
3. **To add a concert:**
   - Click "Add New Concert"
   - Fill in all required fields
   - Click "Create Concert"
4. **To edit a concert:**
   - Go to "Manage Concerts"
   - Click the edit icon
   - Update fields and save
5. **To delete a concert:**
   - Go to "Manage Concerts"
   - Click the delete icon
   - Confirm deletion

## ??? Technologies Used

- **Framework:** ASP.NET Core 9.0
- **UI:** Razor Pages
- **Authentication:** ASP.NET Core Identity + JWT
- **ORM:** Entity Framework Core 9.0
- **Database:** SQL Server
- **Frontend:** Bootstrap 5, Bootstrap Icons, Font Awesome
- **Validation:** Data Annotations

## ?? NuGet Packages

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.10" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="9.0.10" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="9.0.0" />
<PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="9.0.0" />
```

## ?? UI Features

### User Interface
- Modern gradient design
- Responsive grid layout
- Card-based concert display
- Search and filter functionality
- Genre badges
- Ticket availability indicators

### Admin Interface
- Sidebar navigation
- Dashboard statistics cards
- Data tables with sorting
- Form validation
- Success/Error notifications
- Confirmation dialogs

## ?? Future Enhancements

- [ ] Shopping cart functionality
- [ ] Payment integration (Stripe/PayPal)
- [ ] Email notifications
- [ ] Ticket QR codes
- [ ] User profile management
- [ ] Order history for users
- [ ] Advanced reporting for admin
- [ ] File upload for concert images
- [ ] Multi-tier ticket pricing
- [ ] Seat selection

## ?? Troubleshooting

### Database Issues
```bash
# Reset database
dotnet ef database drop
dotnet ef database update
```

### Migration Issues
```bash
# Remove last migration
dotnet ef migrations remove

# Add new migration
dotnet ef migrations add MigrationName
```

### Build Issues
```bash
# Clean and rebuild
dotnet clean
dotnet build
```

## ?? License

This project is for educational purposes.

## ????? Author

JTX Development Team

## ?? Support

For issues or questions, please open an issue on GitHub.

---

**Happy Coding! ????**
