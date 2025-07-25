using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SupplierManagement.API.Controllers;
using SupplierManagement.Core.DTOs;
using SupplierManagement.Core.Interfaces;

namespace SupplierManagement.API.Tests.Controllers
{
    public class SuppliersControllerTests
    {
        private readonly Mock<ISupplierService> _mockSupplierService;
        private readonly Mock<ILogger<SuppliersController>> _mockLogger;
        private readonly SuppliersController _controller;

        public SuppliersControllerTests()
        {
            _mockSupplierService = new Mock<ISupplierService>();
            _mockLogger = new Mock<ILogger<SuppliersController>>();
            _controller = new SuppliersController(_mockSupplierService.Object, _mockLogger.Object);
        }

        #region GetAllSuppliersWithRates Tests

        [Fact]
        public async Task GetAllSuppliersWithRates_ReturnsOkResult_WithSuppliers()
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
                    Rates = new List<SupplierRateDto>
                    {
                        new SupplierRateDto
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

            _mockSupplierService.Setup(s => s.GetAllSuppliersWithRatesAsync())
                               .ReturnsAsync(expectedSuppliers);

            // Act
            var result = await _controller.GetAllSuppliersWithRates();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualSuppliers = Assert.IsAssignableFrom<IEnumerable<SupplierWithRatesDto>>(okResult.Value);
            Assert.Single(actualSuppliers);
            Assert.Equal(expectedSuppliers.First().Name, actualSuppliers.First().Name);
        }

        [Fact]
        public async Task GetAllSuppliersWithRates_ReturnsInternalServerError_WhenExceptionThrown()
        {
            // Arrange
            _mockSupplierService.Setup(s => s.GetAllSuppliersWithRatesAsync())
                               .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetAllSuppliersWithRates();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Internal server error occurred", statusCodeResult.Value);
        }

        #endregion

        #region GetOverlappingRates Tests

        [Fact]
        public async Task GetOverlappingRates_WithoutSupplierId_ReturnsOkResult()
        {
            // Arrange
            var expectedOverlaps = new List<OverlappingRateDto>
            {
                new OverlappingRateDto
                {
                    SupplierId = 1,
                    SupplierName = "Test Supplier",
                    OverlappingRates = new List<SupplierRateDto>(),
                    OverlapReason = "Overlapping periods detected"
                }
            };

            _mockSupplierService.Setup(s => s.GetOverlappingSuppliersAndRatesAsync(null))
                               .ReturnsAsync(expectedOverlaps);

            // Act
            var result = await _controller.GetOverlappingRates();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualOverlaps = Assert.IsAssignableFrom<IEnumerable<OverlappingRateDto>>(okResult.Value);
            Assert.Single(actualOverlaps);
        }

