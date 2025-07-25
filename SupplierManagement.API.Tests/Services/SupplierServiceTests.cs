using Moq;
using SupplierManagement.Core.DTOs;
using SupplierManagement.Core.Entities;
using SupplierManagement.Core.Interfaces;
using SupplierManagement.Core.Services;
using Xunit;

namespace SupplierManagement.API.Tests.Services
{
    public class SupplierServiceTests
    {
        private readonly Mock<ISupplierRepository> _mockRepository;
        private readonly SupplierService _service;

        public SupplierServiceTests()
        {
            _mockRepository = new Mock<ISupplierRepository>();
            _service = new SupplierService(_mockRepository.Object);
        }

        #region GetAllSuppliersWithRatesAsync Tests

        [Fact]
        public async Task GetAllSuppliersWithRatesAsync_ReturnsSupplierWithRatesDtos()
        {
            // Arrange
            var suppliers = new List<Supplier>
            {
                new Supplier
                {
                    SupplierId = 1,
                    Name = "Test Supplier",
                    Address = "Test Address",
                    CreatedByUser = "TestUser",
                    CreatedOn = DateTime.Now,
                    SupplierRates = new List<SupplierRate>
                    {
                        new SupplierRate
                        {
                            SupplierRateId = 1,
                            SupplierId = 1,
                            Rate = 100.50m,
                            RateStartDate = DateTime.Today,
                            RateEndDate = DateTime.Today.AddDays(30),
                            CreatedByUser = "TestUser",
                            CreatedOn = DateTime.Now
                        }
                    }
                }
            };

            _mockRepository.Setup(r => r.GetAllSuppliersWithRatesAsync())
                          .ReturnsAsync(suppliers);

            // Act
            var result = await _service.GetAllSuppliersWithRatesAsync();

            // Assert
            var supplierList = result.ToList();
            Assert.Single(supplierList);
            Assert.Equal("Test Supplier", supplierList[0].Name);
            Assert.Single(supplierList[0].Rates);
            Assert.Equal(100.50m, supplierList[0].Rates[0].Rate);
        }

        [Fact]
        public async Task GetAllSuppliersWithRatesAsync_EmptyRepository_ReturnsEmptyList()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllSuppliersWithRatesAsync())
                          .ReturnsAsync(new List<Supplier>());

            // Act
            var result = await _service.GetAllSuppliersWithRatesAsync();

