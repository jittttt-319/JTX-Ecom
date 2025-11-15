# ?? JTX Concert API Documentation

## Base URL
- **Development**: `https://localhost:7096`
- **Production**: `https://your-domain.com`

---

## ?? Table of Contents
1. [Authentication Endpoints](#authentication-endpoints)
2. [Public Endpoints](#public-endpoints)
3. [Admin Endpoints](#admin-endpoints)
4. [Request/Response Examples](#examples)
5. [Error Codes](#error-codes)

---

## ?? Authentication Endpoints

### Register User
Create a new user account.

**Endpoint:** `POST /api/Auth/register`

**Request Body:**
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "password": "SecurePass123",
  "confirmPassword": "SecurePass123"
}
```

**Success Response:** `200 OK`
```json
{
  "message": "User registered successfully"
}
```

**Error Response:** `400 Bad Request`
```json
{
  "message": "User with this email already exists"
}
```

---

### Login
Authenticate user and receive JWT token.

**Endpoint:** `POST /api/Auth/login`

**Request Body:**
```json
{
  "email": "john.doe@example.com",
  "password": "SecurePass123"
}
```

**Success Response:** `200 OK`
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "email": "john.doe@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "roles": ["User"],
  "expiresAt": "2025-01-16T10:30:00Z"
}
```

**Error Response:** `401 Unauthorized`
```json
{
  "message": "Invalid email or password"
}
```

---

### Register Admin (Protected)
Create a new admin account (requires admin secret).

**Endpoint:** `POST /api/Auth/admin/register`

**Headers:**
```
Content-Type: application/json
adminSecret: JTX-Admin-Secret-2025
```

**Request Body:**
```json
{
  "firstName": "Admin",
  "lastName": "User",
  "email": "admin@example.com",
  "password": "AdminPass123",
  "confirmPassword": "AdminPass123"
}
```

**Success Response:** `200 OK`
```json
{
  "message": "Admin registered successfully"
}
```

---

## ?? Public Web Endpoints

### Home Page
**Endpoint:** `GET /`
- Shows featured concerts
- No authentication required

### Browse Concerts
**Endpoint:** `GET /Concerts`
**Query Parameters:**
- `search` (optional): Search by title or artist
- `genre` (optional): Filter by genre

**Example:**
```
GET /Concerts?search=rock&genre=Rock
```

### Concert Details
**Endpoint:** `GET /Concerts/Details/{id}`
**Example:**
```
GET /Concerts/Details/1
```

---

## ????? Admin Web Endpoints (Protected)

All admin endpoints require authentication and Admin role.

### Admin Dashboard
**Endpoint:** `GET /Admin`
- Requires: `[Authorize(Roles = "Admin")]`
- Shows statistics and quick actions

### Manage Concerts
**Endpoint:** `GET /Admin/Concerts`
- Lists all concerts
- Requires: Admin role

### Create Concert
**Endpoint:** `GET /Admin/CreateConcert`
- Shows create form
- Requires: Admin role

**Endpoint:** `POST /Admin/CreateConcert`
- Creates new concert
- Requires: Admin role
- Content-Type: `multipart/form-data` or `application/x-www-form-urlencoded`

**Form Fields:**
```
Title: string (required)
Artist: string (required)
Genre: string (required)
Description: string (optional)
EventDate: date (required)
EventTime: time (required)
VenueId: int (required)
BasePrice: decimal (required)
TotalTickets: int (required)
AvailableTickets: int (required)
ImageUrl: string (optional)
IsActive: boolean
```

### Edit Concert
**Endpoint:** `GET /Admin/EditConcert/{id}`
- Shows edit form
- Requires: Admin role

**Endpoint:** `POST /Admin/EditConcert/{id}`
- Updates concert
- Requires: Admin role

### Delete Concert
**Endpoint:** `GET /Admin/DeleteConcert/{id}`
- Shows confirmation page
- Requires: Admin role

**Endpoint:** `POST /Admin/DeleteConcert/{id}`
- Deletes concert
- Requires: Admin role

### View Venues
**Endpoint:** `GET /Admin/Venues`
- Lists all venues
- Requires: Admin role

### View Orders
**Endpoint:** `GET /Admin/Orders`
- Lists all orders
- Requires: Admin role

---

## ?? Examples

### Using cURL

#### Register User
```bash
curl -X POST https://localhost:7096/api/Auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "Test",
    "lastName": "User",
    "email": "test@example.com",
    "password": "Test123",
    "confirmPassword": "Test123"
  }'
```

#### Login
```bash
curl -X POST https://localhost:7096/api/Auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@jtxconcert.com",
    "password": "Admin@123"
  }'
```

### Using JavaScript (Fetch API)

#### Register User
```javascript
const register = async () => {
  const response = await fetch('https://localhost:7096/api/Auth/register', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      firstName: 'Test',
      lastName: 'User',
      email: 'test@example.com',
      password: 'Test123',
      confirmPassword: 'Test123'
    })
  });
  
  const data = await response.json();
  console.log(data);
};
```

#### Login and Store Token
```javascript
const login = async () => {
  const response = await fetch('https://localhost:7096/api/Auth/login', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      email: 'admin@jtxconcert.com',
      password: 'Admin@123'
    })
  });
  
  const data = await response.json();
  
  if (response.ok) {
    // Store token
    localStorage.setItem('token', data.token);
    console.log('Login successful!');
  }
};
```

#### Make Authenticated Request
```javascript
const getAdminData = async () => {
  const token = localStorage.getItem('token');
  
  const response = await fetch('https://localhost:7096/Admin/Concerts', {
    headers: {
      'Authorization': `Bearer ${token}`
    }
  });
  
  const data = await response.text(); // Returns HTML
  console.log(data);
};
```

### Using C# (HttpClient)

#### Register User
```csharp
using System.Net.Http;
using System.Text;
using System.Text.Json;

var client = new HttpClient();
var registerDto = new {
    firstName = "Test",
    lastName = "User",
    email = "test@example.com",
    password = "Test123",
    confirmPassword = "Test123"
};

var json = JsonSerializer.Serialize(registerDto);
var content = new StringContent(json, Encoding.UTF8, "application/json");

var response = await client.PostAsync(
    "https://localhost:7096/api/Auth/register", 
    content
);

var result = await response.Content.ReadAsStringAsync();
Console.WriteLine(result);
```

#### Login
```csharp
var loginDto = new {
    email = "admin@jtxconcert.com",
    password = "Admin@123"
};

var json = JsonSerializer.Serialize(loginDto);
var content = new StringContent(json, Encoding.UTF8, "application/json");

var response = await client.PostAsync(
    "https://localhost:7096/api/Auth/login", 
    content
);

var result = await response.Content.ReadAsStringAsync();
var authResponse = JsonSerializer.Deserialize<AuthResponse>(result);
Console.WriteLine($"Token: {authResponse.Token}");
```

---

## ?? Error Codes

### HTTP Status Codes

| Code | Status | Description |
|------|--------|-------------|
| 200 | OK | Request successful |
| 201 | Created | Resource created successfully |
| 400 | Bad Request | Invalid request data |
| 401 | Unauthorized | Authentication required or failed |
| 403 | Forbidden | Insufficient permissions |
| 404 | Not Found | Resource not found |
| 500 | Internal Server Error | Server error |

### Common Error Responses

#### Validation Error (400)
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Password": [
      "The Password field is required."
    ],
    "Email": [
      "The Email field is not a valid e-mail address."
    ]
  }
}
```

#### Unauthorized (401)
```json
{
  "message": "Invalid email or password"
}
```

#### Forbidden (403)
```json
{
  "message": "You do not have permission to access this resource"
}
```

---

## ?? Authentication

### Using JWT Token

Once you receive a token from the login endpoint, include it in subsequent requests:

**Authorization Header:**
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Cookie (Automatic for web):**
The JWT token is automatically stored in an HTTP-only cookie named `jwt` when using web endpoints.

### Token Information
- **Type:** JWT (JSON Web Token)
- **Algorithm:** HS256
- **Expiration:** 24 hours
- **Claims:**
  - `sub`: User ID
  - `email`: User email
  - `role`: User roles (Admin, User)
  - `jti`: Unique token ID
  - `iat`: Issued at timestamp
  - `exp`: Expiration timestamp

---

## ?? Data Models

### User Model
```json
{
  "id": "string (GUID)",
  "firstName": "string",
  "lastName": "string",
  "email": "string",
  "createdAt": "datetime",
  "lastLoginAt": "datetime"
}
```

### Concert Model
```json
{
  "concertId": "integer",
  "title": "string",
  "description": "string",
  "eventDate": "date",
  "eventTime": "time",
  "artist": "string",
  "genre": "string",
  "imageUrl": "string",
  "basePrice": "decimal",
  "availableTickets": "integer",
  "totalTickets": "integer",
  "isActive": "boolean",
  "venueId": "integer",
  "venue": {
    "venueId": "integer",
    "name": "string",
    "city": "string",
    "state": "string"
  }
}
```

### Order Model
```json
{
  "orderId": "integer",
  "orderNumber": "string",
  "customerName": "string",
  "customerEmail": "string",
  "customerPhone": "string",
  "totalAmount": "decimal",
  "paymentStatus": "string",
  "orderDate": "datetime",
  "quantity": "integer"
}
```

---

## ?? Testing with Postman

### Setup
1. Import this documentation into Postman
2. Create environment variables:
   - `base_url`: `https://localhost:7096`
   - `token`: (will be set after login)

### Test Sequence

1. **Register User**
   ```
   POST {{base_url}}/api/Auth/register
   ```

2. **Login**
   ```
   POST {{base_url}}/api/Auth/login
   ```
   Save the token from response

3. **Access Admin (with token)**
   ```
   GET {{base_url}}/Admin
   Authorization: Bearer {{token}}
   ```

---

## ?? Rate Limiting (Future)

Consider implementing rate limiting in production:
- **Authentication endpoints**: 5 requests per minute
- **API endpoints**: 100 requests per minute
- **Admin endpoints**: 1000 requests per minute

---

## ?? Notes

- All timestamps are in UTC
- All prices are in USD
- Email addresses must be unique
- Passwords must meet security requirements
- Tokens expire after 24 hours
- HTTPS is enforced in production

---

## ?? Support

For API issues:
1. Check response status code
2. Review error message
3. Verify authentication token
4. Check request format
5. Review server logs

---

**API Documentation Version: 1.0**
**Last Updated: 2025-01-15**
