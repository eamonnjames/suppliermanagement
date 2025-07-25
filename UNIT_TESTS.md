# Unit Tests Documentation

This document describes the comprehensive unit test suite implemented for both the API and Web components of the Supplier Management application.

## Test Projects

### 1. SupplierManagement.API.Tests

Contains unit and integration tests for the Web API project.

### 2. SupplierManagement.Web.Tests

Contains unit and integration tests for the Web (MVC) project.

## Test Coverage

### API Tests (SupplierManagement.API.Tests)

#### Controllers Tests (SuppliersControllerTests.cs)

Tests the `SuppliersController` endpoints:

**GetAllSuppliersWithRates Tests:**

- ✅ Returns OK result with suppliers
- ✅ Returns internal server error when exception thrown

**GetOverlappingRates Tests:**

- ✅ Without supplier ID returns OK result
- ✅ With supplier ID returns OK result
- ✅ Returns internal server error when exception thrown

**GetAllSuppliers Tests:**

- ✅ Returns OK result with suppliers

**GetSupplierById Tests:**

- ✅ Existing ID returns OK result
- ✅ Non-existing ID returns NotFound result

**CreateSupplier Tests:**

- ✅ Valid DTO returns CreatedAtAction result
- ✅ Invalid model state returns BadRequest

**UpdateSupplier Tests:**

- ✅ Existing ID returns OK result
- ✅ Non-existing ID returns NotFound result

**DeleteSupplier Tests:**

- ✅ Existing ID returns NoContent result
- ✅ Non-existing ID returns NotFound result

**Rate Management Tests:**

- ✅ GetRatesBySupplier existing ID returns OK result
- ✅ CreateRate valid DTO returns CreatedAtAction result

#### Services Tests (SupplierServiceTests.cs)

Tests the `SupplierService` business logic:

**GetAllSuppliersWithRatesAsync Tests:**

- ✅ Returns SupplierWithRatesDtos
- ✅ Empty repository returns empty list

**GetOverlappingSuppliersAndRatesAsync Tests:**

- ✅ Without supplier ID checks all suppliers
- ✅ With supplier ID checks specific supplier
- ✅ No overlaps returns empty list

**CRUD Operations Tests:**

- ✅ GetAllSuppliersAsync returns SupplierDtos
- ✅ GetSupplierByIdAsync existing ID returns SupplierDto
- ✅ GetSupplierByIdAsync non-existing ID returns null
- ✅ CreateSupplierAsync valid DTO returns created SupplierDto
- ✅ UpdateSupplierAsync existing ID returns updated SupplierDto
- ✅ UpdateSupplierAsync non-existing ID returns null
- ✅ DeleteSupplierAsync existing ID returns true
- ✅ DeleteSupplierAsync non-existing ID returns false

**Rate Management Tests:**

- ✅ GetRatesBySupplierIdAsync existing ID returns RateDtos
- ✅ CreateRateAsync valid DTO returns created RateDto

#### Integration Tests (SuppliersControllerIntegrationTests.cs)

Full HTTP endpoint tests using WebApplicationFactory:

**API Endpoint Tests:**

- ✅ GET `/api/suppliers/all-with-rates` returns success
- ✅ GET `/api/suppliers/overlapping-rates` (without parameters) returns success
- ✅ GET `/api/suppliers/overlapping-rates?supplierId=1` returns success
- ✅ GET `/api/suppliers` returns success with data validation
- ✅ GET `/api/suppliers/{id}` existing ID returns supplier
- ✅ GET `/api/suppliers/{id}` non-existing ID returns NotFound
- ✅ POST `/api/suppliers` valid data returns Created
- ✅ PUT `/api/suppliers/{id}` existing ID returns OK
- ✅ PUT `/api/suppliers/{id}` non-existing ID returns NotFound
- ✅ DELETE `/api/suppliers/{id}` existing ID returns NoContent
- ✅ DELETE `/api/suppliers/{id}` non-existing ID returns NotFound
- ✅ GET `/api/suppliers/{id}/rates` returns rates
- ✅ POST `/api/suppliers/rates` valid data returns Created

### Web Tests (SupplierManagement.Web.Tests)

#### Controllers Tests (HomeControllerTests.cs)

Tests the `HomeController` MVC endpoints:

**Index Action Tests:**

- ✅ Returns ViewResult
- ✅ Returns non-null result

**Error Action Tests:**

- ✅ Returns ViewResult
- ✅ Returns non-null result

#### Integration Tests (HomeControllerIntegrationTests.cs)

Full HTTP endpoint tests for the MVC application:

**Homepage Tests:**

- ✅ Home index returns success status code
- ✅ Home index returns HTML content
- ✅ Home index contains expected HTML elements
- ✅ Alternative routes (`/Home`, `/Home/Index`) return success

**Error Handling Tests:**

- ✅ Non-existent page returns NotFound
- ✅ Error page returns success status code

**Application Tests:**

- ✅ Static files handling (CSS)
- ✅ Application starts successfully

## Test Utilities

### TestDataFactory.cs

Provides factory methods for creating test data:

- `CreateSupplier()` - Creates test Supplier entities
- `CreateSupplierRate()` - Creates test SupplierRate entities
- `CreateSupplierDto()` - Creates test SupplierDto objects
- `CreateSupplierWithRatesDto()` - Creates test SupplierWithRatesDto objects
- `CreateSuppliersWithOverlappingRates()` - Creates test data with overlapping rates
- `CreateSuppliersWithoutOverlappingRates()` - Creates test data without overlapping rates

## Test Technologies Used

- **xUnit** - Primary testing framework
- **Moq** - Mocking framework for isolating dependencies
- **Microsoft.AspNetCore.Mvc.Testing** - Integration testing for ASP.NET Core applications
- **WebApplicationFactory** - For creating test servers

## Running Tests

### Run All Tests

```powershell
dotnet test
```

### Run Specific Test Project

```powershell
# API Tests
dotnet test SupplierManagement.API.Tests

# Web Tests
dotnet test SupplierManagement.Web.Tests
```

### Run with Coverage (if coverage tools are installed)

```powershell
dotnet test --collect:"XPlat Code Coverage"
```

## Test Architecture

### Unit Tests

- **Isolated** - Use mocks to isolate the system under test
- **Fast** - Execute quickly without external dependencies
- **Focused** - Test single units of functionality

### Integration Tests

- **End-to-End** - Test complete HTTP request/response cycles
- **Realistic** - Use actual ASP.NET Core test server
- **Comprehensive** - Validate serialization, routing, and middleware

## Best Practices Implemented

1. **AAA Pattern** - Arrange, Act, Assert structure in all tests
2. **Descriptive Names** - Test method names clearly describe what is being tested
3. **Single Responsibility** - Each test verifies one specific behavior
4. **Test Data Factory** - Centralized creation of test data
5. **Mocking** - Proper isolation of dependencies
6. **Both Positive and Negative Cases** - Testing success and failure scenarios
7. **Integration Testing** - Validating full application behavior

## Future Enhancements

Consider adding:

- Performance tests for high-load scenarios
- Security tests for authentication/authorization
- Data validation tests with edge cases
- Database integration tests with real database
- API contract tests
- Load testing with tools like NBomber or k6