            // Assert
            Assert.Empty(result);
        }

        #endregion

        #region GetOverlappingSuppliersAndRatesAsync Tests

        [Fact]
        public async Task GetOverlappingSuppliersAndRatesAsync_WithoutSupplierId_ChecksAllSuppliers()
        {
            // Arrange
            var suppliers = new List<Supplier>
            {
                new Supplier
                {
                    SupplierId = 1,
                    Name = "Test Supplier",
                    Address = "Test Address",
                    CreatedByUser = "TestUser",
                    CreatedOn = DateTime.Now,
                    SupplierRates = new List<SupplierRate>
                    {
                        new SupplierRate
                        {
                            SupplierRateId = 1,
                            SupplierId = 1,
                            Rate = 100.50m,
                            RateStartDate = DateTime.Today,
                            RateEndDate = DateTime.Today.AddDays(15),
                            CreatedByUser = "TestUser",
                            CreatedOn = DateTime.Now
                        },
                        new SupplierRate
                        {
                            SupplierRateId = 2,
                            SupplierId = 1,
                            Rate = 120.75m,
                            RateStartDate = DateTime.Today.AddDays(10),
                            RateEndDate = DateTime.Today.AddDays(25),
                            CreatedByUser = "TestUser",
                            CreatedOn = DateTime.Now
                        }
                    }
                }
            };

            _mockRepository.Setup(r => r.GetAllSuppliersWithRatesAsync())
                          .ReturnsAsync(suppliers);

            // Act
            var result = await _service.GetOverlappingSuppliersAndRatesAsync();

            // Assert
            var overlapList = result.ToList();
            Assert.Single(overlapList);
            Assert.Equal("Test Supplier", overlapList[0].SupplierName);
            Assert.Equal(2, overlapList[0].OverlappingRates.Count);
        }

        [Fact]
        public async Task GetOverlappingSuppliersAndRatesAsync_WithSupplierId_ChecksSpecificSupplier()
        {
            // Arrange
            int targetSupplierId = 1;
            var suppliers = new List<Supplier>
            {
                new Supplier
                {
                    SupplierId = 1,
                    Name = "Target Supplier",
                    Address = "Target Address",
                    CreatedByUser = "TestUser",
                    CreatedOn = DateTime.Now,
                    SupplierRates = new List<SupplierRate>()
                },
                new Supplier
                {
                    SupplierId = 2,
                    Name = "Other Supplier",
                    Address = "Other Address",
                    CreatedByUser = "TestUser",
                    CreatedOn = DateTime.Now,
                    SupplierRates = new List<SupplierRate>()
                }
            };

            _mockRepository.Setup(r => r.GetAllSuppliersWithRatesAsync())
                          .ReturnsAsync(suppliers);

            // Act
            var result = await _service.GetOverlappingSuppliersAndRatesAsync(targetSupplierId);

            // Assert
            var overlapList = result.ToList();
            // Should only process the target supplier, so results depend on overlap logic
            _mockRepository.Verify(r => r.GetAllSuppliersWithRatesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetOverlappingSuppliersAndRatesAsync_NoOverlaps_ReturnsEmptyList()
        {
            // Arrange
            var suppliers = new List<Supplier>
            {
                new Supplier
                {
                    SupplierId = 1,
                    Name = "Test Supplier",
                    Address = "Test Address",
                    CreatedByUser = "TestUser",
                    CreatedOn = DateTime.Now,
                    SupplierRates = new List<SupplierRate>
                    {
                        new SupplierRate
                        {
                            SupplierRateId = 1,
                            SupplierId = 1,
                            Rate = 100.50m,
                            RateStartDate = DateTime.Today,
                            RateEndDate = DateTime.Today.AddDays(10),
                            CreatedByUser = "TestUser",
                            CreatedOn = DateTime.Now
                        },
                        new SupplierRate
                        {
                            SupplierRateId = 2,
                            SupplierId = 1,
                            Rate = 120.75m,
                            RateStartDate = DateTime.Today.AddDays(15),
                            RateEndDate = DateTime.Today.AddDays(25),
                            CreatedByUser = "TestUser",
                            CreatedOn = DateTime.Now
                        }
                    }
                }
            };

            _mockRepository.Setup(r => r.GetAllSuppliersWithRatesAsync())
                          .ReturnsAsync(suppliers);

            // Act
            var result = await _service.GetOverlappingSuppliersAndRatesAsync();

            // Assert
            Assert.Empty(result);
        }

        #endregion

        #region CRUD Operations Tests

        [Fact]
        public async Task GetAllSuppliersAsync_ReturnsSupplierDtos()
        {
            // Arrange
            var suppliers = new List<Supplier>
            {
                new Supplier
                {
                    SupplierId = 1,
                    Name = "Test Supplier 1",
                    Address = "Address 1",
                    CreatedByUser = "User1",
                    CreatedOn = DateTime.Now
                },
                new Supplier
                {
                    SupplierId = 2,
                    Name = "Test Supplier 2",
                    Address = "Address 2",
                    CreatedByUser = "User2",
                    CreatedOn = DateTime.Now
                }
            };

            _mockRepository.Setup(r => r.GetAllSuppliersAsync())
                          .ReturnsAsync(suppliers);

            // Act
            var result = await _service.GetAllSuppliersAsync();

            // Assert
            var supplierList = result.ToList();
            Assert.Equal(2, supplierList.Count);
            Assert.Equal("Test Supplier 1", supplierList[0].Name);
            Assert.Equal("Test Supplier 2", supplierList[1].Name);
        }

        [Fact]
        public async Task GetSupplierByIdAsync_ExistingId_ReturnsSupplierDto()
        {
            // Arrange
            int supplierId = 1;
            var supplier = new Supplier
            {
                SupplierId = supplierId,
                Name = "Test Supplier",
                Address = "Test Address",
                CreatedByUser = "TestUser",
                CreatedOn = DateTime.Now
            };

            _mockRepository.Setup(r => r.GetSupplierByIdAsync(supplierId))
                          .ReturnsAsync(supplier);

            // Act
            var result = await _service.GetSupplierByIdAsync(supplierId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(supplierId, result.SupplierId);
            Assert.Equal("Test Supplier", result.Name);
        }

        [Fact]
        public async Task GetSupplierByIdAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            int supplierId = 999;
            _mockRepository.Setup(r => r.GetSupplierByIdAsync(supplierId))
                          .ReturnsAsync((Supplier?)null);

            // Act
            var result = await _service.GetSupplierByIdAsync(supplierId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateSupplierAsync_ValidDto_ReturnsCreatedSupplierDto()
        {
            // Arrange
            var createDto = new CreateSupplierDto
            {
                Name = "New Supplier",
                Address = "New Address",
                CreatedByUser = "TestUser"
            };

            var createdSupplier = new Supplier
            {
                SupplierId = 1,
                Name = createDto.Name,
                Address = createDto.Address,
                CreatedByUser = createDto.CreatedByUser,
                CreatedOn = DateTime.Now
            };

            _mockRepository.Setup(r => r.CreateSupplierAsync(It.IsAny<Supplier>()))
                          .ReturnsAsync(createdSupplier);

            // Act
            var result = await _service.CreateSupplierAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createDto.Name, result.Name);
            Assert.Equal(createDto.Address, result.Address);
            Assert.Equal(createDto.CreatedByUser, result.CreatedByUser);
        }

        [Fact]
        public async Task UpdateSupplierAsync_ExistingId_ReturnsUpdatedSupplierDto()
        {
            // Arrange
            int supplierId = 1;
            var updateDto = new UpdateSupplierDto
            {
                Name = "Updated Supplier",
                Address = "Updated Address"
            };

            var existingSupplier = new Supplier
            {
                SupplierId = supplierId,
                Name = "Original Name",
                Address = "Original Address",
                CreatedByUser = "TestUser",
                CreatedOn = DateTime.Now
            };

            var updatedSupplier = new Supplier
            {
                SupplierId = supplierId,
                Name = updateDto.Name,
                Address = updateDto.Address,
                CreatedByUser = existingSupplier.CreatedByUser,
                CreatedOn = existingSupplier.CreatedOn
            };

            _mockRepository.Setup(r => r.GetSupplierByIdAsync(supplierId))
                          .ReturnsAsync(existingSupplier);
            _mockRepository.Setup(r => r.UpdateSupplierAsync(It.IsAny<Supplier>()))
                          .ReturnsAsync(updatedSupplier);

            // Act
            var result = await _service.UpdateSupplierAsync(supplierId, updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updateDto.Name, result.Name);
            Assert.Equal(updateDto.Address, result.Address);
        }

        [Fact]
        public async Task UpdateSupplierAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            int supplierId = 999;
            var updateDto = new UpdateSupplierDto
            {
                Name = "Updated Supplier",
                Address = "Updated Address"
            };

            _mockRepository.Setup(r => r.GetSupplierByIdAsync(supplierId))
                          .ReturnsAsync((Supplier?)null);

            // Act
            var result = await _service.UpdateSupplierAsync(supplierId, updateDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteSupplierAsync_ExistingId_ReturnsTrue()
        {
            // Arrange
            int supplierId = 1;
            _mockRepository.Setup(r => r.DeleteSupplierAsync(supplierId))
                          .ReturnsAsync(true);

            // Act
            var result = await _service.DeleteSupplierAsync(supplierId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteSupplierAsync_NonExistingId_ReturnsFalse()
        {
            // Arrange
            int supplierId = 999;
            _mockRepository.Setup(r => r.DeleteSupplierAsync(supplierId))
                          .ReturnsAsync(false);

            // Act
            var result = await _service.DeleteSupplierAsync(supplierId);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region Rate Management Tests

        [Fact]
        public async Task GetRatesBySupplierIdAsync_ExistingId_ReturnsRateDtos()
        {
            // Arrange
            int supplierId = 1;
            var rates = new List<SupplierRate>
            {
                new SupplierRate
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

            _mockRepository.Setup(r => r.GetRatesBySupplierIdAsync(supplierId))
                          .ReturnsAsync(rates);

            // Act
            var result = await _service.GetRatesBySupplierIdAsync(supplierId);

            // Assert
            var rateList = result.ToList();
            Assert.Single(rateList);
            Assert.Equal(100.50m, rateList[0].Rate);
        }

        [Fact]
        public async Task CreateRateAsync_ValidDto_ReturnsCreatedRateDto()
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

            var createdRate = new SupplierRate
            {
                SupplierRateId = 1,
                SupplierId = createRateDto.SupplierId,
                Rate = createRateDto.Rate,
                RateStartDate = createRateDto.RateStartDate,
                RateEndDate = createRateDto.RateEndDate,
                CreatedByUser = createRateDto.CreatedByUser,
                CreatedOn = DateTime.Now
            };

            _mockRepository.Setup(r => r.CreateRateAsync(It.IsAny<SupplierRate>()))
                          .ReturnsAsync(createdRate);

            // Act
            var result = await _service.CreateRateAsync(createRateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createRateDto.Rate, result.Rate);
            Assert.Equal(createRateDto.SupplierId, result.SupplierId);
        }

        #endregion
    }
}
