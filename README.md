# Supplier Management System - Complete Solution

This project implements the complete coding exercise with all three exercises:

- **Exercise 1**: Multi-layered web application with CRUD operations
- **Exercise 2**: REST APIs with JWT authentication
- **Exercise 3**: Separate web application with client-side JavaScript

## 🏗️ Architecture

The solution follows a clean architecture pattern with the following projects:

- **SupplierManagement.API**: Web API layer with controllers and authentication
- **SupplierManagement.Core**: Business logic and domain entities
- **SupplierManagement.Infrastructure**: Data access layer with Entity Framework
- **SupplierManagement.Web**: Web application for testing APIs with JavaScript

## 🚀 Features

### Exercise 1: Web Application Layers ✅

- Clean architecture with proper separation of concerns
- Supplier and SupplierRate entities
- In-memory database with seeded test data
- CRUD operations implemented

### Exercise 2: APIs with Authentication ✅

1. **GET /api/suppliers/all-with-rates**
   - Returns all suppliers with their associated rates
   - Requires JWT authentication

2. **GET /api/suppliers/overlapping-rates**
   - Returns suppliers with overlapping rate periods
   - Optional query parameter: `supplierId` (int) - to check specific supplier
   - Requires JWT authentication

3. **POST /api/auth/login**
   - Authenticates users and returns JWT token
   - No authentication required

### Exercise 3: Web Page with Client Scripting ✅

- **Separate web project** (SupplierManagement.Web)
- **Event-triggered API calls** via buttons
- **Synchronous call** for getting all suppliers and rates
- **Asynchronous call** for getting overlapping suppliers and rates
- **User-friendly interface** with Bootstrap styling
- **Real-time results display** with formatted data

### Authentication

- JWT (JSON Web Token) based authentication
- Test credentials available:
  - Username: `admin`, Password: `password123`
  - Username: `user`, Password: `user123`
  - Username: `demo`, Password: `demo123`

## 🛠️ Running the Application

### Prerequisites

- .NET 9.0 SDK
- Any IDE supporting .NET (Visual Studio, VS Code, Rider)

### Quick Start

1. **Navigate to the solution directory:**

   ```powershell
   cd "c:\Test\OneAdvanced\Supplier"
   ```

2. **Build the solution:**

   ```powershell
   dotnet build
   ```

3. **Run the API (Terminal 1):**

   ```powershell
   cd SupplierManagement.API
   dotnet run
   ```

4. **Run the Web Application (Terminal 2):**

   ```powershell
   cd SupplierManagement.Web
   dotnet run
   ```

5. **Access the applications:**
   - **API**: `https://localhost:7001` (Swagger UI: `https://localhost:7001/swagger`)
   - **Web App**: `https://localhost:5001`

### Using the Web Application

1. Open the web application at `https://localhost:5001`
2. Login using the provided credentials (default: admin/password123)
3. Use the buttons to test the APIs:
   - **"Load All Suppliers & Rates"** - Makes a synchronous call
   - **"Load Overlapping Rates"** - Makes an asynchronous call
4. Optionally filter by Supplier ID for overlapping rates
5. Results are displayed in a user-friendly format below

## 📝 Complete API Reference

### Authentication Endpoints

#### POST /api/auth/login

```json
{
  "username": "admin",
  "password": "password123"
}
```

### Exercise 2 APIs (Main Requirements)

#### GET /api/suppliers/all-with-rates

- **Purpose**: Returns all suppliers with their associated rates
- **Authentication**: Required (JWT Bearer token)
- **Response**: Array of suppliers with embedded rates

#### GET /api/suppliers/overlapping-rates?supplierId={id}

- **Purpose**: Returns suppliers with overlapping rate periods
- **Authentication**: Required (JWT Bearer token)
- **Query Parameters**:
  - `supplierId` (optional): Filter by specific supplier ID
- **Response**: Array of suppliers with overlapping rates and reasons

### Exercise 1 CRUD APIs

#### Supplier Management

- `GET /api/suppliers` - Get all suppliers
- `GET /api/suppliers/{id}` - Get supplier by ID
- `POST /api/suppliers` - Create new supplier
- `PUT /api/suppliers/{id}` - Update supplier
- `DELETE /api/suppliers/{id}` - Delete supplier

#### Rate Management

- `GET /api/suppliers/{supplierId}/rates` - Get rates for supplier
- `GET /api/suppliers/rates/{rateId}` - Get rate by ID
- `POST /api/suppliers/rates` - Create new rate
- `PUT /api/suppliers/rates/{rateId}` - Update rate
- `DELETE /api/suppliers/rates/{rateId}` - Delete rate

## 🔍 Business Logic Implementation

### Overlap Detection Algorithm

The system identifies overlapping rates using the following logic:

1. **Sort rates by start date** for each supplier
2. **Compare consecutive rates** to detect overlaps
3. **Consider null end dates** as indefinite (continuing forever)
4. **Flag overlaps** when:
   - Next rate starts before current rate ends
   - Next rate starts on the day after current rate ends (contiguous)

### Test Data Validation

