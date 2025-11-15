# ?? JTX Concert Ticketing System

<div align="center">

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-9.0-512BD4?style=for-the-badge&logo=.net)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-9.0-512BD4?style=for-the-badge)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoftsqlserver)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5-7952B3?style=for-the-badge&logo=bootstrap)
![License](https://img.shields.io/badge/License-Educational-green?style=for-the-badge)

**A modern, full-featured concert ticketing e-commerce platform built with ASP.NET Core 9.0**

[Features](#-features) • [Installation](#-installation) • [Documentation](#-documentation) • [Demo](#-demo-credentials) • [Screenshots](#-screenshots)

</div>

---

## ?? Table of Contents

- [Overview](#-overview)
- [Features](#-features)
- [Tech Stack](#-tech-stack)
- [Installation](#-installation)
- [Configuration](#-configuration)
- [Usage](#-usage)
- [Project Structure](#-project-structure)
- [API Documentation](#-api-documentation)
- [Database Schema](#-database-schema)
- [Security](#-security)
- [Screenshots](#-screenshots)
- [Troubleshooting](#-troubleshooting)
- [Contributing](#-contributing)
- [License](#-license)

---

## ?? Overview

JTX Concert Ticketing System is a complete, production-ready e-commerce platform designed specifically for concert ticket sales. Built with ASP.NET Core 9.0, it features a modern dark-themed UI, JWT authentication, role-based authorization, and comprehensive admin management tools.

### ?? Key Highlights

- ?? **Modern UI/UX** - Glassmorphism design with purple gradient theme
- ?? **Secure Authentication** - JWT token-based with HTTP-only cookies
- ??? **Role-Based Access** - Separate admin and user interfaces
- ?? **Fully Responsive** - Optimized for mobile, tablet, and desktop
- ?? **Complete E-Commerce** - Cart, checkout, orders, and ticket management
- ? **High Performance** - Optimized queries and caching strategies
- ??? **Image Management** - Upload preview and URL-based image handling

---

## ? Features

### ?? User Features

- ? **Browse Concerts** - View all available concerts with filters
- ?? **Search & Filter** - Search by title, artist, or filter by genre
- ?? **Concert Details** - Detailed event information with venue details
- ?? **Shopping Cart** - Add/remove tickets with real-time total calculation
- ?? **Secure Checkout** - Complete payment flow with order confirmation
- ??? **My Tickets** - View upcoming and past tickets with QR codes
- ?? **Order History** - Track all purchases and payment status
- ?? **User Profile** - Manage personal information and preferences
- ?? **Newsletter** - Subscribe for updates and exclusive offers

### ?? Admin Features

- ?? **Dashboard** - Overview of key metrics and statistics
- ?? **Concert Management** - Full CRUD operations for concerts
  - Create new concerts with image upload preview
  - Edit existing concert information
  - Delete concerts with confirmation
  - Toggle active/inactive status
- ??? **Venue Management** - View all venues with capacity info
- ?? **Order Management** - Track and manage customer orders
- ?? **User Management** - View registered users and roles
- ?? **Analytics** - Sales reports and performance metrics
- ??? **Media Management** - Upload and manage concert images

### ?? UI/UX Features

- ?? **Dark Theme** - Professional dark mode with purple accents
- ? **Glassmorphism Effects** - Modern frosted glass card designs
- ?? **Smooth Animations** - Hover effects, transitions, and scroll animations
- ?? **Mobile-First Design** - Responsive across all devices
- ?? **Custom Components** - Reusable UI component library
- ?? **Toast Notifications** - User-friendly success/error messages
- ?? **Keyboard Navigation** - Accessibility-friendly interface

---

## ??? Tech Stack

### Backend
- **Framework:** ASP.NET Core 9.0 (MVC)
- **Language:** C# 13.0
- **ORM:** Entity Framework Core 9.0
- **Database:** SQL Server
- **Authentication:** ASP.NET Core Identity + JWT Bearer
- **API:** RESTful API endpoints

### Frontend
- **UI Framework:** Bootstrap 5.3
- **Icons:** Bootstrap Icons + Font Awesome 6
- **Fonts:** Google Fonts (Inter)
- **JavaScript:** Vanilla JS + jQuery
- **CSS:** Custom CSS with CSS Variables
- **Animations:** AOS (Animate On Scroll)

### Development Tools
- **IDE:** Visual Studio 2022 / VS Code
- **Version Control:** Git
- **Package Manager:** NuGet
- **Testing:** xUnit (future)

---

## ?? Installation

### Prerequisites

Before you begin, ensure you have the following installed:

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) (LocalDB or full SQL Server)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/) (optional)

### Step-by-Step Installation

1. **Clone the Repository**
   ```bash
   git clone https://github.com/jittttt-319/JTX-Ecom.git
   cd JTX-Ecom
   ```

2. **Navigate to Project Directory**
   ```bash
   cd JTX-Ecom
   ```

3. **Restore NuGet Packages**
   ```bash
   dotnet restore
   ```

4. **Update Database Connection String** (Optional)
   
   Edit `appsettings.json` if you need to change the connection string:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=JTXConcertDB;Trusted_Connection=true;MultipleActiveResultSets=true"
     }
   }
   ```

5. **Apply Database Migrations**
   ```bash
   dotnet ef database update
   ```

6. **Run the Application**
   ```bash
   dotnet run
   ```

7. **Access the Application**
   
   Open your browser and navigate to:
   - **HTTPS:** `https://localhost:7096`
   - **HTTP:** `http://localhost:5037`

---

## ?? Configuration

### JWT Settings

Configure JWT authentication in `appsettings.json`:

```json
{
  "JwtSettings": {
    "SecretKey": "YourSuperSecretKeyHere_MinimumLength32Characters!",
    "Issuer": "JTXConcertApp",
    "Audience": "JTXConcertUsers",
    "ExpirationHours": 24
  }
}
```

### Email Settings (Future)

```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "SenderEmail": "noreply@jtxconcerts.com",
    "SenderName": "JTX Concerts"
  }
}
```

### Logging

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

---

## ?? Demo Credentials

### Admin Account
- **Email:** `admin@jtxconcert.com`
- **Password:** `Admin@123`
- **Access:** Full admin panel with all permissions

### Test User Account
Create your own account or use these test credentials after seeding:
- **Email:** `user@example.com`
- **Password:** `User@123`

---

## ?? Usage

### For End Users

1. **Register/Login**
   - Click "Register" to create a new account
   - Or "Login" if you already have an account

2. **Browse Concerts**
   - View all concerts on the home page
   - Use search to find specific concerts
   - Filter by genre (Rock, Pop, Jazz, etc.)

3. **Book Tickets**
   - Click on a concert to view details
   - Select ticket type (General, VIP, VVIP)
   - Choose quantity
   - Add to cart

4. **Checkout**
   - Review cart items
   - Proceed to checkout
   - Fill in payment details
   - Complete order

5. **View Tickets**
   - Go to "My Tickets" to see all purchased tickets
   - View ticket details with QR code
   - Check upcoming and past events

### For Administrators

1. **Login as Admin**
   - Use admin credentials
   - Automatically redirected to admin dashboard

2. **Manage Concerts**
   - **Create:** Click "Add New Concert" and fill in details
   - **Edit:** Click edit icon on concert list
   - **Delete:** Click delete icon and confirm
   - **Upload Images:** Use file upload or enter URL

3. **View Analytics**
   - Dashboard shows key metrics
   - Total concerts, active events
   - Revenue and order statistics

4. **Manage Orders**
   - View all customer orders
   - Check payment status
   - Process refunds (future)

---

## ?? Project Structure

```
JTX-Ecom/
??? Controllers/
?   ??? AccountController.cs      # User authentication
?   ??? AdminController.cs        # Admin panel management
?   ??? AuthController.cs         # API authentication
?   ??? CartController.cs         # Shopping cart
?   ??? ConcertsController.cs     # Public concert pages
?   ??? HomeController.cs         # Home page
?   ??? OrdersController.cs       # Order management
?   ??? ProfileController.cs      # User profile
?
??? Models/
?   ??? Concert.cs                # Concert entity
?   ??? Venue.cs                  # Venue entity
?   ??? Ticket.cs                 # Ticket entity
?   ??? Order.cs                  # Order entity
?   ??? Cart.cs                   # Cart entity
?   ??? User.cs                   # User entity (Identity)
?   ??? UserProfile.cs            # Extended user profile
?   ??? DTOs/
?       ??? AuthDtos.cs           # Auth DTOs
?       ??? ConcertDto.cs         # Concert form DTO
?       ??? CartDtos.cs           # Cart DTOs
?       ??? CheckoutDtos.cs       # Checkout DTOs
?       ??? ProfileDtos.cs        # Profile DTOs
?
??? Views/
?   ??? Home/
?   ?   ??? Index.cshtml          # Home page
?   ??? Concerts/
?   ?   ??? Index.cshtml          # Concert listing
?   ?   ??? Details.cshtml        # Concert details
?   ??? Account/
?   ?   ??? Login.cshtml          # Login page
?   ?   ??? Register.cshtml       # Registration page
?   ??? Admin/
?   ?   ??? Index.cshtml          # Admin dashboard
?   ?   ??? Concerts.cshtml       # Concert management
?   ?   ??? CreateConcert.cshtml  # Create concert
?   ?   ??? EditConcert.cshtml    # Edit concert
?   ?   ??? DeleteConcert.cshtml  # Delete confirmation
?   ?   ??? Venues.cshtml         # Venue listing
?   ?   ??? Orders.cshtml         # Order management
?   ??? Cart/
?   ?   ??? Index.cshtml          # Shopping cart
?   ?   ??? Checkout.cshtml       # Checkout page
?   ??? Orders/
?   ?   ??? Index.cshtml          # Order history
?   ?   ??? Details.cshtml        # Order details
?   ?   ??? MyTickets.cshtml      # User tickets
?   ?   ??? TicketDetails.cshtml  # Ticket details
?   ?   ??? Confirmation.cshtml   # Order confirmation
?   ??? Profile/
?   ?   ??? Index.cshtml          # User profile
?   ?   ??? ChangePassword.cshtml # Change password
?   ??? Shared/
?       ??? _Layout.cshtml        # Main layout
?       ??? _AdminLayout.cshtml   # Admin layout
?
??? Services/
?   ??? IJwtService.cs            # JWT interface
?   ??? JwtService.cs             # JWT implementation
?
??? Data/
?   ??? ApplicationDbContext.cs   # EF Core DbContext
?
??? wwwroot/
?   ??? css/
?   ?   ??? site.css              # Custom styles (1200+ lines)
?   ??? js/
?   ?   ??? site.js               # Custom JavaScript
?   ??? lib/                      # Client libraries
?   ??? assets/
?       ??? images/
?           ??? X-logo.png        # Application logo
?           ??? concerts/         # Concert images
?           ??? banners/          # Banner images
?
??? Migrations/                   # EF Core migrations
??? Properties/
?   ??? launchSettings.json       # Launch configuration
??? appsettings.json              # App configuration
??? Program.cs                    # Application entry point
??? JTX-Ecom.csproj              # Project file
```

---

## ?? API Documentation

### Authentication Endpoints

#### Register User
```http
POST /api/Auth/register
Content-Type: application/json

{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john@example.com",
  "password": "Password@123",
  "confirmPassword": "Password@123"
}
```

#### Login
```http
POST /api/Auth/login
Content-Type: application/json

{
  "email": "john@example.com",
  "password": "Password@123"
}

Response:
{
  "token": "eyJhbGci...",
  "email": "john@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "roles": ["User"],
  "expiresAt": "2024-12-25T10:00:00Z"
}
```

### Web Routes

| Route | Method | Description | Auth Required |
|-------|--------|-------------|---------------|
| `/` | GET | Home page | No |
| `/Concerts` | GET | Concert listing | No |
| `/Concerts/Details/{id}` | GET | Concert details | No |
| `/Account/Login` | GET/POST | Login page | No |
| `/Account/Register` | GET/POST | Registration | No |
| `/Account/Logout` | POST | Logout | Yes |
| `/Cart` | GET | Shopping cart | Yes |
| `/Cart/AddToCart` | POST | Add to cart | Yes |
| `/Cart/Checkout` | GET/POST | Checkout | Yes |
| `/Orders` | GET | Order history | Yes |
| `/Orders/Details/{id}` | GET | Order details | Yes |
| `/Orders/MyTickets` | GET | User tickets | Yes |
| `/Profile` | GET/POST | User profile | Yes |
| `/Admin` | GET | Admin dashboard | Admin |
| `/Admin/Concerts` | GET | Manage concerts | Admin |
| `/Admin/CreateConcert` | GET/POST | Create concert | Admin |
| `/Admin/EditConcert/{id}` | GET/POST | Edit concert | Admin |
| `/Admin/DeleteConcert/{id}` | GET/POST | Delete concert | Admin |

---

## ??? Database Schema

See [DATABASE.md](DATABASE.md) for complete database documentation including:
- Entity Relationship Diagram (ERD)
- Table structures and columns
- Relationships and foreign keys
- Indexes and constraints
- Sample data and queries

### Quick Overview

**Main Tables:**
- `AspNetUsers` - User accounts
- `Concerts` - Concert events
- `Venues` - Concert venues
- `Tickets` - Individual tickets
- `Orders` - Customer orders
- `CartItems` - Shopping cart
- `UserProfiles` - Extended user info

---

## ?? Security

### Authentication
- **JWT Bearer Tokens** - Secure API authentication
- **HTTP-Only Cookies** - Token storage for web
- **Token Expiration** - 24-hour validity
- **Refresh Tokens** - Future implementation

### Authorization
- **Role-Based Access Control** - Admin vs User roles
- **`[Authorize]` Attributes** - Controller-level protection
- **Claims-Based Identity** - User claims for permissions

### Password Requirements
- Minimum 6 characters
- At least one uppercase letter
- At least one lowercase letter
- At least one number
- Special characters recommended

### Data Protection
- **SQL Injection Prevention** - Parameterized queries
- **XSS Protection** - Input sanitization
- **CSRF Tokens** - Anti-forgery validation
- **HTTPS Enforcement** - SSL/TLS encryption

---

## ?? Screenshots

### User Interface

**Home Page**
- Modern hero section with floating cards
- Featured concerts grid
- Genre filter pills
- Newsletter subscription

**Concert Listing**
- Grid/List view toggle
- Advanced search and filters
- Availability indicators
- Sort by date/price/popularity

**Concert Details**
- Large hero image
- Event information cards
- Ticket type selection
- Real-time availability

**Shopping Cart**
- Item summary with images
- Quantity adjustment
- Order total calculation
- Secure checkout button

### Admin Interface

**Dashboard**
- Statistics cards (concerts, orders, revenue)
- Quick action buttons
- Recent activity feed
- Charts and graphs

**Concert Management**
- Data table with search
- Edit/Delete actions
- Status badges
- Image thumbnails

**Create/Edit Concert**
- Organized form sections
- Image upload with preview
- Real-time validation
- Auto-save draft (future)

---

## ?? Troubleshooting

### Database Issues

**Problem:** Database connection fails
```bash
# Solution: Check connection string
dotnet ef database drop
dotnet ef database update
```

**Problem:** Migration errors
```bash
# Remove last migration
dotnet ef migrations remove

# Create new migration
dotnet ef migrations add MigrationName

# Apply migrations
dotnet ef database update
```

### Build Issues

**Problem:** Build fails with missing packages
```bash
# Clean and restore
dotnet clean
dotnet restore
dotnet build
```

**Problem:** CSS/JS not loading
```bash
# Clear browser cache: Ctrl + F5
# Or rebuild with:
dotnet clean
dotnet build
```

### Runtime Issues

**Problem:** 401 Unauthorized errors
- Check JWT token expiration
- Verify user roles in database
- Re-login to get new token

**Problem:** Images not displaying
- Verify image path: `/wwwroot/assets/images/concerts/`
- Check file permissions
- Use correct URL format: `/assets/images/concerts/image.jpg`

### Common Errors

| Error | Cause | Solution |
|-------|-------|----------|
| `InvalidOperationException: Sections not rendered` | Missing @RenderSection | Add @RenderSection("Name", required: false) to layout |
| `NullReferenceException` in views | Model is null | Check controller returns correct model |
| `404 Not Found` for routes | Route not configured | Check Program.cs routing configuration |
| Database login failed | Wrong connection string | Update appsettings.json |

---

## ?? Future Enhancements

### Phase 1 (Q1 2025)
- [ ] Payment Gateway Integration (Stripe/PayPal)
- [ ] Email Notifications (SendGrid)
- [ ] QR Code Generation for tickets
- [ ] PDF Ticket Download
- [ ] Social Media Login (Google, Facebook)

### Phase 2 (Q2 2025)
- [ ] Advanced Reporting & Analytics
- [ ] Seat Selection System
- [ ] Multi-tier Pricing
- [ ] Discount Codes & Promotions
- [ ] Wishlist Functionality

### Phase 3 (Q3 2025)
- [ ] Mobile App (React Native/Flutter)
- [ ] Real-time Notifications (SignalR)
- [ ] Review & Rating System
- [ ] Artist Portal
- [ ] Multi-language Support

### Phase 4 (Q4 2025)
- [ ] AI-Powered Recommendations
- [ ] Chatbot Support
- [ ] Advanced Search (Elasticsearch)
- [ ] Performance Optimization
- [ ] Microservices Architecture

---

## ?? Contributing

Contributions are welcome! Please follow these steps:

1. **Fork the Repository**
2. **Create a Feature Branch**
   ```bash
   git checkout -b feature/AmazingFeature
   ```
3. **Commit Your Changes**
   ```bash
   git commit -m 'Add some AmazingFeature'
   ```
4. **Push to the Branch**
   ```bash
   git push origin feature/AmazingFeature
   ```
5. **Open a Pull Request**

### Code Style Guidelines
- Follow C# coding conventions
- Use meaningful variable/method names
- Add XML documentation comments
- Write unit tests for new features
- Update README if needed

---

## ?? License

This project is created for **educational purposes**. Feel free to use it for learning and portfolio projects.

---

## ????? Author

**JTX Development Team**

- GitHub: [@jittttt-319](https://github.com/jittttt-319)
- Project Link: [https://github.com/jittttt-319/JTX-Ecom](https://github.com/jittttt-319/JTX-Ecom)

---

## ?? Support

For issues, questions, or feature requests:

1. **GitHub Issues:** [Open an issue](https://github.com/jittttt-319/JTX-Ecom/issues)
2. **Discussions:** Use GitHub Discussions for general questions
3. **Email:** support@jtxconcerts.com (fictional)

---

## ?? Acknowledgments

- ASP.NET Core Team for the excellent framework
- Bootstrap Team for the UI framework
- Bootstrap Icons for the icon library
- Microsoft for comprehensive documentation
- The open-source community

---

## ?? Additional Documentation

- [DATABASE.md](DATABASE.md) - Complete database schema and documentation
- [UI_DESIGN_SYSTEM.md](UI_DESIGN_SYSTEM.md) - UI components and styling guide
- [ADMIN_UI_IMPROVEMENTS.md](ADMIN_UI_IMPROVEMENTS.md) - Admin panel improvements
- [LOGO_IMPLEMENTATION.md](LOGO_IMPLEMENTATION.md) - Logo integration guide

---

<div align="center">

**? Star this repository if you found it helpful!**

Made with ?? for music lovers and concert-goers

**Happy Coding! ????**

</div>