        [Fact]
        public async Task GetOverlappingRates_WithSupplierId_ReturnsOkResult()
        {
            // Arrange
            int supplierId = 1;
            var expectedOverlaps = new List<OverlappingRateDto>();

            _mockSupplierService.Setup(s => s.GetOverlappingSuppliersAndRatesAsync(supplierId))
                               .ReturnsAsync(expectedOverlaps);

            // Act
            var result = await _controller.GetOverlappingRates(supplierId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualOverlaps = Assert.IsAssignableFrom<IEnumerable<OverlappingRateDto>>(okResult.Value);
            Assert.Empty(actualOverlaps);
        }

        [Fact]
        public async Task GetOverlappingRates_ReturnsInternalServerError_WhenExceptionThrown()
        {
            // Arrange
            _mockSupplierService.Setup(s => s.GetOverlappingSuppliersAndRatesAsync(It.IsAny<int?>()))
                               .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetOverlappingRates();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        #endregion

        #region GetAllSuppliers Tests

        [Fact]
        public async Task GetAllSuppliers_ReturnsOkResult_WithSuppliers()
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
            var result = await _controller.GetAllSuppliers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualSuppliers = Assert.IsAssignableFrom<IEnumerable<SupplierDto>>(okResult.Value);
            Assert.Single(actualSuppliers);
        }

        #endregion

        #region GetSupplierById Tests

        [Fact]
        public async Task GetSupplierById_ExistingId_ReturnsOkResult()
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
            var result = await _controller.GetSupplierById(supplierId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualSupplier = Assert.IsType<SupplierDto>(okResult.Value);
            Assert.Equal(expectedSupplier.SupplierId, actualSupplier.SupplierId);
        }

        [Fact]
        public async Task GetSupplierById_NonExistingId_ReturnsNotFoundResult()
        {
            // Arrange
            int supplierId = 999;
            _mockSupplierService.Setup(s => s.GetSupplierByIdAsync(supplierId))
                               .ReturnsAsync((SupplierDto?)null);

            // Act
            var result = await _controller.GetSupplierById(supplierId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion

        #region CreateSupplier Tests

        [Fact]
        public async Task CreateSupplier_ValidDto_ReturnsCreatedAtActionResult()
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

            _mockSupplierService.Setup(s => s.CreateSupplierAsync(createDto))
                               .ReturnsAsync(createdSupplier);

            // Act
            var result = await _controller.CreateSupplier(createDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(SuppliersController.GetSupplierById), createdAtActionResult.ActionName);
            Assert.Equal(createdSupplier.SupplierId, createdAtActionResult.RouteValues["id"]);
        }

        [Fact]
        public async Task CreateSupplier_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var createDto = new CreateSupplierDto();
            _controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await _controller.CreateSupplier(createDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        #endregion

        #region UpdateSupplier Tests

        [Fact]
        public async Task UpdateSupplier_ExistingId_ReturnsOkResult()
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

            _mockSupplierService.Setup(s => s.UpdateSupplierAsync(supplierId, updateDto))
                               .ReturnsAsync(updatedSupplier);

            // Act
            var result = await _controller.UpdateSupplier(supplierId, updateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualSupplier = Assert.IsType<SupplierDto>(okResult.Value);
            Assert.Equal(updatedSupplier.Name, actualSupplier.Name);
        }

        [Fact]
        public async Task UpdateSupplier_NonExistingId_ReturnsNotFoundResult()
        {
            // Arrange
            int supplierId = 999;
            var updateDto = new UpdateSupplierDto
            {
                Name = "Updated Supplier",
                Address = "Updated Address"
            };

            _mockSupplierService.Setup(s => s.UpdateSupplierAsync(supplierId, updateDto))
                               .ReturnsAsync((SupplierDto?)null);

            // Act
            var result = await _controller.UpdateSupplier(supplierId, updateDto);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion

        #region DeleteSupplier Tests

        [Fact]
        public async Task DeleteSupplier_ExistingId_ReturnsNoContentResult()
        {
            // Arrange
            int supplierId = 1;
            _mockSupplierService.Setup(s => s.DeleteSupplierAsync(supplierId))
                               .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteSupplier(supplierId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteSupplier_NonExistingId_ReturnsNotFoundResult()
        {
            // Arrange
            int supplierId = 999;
            _mockSupplierService.Setup(s => s.DeleteSupplierAsync(supplierId))
                               .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteSupplier(supplierId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion

        #region Rate Management Tests

        [Fact]
        public async Task GetRatesBySupplier_ExistingId_ReturnsOkResult()
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
            var result = await _controller.GetRatesBySupplier(supplierId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualRates = Assert.IsAssignableFrom<IEnumerable<SupplierRateDto>>(okResult.Value);
            Assert.Single(actualRates);
        }

        [Fact]
        public async Task CreateRate_ValidDto_ReturnsCreatedAtActionResult()
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

            _mockSupplierService.Setup(s => s.CreateRateAsync(createRateDto))
                               .ReturnsAsync(createdRate);

            // Act
            var result = await _controller.CreateRate(createRateDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(SuppliersController.GetRateById), createdAtActionResult.ActionName);
            Assert.Equal(createdRate.SupplierRateId, createdAtActionResult.RouteValues["rateId"]);
        }

        #endregion
    }
}