Based on the provided test data, the system correctly identifies:

- **Supplier 3 (Premium Ltd)**: Has overlapping rates
  - Rate 1: 2016-12-01 to 2017-01-01
  - Rate 2: 2017-01-02 to null (ongoing)
  - *Reason*: Contiguous dates with ongoing rate

- **Suppliers 1 & 2**: No overlaps detected (correct gaps between rates)

## 🏛️ Architecture Details

### Clean Architecture Layers

```
┌─────────────────────────────────┐
│       Presentation Layer        │
├─────────────────────────────────┤
│ • Web Application (JavaScript)  │
│ • Web API (Controllers)         │
└─────────────────────────────────┘
                │
                ▼
┌─────────────────────────────────┐
│        Business Layer           │
├─────────────────────────────────┤
│ • Services (Logic)              │
│ • DTOs (Data Transfer)          │
└─────────────────────────────────┘
                │
                ▼
┌─────────────────────────────────┐
│       Data Access Layer         │
├─────────────────────────────────┤
│ • Repositories (Data Access)    │
│ • Entities (Domain)             │
└─────────────────────────────────┘
                │
                ▼
┌─────────────────────────────────┐
│     Infrastructure Layer        │
├─────────────────────────────────┤
│ • DbContext (EF Core)           │
│ • Database (In-Memory)          │
└─────────────────────────────────┘
```

### Key Features Implemented

✅ **Exercise 1 Requirements**:

- Multi-layered architecture with proper separation
- Complete CRUD operations for Suppliers and SupplierRates
- Entity Framework with seeded test data
- Clean domain entities following business rules

✅ **Exercise 2 Requirements**:

- JWT-based authentication with test users
- RESTful APIs with proper HTTP methods
- Comprehensive error handling and logging
- Swagger documentation for all endpoints

✅ **Exercise 3 Requirements**:

- Separate web application project
- Event-triggered JavaScript API calls
- Synchronous call for all suppliers (using XMLHttpRequest)
- Asynchronous call for overlapping rates (using fetch)
- User-friendly interface with real-time results

## 🚀 Scalability Considerations

For enterprise-scale deployment, see `ARCHITECTURE.md` for detailed analysis including:

- Microservices decomposition strategy
- Database scaling with read replicas
- Caching layers (Redis, CDN)
- Event-driven architecture
- High availability infrastructure
- Performance optimizations

## 🧪 Testing the Application

### Quick Test Scenarios

1. **Authentication Test**:

   ```bash
   curl -X POST "https://localhost:7001/api/auth/login" \
     -H "Content-Type: application/json" \
     -d '{"username": "admin", "password": "password123"}'
   ```

2. **Get All Suppliers with Rates**:

   ```bash
   curl -X GET "https://localhost:7001/api/suppliers/all-with-rates" \
     -H "Authorization: Bearer YOUR_JWT_TOKEN"
   ```

3. **Check Overlapping Rates**:

   ```bash
   curl -X GET "https://localhost:7001/api/suppliers/overlapping-rates" \
     -H "Authorization: Bearer YOUR_JWT_TOKEN"
   ```

4. **Create New Supplier**:

   ```bash
   curl -X POST "https://localhost:7001/api/suppliers" \
     -H "Authorization: Bearer YOUR_JWT_TOKEN" \
     -H "Content-Type: application/json" \
     -d '{"name": "New Supplier", "address": "123 Test St", "createdByUser": "test.user"}'
   ```

### Web Application Testing

1. Navigate to `https://localhost:5001`
2. Use provided test credentials
3. Test both synchronous and asynchronous API calls
4. Verify results display correctly
5. Test with supplier ID filter for overlapping rates

## 📋 Project Structure

SupplierManagement/
├── SupplierManagement.API/          # Web API project
│   ├── Controllers/                 # API controllers
│   ├── Properties/                  # Launch settings
│   └── Program.cs                   # API configuration
├── SupplierManagement.Core/         # Business logic
│   ├── DTOs/                        # Data transfer objects
│   ├── Entities/                    # Domain entities
│   ├── Interfaces/                  # Service interfaces
│   └── Services/                    # Business services
├── SupplierManagement.Infrastructure/ # Data access
│   ├── Data/                        # DbContext & seed data
│   └── Repositories/                # Data repositories
├── SupplierManagement.Web/          # Web application
│   ├── Controllers/                 # MVC controllers
│   ├── Views/                       # Razor views & JavaScript
│   └── Program.cs                   # Web app configuration
├── README.md                        # This file
├── TESTING.md                       # Testing guidelines
├── ARCHITECTURE.md                  # Scalability analysis
└── SupplierManagement.sln          # Solution file

## ✨ Additional Features

- **Comprehensive Error Handling**: All endpoints include proper exception handling
- **Logging**: Structured logging throughout the application
- **Validation**: Model validation for all input DTOs
- **CORS Support**: Properly configured for cross-origin requests
- **Swagger Documentation**: Complete API documentation with examples
- **Clean Code**: Following SOLID principles and best practices

## 🤝 Contributing

This is a coding exercise project. For production use, consider:

