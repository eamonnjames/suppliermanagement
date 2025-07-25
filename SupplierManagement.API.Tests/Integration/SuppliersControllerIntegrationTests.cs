using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using SupplierManagement.API.Tests.Configuration;
using SupplierManagement.Core.DTOs;
using SupplierManagement.Core.Interfaces;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace SupplierManagement.API.Tests.Integration
{
    public class SuppliersControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private readonly Mock<ISupplierService> _mockSupplierService;

        public SuppliersControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _mockSupplierService = new Mock<ISupplierService>();

            _factory = TestWebApplicationFactory.CreateWithMockedAuth()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        // Remove the real service
                        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ISupplierService));
                        if (descriptor != null)
                            services.Remove(descriptor);

                        // Add mock service
                        services.AddSingleton(_mockSupplierService.Object);
                    });
                });

            _client = _factory.CreateClient();
        }        [Fact]
        public async Task GetAllSuppliersWithRates_ReturnsSuccessStatusCode()
        {
            // Arrange
            var expectedSuppliers = new List<SupplierWithRatesDto>
            {
                new SupplierWithRatesDto
                {
                    SupplierId = 1,
                    Name = "Test Supplier",
                    Address = "Test Address",
                    CreatedByUser = "TestUser",
                    CreatedOn = DateTime.Now,
                    Rates = new List<SupplierRateDto>()
                }
            };

            _mockSupplierService.Setup(s => s.GetAllSuppliersWithRatesAsync())
                               .ReturnsAsync(expectedSuppliers);

            // Act
            var response = await _client.GetAsync("/api/suppliers/all-with-rates");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetOverlappingRates_WithoutParameters_ReturnsSuccessStatusCode()
        {
            // Arrange
            var expectedOverlaps = new List<OverlappingRateDto>();
            _mockSupplierService.Setup(s => s.GetOverlappingSuppliersAndRatesAsync(null))
                               .ReturnsAsync(expectedOverlaps);

            // Act
            var response = await _client.GetAsync("/api/suppliers/overlapping-rates");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetOverlappingRates_WithSupplierId_ReturnsSuccessStatusCode()
        {
            // Arrange
            int supplierId = 1;
            var expectedOverlaps = new List<OverlappingRateDto>();
            _mockSupplierService.Setup(s => s.GetOverlappingSuppliersAndRatesAsync(supplierId))
                               .ReturnsAsync(expectedOverlaps);

            // Act
            var response = await _client.GetAsync($"/api/suppliers/overlapping-rates?supplierId={supplierId}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetAllSuppliers_ReturnsSuccessStatusCode()
        {
            // Arrange
            var expectedSuppliers = new List<SupplierDto>
            {
                new SupplierDto
                {
                    SupplierId = 1,
                    Name = "Test Supplier",
                    Address = "Test Address",
                    CreatedByUser = "TestUser",
                    CreatedOn = DateTime.Now
                }
            };

            _mockSupplierService.Setup(s => s.GetAllSuppliersAsync())
                               .ReturnsAsync(expectedSuppliers);

            // Act
            var response = await _client.GetAsync("/api/suppliers");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var suppliers = JsonSerializer.Deserialize<List<SupplierDto>>(content, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            Assert.NotNull(suppliers);
            Assert.Single(suppliers);
            Assert.Equal("Test Supplier", suppliers[0].Name);
        }

        [Fact]
        public async Task GetSupplierById_ExistingId_ReturnsSupplier()
        {
            // Arrange
            int supplierId = 1;
            var expectedSupplier = new SupplierDto
            {
                SupplierId = supplierId,
                Name = "Test Supplier",
                Address = "Test Address",
                CreatedByUser = "TestUser",
                CreatedOn = DateTime.Now
            };

            _mockSupplierService.Setup(s => s.GetSupplierByIdAsync(supplierId))
                               .ReturnsAsync(expectedSupplier);

            // Act
            var response = await _client.GetAsync($"/api/suppliers/{supplierId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var supplier = JsonSerializer.Deserialize<SupplierDto>(content, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            Assert.NotNull(supplier);
            Assert.Equal(supplierId, supplier.SupplierId);
            Assert.Equal("Test Supplier", supplier.Name);
        }

        [Fact]
        public async Task GetSupplierById_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            int supplierId = 999;
            _mockSupplierService.Setup(s => s.GetSupplierByIdAsync(supplierId))
                               .ReturnsAsync((SupplierDto?)null);

            // Act
            var response = await _client.GetAsync($"/api/suppliers/{supplierId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateSupplier_ValidData_ReturnsCreated()
        {
            // Arrange
            var createDto = new CreateSupplierDto
            {
                Name = "New Supplier",
                Address = "New Address",
                CreatedByUser = "TestUser"
            };

            var createdSupplier = new SupplierDto
            {
                SupplierId = 1,
                Name = createDto.Name,
                Address = createDto.Address,
                CreatedByUser = createDto.CreatedByUser,
                CreatedOn = DateTime.Now
            };

            _mockSupplierService.Setup(s => s.CreateSupplierAsync(It.IsAny<CreateSupplierDto>()))
                               .ReturnsAsync(createdSupplier);

            // Act
            var response = await _client.PostAsJsonAsync("/api/suppliers", createDto);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Contains($"/api/suppliers/{createdSupplier.SupplierId}", response.Headers.Location?.ToString());
        }

        [Fact]
        public async Task UpdateSupplier_ExistingId_ReturnsOk()
        {
            // Arrange
            int supplierId = 1;
            var updateDto = new UpdateSupplierDto
            {
                Name = "Updated Supplier",
                Address = "Updated Address"
            };

            var updatedSupplier = new SupplierDto
            {
                SupplierId = supplierId,
                Name = updateDto.Name,
                Address = updateDto.Address,
                CreatedByUser = "TestUser",
                CreatedOn = DateTime.Now
            };

            _mockSupplierService.Setup(s => s.UpdateSupplierAsync(supplierId, It.IsAny<UpdateSupplierDto>()))
                               .ReturnsAsync(updatedSupplier);

            // Act
            var response = await _client.PutAsJsonAsync($"/api/suppliers/{supplierId}", updateDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task UpdateSupplier_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            int supplierId = 999;
            var updateDto = new UpdateSupplierDto
            {
                Name = "Updated Supplier",
                Address = "Updated Address"
            };

            _mockSupplierService.Setup(s => s.UpdateSupplierAsync(supplierId, It.IsAny<UpdateSupplierDto>()))
                               .ReturnsAsync((SupplierDto?)null);

            // Act
            var response = await _client.PutAsJsonAsync($"/api/suppliers/{supplierId}", updateDto);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteSupplier_ExistingId_ReturnsNoContent()
        {
            // Arrange
            int supplierId = 1;
            _mockSupplierService.Setup(s => s.DeleteSupplierAsync(supplierId))
                               .ReturnsAsync(true);

            // Act
            var response = await _client.DeleteAsync($"/api/suppliers/{supplierId}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteSupplier_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            int supplierId = 999;
            _mockSupplierService.Setup(s => s.DeleteSupplierAsync(supplierId))
                               .ReturnsAsync(false);

            // Act
            var response = await _client.DeleteAsync($"/api/suppliers/{supplierId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetRatesBySupplier_ExistingId_ReturnsRates()
        {
            // Arrange
            int supplierId = 1;
            var expectedRates = new List<SupplierRateDto>
            {
                new SupplierRateDto
                {
                    SupplierRateId = 1,
                    SupplierId = supplierId,
                    Rate = 100.50m,
                    RateStartDate = DateTime.Today,
                    RateEndDate = DateTime.Today.AddDays(30),
                    CreatedByUser = "TestUser",
                    CreatedOn = DateTime.Now
                }
            };

            _mockSupplierService.Setup(s => s.GetRatesBySupplierIdAsync(supplierId))
                               .ReturnsAsync(expectedRates);

            // Act
            var response = await _client.GetAsync($"/api/suppliers/{supplierId}/rates");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var rates = JsonSerializer.Deserialize<List<SupplierRateDto>>(content, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            Assert.NotNull(rates);
            Assert.Single(rates);
            Assert.Equal(100.50m, rates[0].Rate);
        }

        [Fact]
        public async Task CreateRate_ValidData_ReturnsCreated()
        {
            // Arrange
            var createRateDto = new CreateSupplierRateDto
            {
                SupplierId = 1,
                Rate = 150.75m,
                RateStartDate = DateTime.Today,
                RateEndDate = DateTime.Today.AddDays(60),
                CreatedByUser = "TestUser"
            };

            var createdRate = new SupplierRateDto
            {
                SupplierRateId = 1,
                SupplierId = createRateDto.SupplierId,
                Rate = createRateDto.Rate,
                RateStartDate = createRateDto.RateStartDate,
                RateEndDate = createRateDto.RateEndDate,
                CreatedByUser = createRateDto.CreatedByUser,
                CreatedOn = DateTime.Now
            };

            _mockSupplierService.Setup(s => s.CreateRateAsync(It.IsAny<CreateSupplierRateDto>()))
                               .ReturnsAsync(createdRate);

            // Act
            var response = await _client.PostAsJsonAsync("/api/suppliers/rates", createRateDto);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Contains($"/api/suppliers/rates/{createdRate.SupplierRateId}", response.Headers.Location?.ToString());
        }
    }
}
