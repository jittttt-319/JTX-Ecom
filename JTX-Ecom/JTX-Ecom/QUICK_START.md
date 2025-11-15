# ?? Quick Start Guide - JTX Concert Website

## Start the Website

### Method 1: Using Visual Studio
1. Open `JTX-Ecom.sln` in Visual Studio 2022
2. Press **F5** (Run with debugging) or **Ctrl+F5** (Run without debugging)
3. The browser will automatically open

### Method 2: Using Command Line
```bash
cd C:\Users\Jit\Downloads\GitHub\JTX-Ecom\JTX-Ecom\JTX-Ecom
dotnet run
```

### Method 3: Using VS Code
1. Open the `JTX-Ecom` folder in VS Code
2. Press **F5** or use the Run menu
3. Select ".NET Core Launch (web)"

## Access the Website

Once started, visit:
- **HTTP**: http://localhost:5037
- **HTTPS**: https://localhost:7096 (recommended)

## ????? Admin Access

### Login as Admin
1. Go to: https://localhost:7096/Account/Login
2. Use these credentials:
   - **Email**: `admin@jtxconcert.com`
   - **Password**: `Admin@123`
3. After login, click "Admin Dashboard" in the top navigation

### Admin Features
- **Dashboard**: View statistics (total concerts, orders, revenue)
- **Manage Concerts**: Create, edit, delete concerts
- **View Venues**: See all concert venues
- **View Orders**: Track customer orders

## ?? User Access

### Register a New User
1. Go to: https://localhost:7096/Account/Register
2. Fill in:
   - First Name
   - Last Name
   - Email
   - Password (min 6 chars, must have uppercase, lowercase, and numbers)
   - Confirm Password
3. Click "Create Account"

### Login as User
1. Go to: https://localhost:7096/Account/Login
2. Enter your email and password
3. Click "Login"

### Browse Concerts
1. Click "Concerts" in the navigation menu
2. Use search box to find concerts by title or artist
3. Filter by genre using the dropdown
4. Click "View Details" to see concert information

## ?? Quick Tasks

### Add a New Concert (Admin Only)
1. Login as admin
2. Go to Admin Dashboard
3. Click "Add New Concert" or go to "Manage Concerts" ? "Add New Concert"
4. Fill in all fields:
   - Title (e.g., "Summer Rock Festival 2025")
   - Artist (e.g., "Rock Legends")
   - Genre (select from dropdown)
   - Description
   - Event Date (pick a future date)
   - Event Time (e.g., 19:00)
   - Venue (select from dropdown)
   - Base Price (e.g., 99.00)
   - Total Tickets (e.g., 5000)
   - Available Tickets (should equal Total Tickets for new concert)
   - Image URL (e.g., /assets/images/concerts/concert7.jpg)
   - Active checkbox (check to make it visible)
5. Click "Create Concert"

### Edit a Concert (Admin Only)
1. Go to "Manage Concerts"
2. Find the concert you want to edit
3. Click the edit icon (pencil)
4. Update any fields
5. Click "Save Changes"

### Delete a Concert (Admin Only)
1. Go to "Manage Concerts"
2. Find the concert you want to delete
3. Click the delete icon (trash)
4. Confirm deletion by clicking "Yes, Delete Concert"

## ?? Test Accounts

### Admin Account (Pre-created)
- **Email**: admin@jtxconcert.com
- **Password**: Admin@123
- **Access**: Full admin panel access

### Test User Accounts (Create your own)
You can register multiple test users with different emails:
- user1@test.com
- user2@test.com
- etc.

Password must meet requirements:
- At least 6 characters
- Contains uppercase letter
- Contains lowercase letter
- Contains number

## ?? Features to Test

### Public Features (No Login Required)
- ? View home page with featured concerts
- ? Browse all concerts
- ? Search concerts by title or artist
- ? Filter concerts by genre
- ? View concert details
- ? See venue information
- ? Check ticket availability

### User Features (Login Required)
- ? Register new account
- ? Login/Logout
- ? View profile in dropdown menu

### Admin Features (Admin Login Required)
- ? Access admin dashboard
- ? View statistics (concerts, orders, revenue)
- ? Create new concerts
- ? Edit existing concerts
- ? Delete concerts
- ? View all venues
- ? View all orders
- ? Manage concert status (active/inactive)

## ??? Troubleshooting

### Application won't start
```bash
# Check if port is already in use
# Try cleaning and rebuilding
dotnet clean
dotnet build
dotnet run
```

### Database errors
```bash
# Update database to latest migration
dotnet ef database update
```

### Can't login as admin
The admin user is automatically created on first run. If it doesn't exist:
1. Stop the application
2. Delete the database: `dotnet ef database drop`
3. Recreate it: `dotnet ef database update`
4. Run the application again

### Forgot to create admin on first run
Delete and recreate the database as shown above.

## ?? Sample Concert Data

The database comes pre-seeded with 6 sample concerts:
1. **Rock Festival 2025** - Madison Square Garden, NYC
2. **Pop Extravaganza** - Staples Center, LA
3. **Jazz Night Live** - Blue Note, Chicago
4. **Electronic Dance Night** - Ultra Arena, Miami
5. **Country Music Fest** - Grand Ole Opry, Nashville
6. **Hip Hop Summit** - Barclays Center, Brooklyn

## ?? Admin Dashboard Overview

### Statistics Cards
- **Total Concerts**: Shows all concerts in database
- **Active Concerts**: Shows only visible concerts
- **Total Orders**: Count of all orders
- **Total Revenue**: Sum of completed payments

### Quick Actions
- Add New Concert
- Manage Concerts
- View Orders

## ?? Security Features

### JWT Token Authentication
- Tokens expire after 24 hours
- Stored in HTTP-only cookies
- Secure HTTPS communication

### Password Requirements
- Minimum 6 characters
- At least one uppercase letter
- At least one lowercase letter  
- At least one number

### Role-Based Access
- Admin role: Full access to admin panel
- User role: Access to public pages and booking

## ?? API Endpoints (For Testing)

### Using Postman or curl

**Register User (API)**
```bash
POST https://localhost:7096/api/Auth/register
Content-Type: application/json

{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john@example.com",
  "password": "Password123",
  "confirmPassword": "Password123"
}
```

**Login (API)**
```bash
POST https://localhost:7096/api/Auth/login
Content-Type: application/json

{
  "email": "admin@jtxconcert.com",
  "password": "Admin@123"
}
```

Response will include JWT token.

## ?? Mobile Testing

The website is responsive! Test on:
- Desktop browsers (Chrome, Firefox, Edge)
- Tablet view (use browser dev tools)
- Mobile view (use browser dev tools or actual device)

## ?? Next Steps

After getting familiar with the system:
1. Add your own concerts
2. Create test user accounts
3. Customize the design in `/wwwroot/css/site.css`
4. Add concert images to `/wwwroot/assets/images/concerts/`
5. Implement booking functionality (next phase)

## ?? Tips

- Keep the admin credentials safe
- Use descriptive concert titles
- Set realistic ticket numbers
- Make sure event dates are in the future
- Use high-quality images for concerts
- Test on different browsers
- Check responsive design on mobile

## ?? Need Help?

1. Check the main README.md for detailed documentation
2. Review the code comments in controllers
3. Check the console for error messages
4. Verify database connection in appsettings.json

---

**You're all set! Start exploring the admin panel and adding concerts! ????**