- Replace in-memory database with production database
- Implement comprehensive unit and integration tests
- Add input validation and sanitization
- Implement rate limiting and advanced security measures
- Add monitoring and health checks
- Configure for containerization and cloud deployment

**Restore dependencies:**

   ```bash
   dotnet restore
   ```

**Build the solution:**

   ```bash
   dotnet build
   ```

**Run the API:**

   ```bash
   cd SupplierManagement.API
   dotnet run
   ```

**Access the application:**

- API will run on: `https://localhost:7xxx` (port will be displayed in console)
- Swagger UI: `https://localhost:7xxx/swagger`

## 📝 API Usage Examples

### 1. Get Authentication Token

```bash
POST /api/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "password123"
}
```

**Response:**

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expires": "2025-07-25T15:30:00Z",
  "username": "admin"
}
```

### 2. Get All Suppliers with Rates

```bash
GET /api/suppliers/all-with-rates
Authorization: Bearer {your-jwt-token}
```

**Response:**

```json
[
  {
    "supplierId": 1,
    "name": "BestValue",
    "address": "1, Main Street, The District, City1, XXX-AADA",
    "createdByUser": "System.Admin",
    "createdOn": "2021-07-30T00:00:00",
    "rates": [
      {
        "supplierRateId": 1,
        "supplierId": 1,
        "rate": 10.00,
        "rateStartDate": "2015-01-01T00:00:00",
        "rateEndDate": "2015-03-31T00:00:00",
        "createdByUser": "System.Admin",
        "createdOn": "2021-07-30T00:00:00"
      }
    ]
  }
]
```

### 3. Get Overlapping Rates

**All suppliers:**

```bash
GET /api/suppliers/overlapping-rates
Authorization: Bearer {your-jwt-token}
```

**Specific supplier:**

```bash
GET /api/suppliers/overlapping-rates?supplierId=3
Authorization: Bearer {your-jwt-token}
```

**Response:**

```json
[
  {
    "supplierId": 3,
    "supplierName": "Premium Ltd",
    "overlappingRates": [
      {
        "supplierRateId": 6,
        "supplierId": 3,
        "rate": 30.00,
        "rateStartDate": "2016-12-01T00:00:00",
        "rateEndDate": "2017-01-01T00:00:00",
        "createdByUser": "System.Admin",
        "createdOn": "2021-07-30T00:00:00"
      },
      {
        "supplierRateId": 7,
        "supplierId": 3,
        "rate": 30.00,
        "rateStartDate": "2017-01-02T00:00:00",
        "rateEndDate": null,
        "createdByUser": "System.Admin",
        "createdOn": "2021-07-30T00:00:00"
      }
    ],
    "overlapReason": "Date ranges overlap or are contiguous"
  }
]
```

## 📊 Test Data

The application comes with pre-seeded test data:

### Suppliers

| ID | Name | Address |
|----|------|---------|
| 1 | BestValue | 1, Main Street, The District, City1, XXX-AADA |
| 2 | Quality Corp | 2, High Street, Downtown, City2, YYY-BBBB |
| 3 | Premium Ltd | 3, Park Avenue, Uptown, City3, ZZZ-CCCC |

### Supplier Rates

| Supplier ID | Rate | Start Date | End Date |
|-------------|------|------------|----------|
| 1 | 10 | 2015-01-01 | 2015-03-31 |
| 1 | 20 | 2015-04-01 | 2015-05-01 |
| 1 | 10 | 2015-05-30 | 2015-07-25 |
| 1 | 25 | 2015-10-01 | null |
| 2 | 100 | 2016-11-01 | null |
| 3 | 30 | 2016-12-01 | 2017-01-01 |
| 3 | 30 | 2017-01-02 | null |

## 🔒 Security Features

- JWT token-based authentication
- CORS enabled for web client integration (Exercise 3)
- Secure password handling (demo implementation)
- Request validation and error handling
- Structured logging

## 🧪 Testing with Swagger

1. Start the application
2. Navigate to `https://localhost:7xxx/swagger`
3. Use the "Authorize" button to enter your JWT token
4. Test the endpoints directly from the Swagger UI

## 🔍 Business Logic - Overlap Detection

The overlap detection algorithm:

1. **Sorts rates by start date** for each supplier
2. **Checks consecutive rates** for overlaps
3. **Handles null end dates** (rates that continue indefinitely)
4. **Identifies overlapping periods** where:
   - Next rate starts before current rate ends
   - Current rate has no end date (null) and there's a subsequent rate

## ⚙️ Configuration

Key configuration in `appsettings.json`:

```json
{
  "Jwt": {
    "Key": "your-secret-key-here-must-be-at-least-32-characters-long-for-security",
    "Issuer": "SupplierManagement.API",
    "Audience": "SupplierManagement.API"
  }
}
```

## 🚀 Next Steps

This API is ready for **Exercise 3** integration, which will create a separate web application to consume these endpoints.

## 📈 Scalability Considerations

For the design explanation requirement, see the separate architectural analysis document discussing how this system could scale to support millions of suppliers and rates with high availability requirements.
