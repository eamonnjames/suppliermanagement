using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupplierManagement.Core.DTOs;
using SupplierManagement.Core.Interfaces;

namespace SupplierManagement.API.Controllers
{
    [ApiController]
    [Route("api/suppliers")]
    [Authorize] // Require authentication for all endpoints
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _supplierService;
        private readonly ILogger<SuppliersController> _logger;

        public SuppliersController(ISupplierService supplierService, ILogger<SuppliersController> logger)
        {
            _supplierService = supplierService;
            _logger = logger;
        }

        /// <summary>
        /// Get all suppliers with their rates
        /// </summary>
        /// <returns>List of suppliers with their associated rates</returns>
        [HttpGet("all-with-rates")]
        public async Task<IActionResult> GetAllSuppliersWithRates()
        {
            try
            {
                _logger.LogInformation("Getting all suppliers with rates");
                var suppliers = await _supplierService.GetAllSuppliersWithRatesAsync();
                return Ok(suppliers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all suppliers with rates");
                return StatusCode(500, "Internal server error occurred");
            }
        }

        /// <summary>
        /// Get suppliers with overlapping rates
        /// </summary>
        /// <param name="supplierId">Optional: specific supplier ID to check for overlaps</param>
        /// <returns>List of suppliers with overlapping rates</returns>
        [HttpGet("overlapping-rates")]
        public async Task<IActionResult> GetOverlappingRates([FromQuery] int? supplierId = null)
        {
            try
            {
                if (supplierId.HasValue)
                {
                    _logger.LogInformation("Getting overlapping rates for supplier {SupplierId}", supplierId.Value);
                }
                else
                {
                    _logger.LogInformation("Getting overlapping rates for all suppliers");
                }

                var overlappingRates = await _supplierService.GetOverlappingSuppliersAndRatesAsync(supplierId);
                return Ok(overlappingRates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting overlapping rates for supplier {SupplierId}", supplierId);
                return StatusCode(500, "Internal server error occurred");
            }
        }

        // Exercise 1 CRUD Operations for Suppliers

        /// <summary>
        /// Get all suppliers
        /// </summary>
        /// <returns>List of all suppliers</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllSuppliers()
        {
            try
            {
                _logger.LogInformation("Getting all suppliers");
                var suppliers = await _supplierService.GetAllSuppliersAsync();
                return Ok(suppliers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all suppliers");
                return StatusCode(500, "Internal server error occurred");
            }
        }

        /// <summary>
        /// Get supplier by ID
        /// </summary>
        /// <param name="id">Supplier ID</param>
        /// <returns>Supplier details</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupplierById(int id)
        {
            try
            {
                _logger.LogInformation("Getting supplier {SupplierId}", id);
                var supplier = await _supplierService.GetSupplierByIdAsync(id);

                if (supplier == null)
                    return NotFound($"Supplier with ID {id} not found");

                return Ok(supplier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting supplier {SupplierId}", id);
                return StatusCode(500, "Internal server error occurred");
            }
        }

        /// <summary>
        /// Create a new supplier
        /// </summary>
        /// <param name="createSupplierDto">Supplier creation data</param>
        /// <returns>Created supplier</returns>
        [HttpPost]
        public async Task<IActionResult> CreateSupplier([FromBody] CreateSupplierDto createSupplierDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _logger.LogInformation("Creating new supplier {SupplierName}", createSupplierDto.Name);
                var createdSupplier = await _supplierService.CreateSupplierAsync(createSupplierDto);

                return CreatedAtAction(nameof(GetSupplierById),
                    new { id = createdSupplier.SupplierId }, createdSupplier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating supplier {SupplierName}", createSupplierDto.Name);
                return StatusCode(500, "Internal server error occurred");
            }
        }

        /// <summary>
        /// Update an existing supplier
        /// </summary>
        /// <param name="id">Supplier ID</param>
        /// <param name="updateSupplierDto">Supplier update data</param>
        /// <returns>Updated supplier</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplier(int id, [FromBody] UpdateSupplierDto updateSupplierDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _logger.LogInformation("Updating supplier {SupplierId}", id);
                var updatedSupplier = await _supplierService.UpdateSupplierAsync(id, updateSupplierDto);

                if (updatedSupplier == null)
                    return NotFound($"Supplier with ID {id} not found");

                return Ok(updatedSupplier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating supplier {SupplierId}", id);
                return StatusCode(500, "Internal server error occurred");
            }
        }

        /// <summary>
        /// Delete a supplier
        /// </summary>
        /// <param name="id">Supplier ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            try
            {
                _logger.LogInformation("Deleting supplier {SupplierId}", id);
                var deleted = await _supplierService.DeleteSupplierAsync(id);

                if (!deleted)
                    return NotFound($"Supplier with ID {id} not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting supplier {SupplierId}", id);
                return StatusCode(500, "Internal server error occurred");
            }
        }

        // Exercise 1 CRUD Operations for Supplier Rates

        /// <summary>
        /// Get rates for a specific supplier
        /// </summary>
        /// <param name="supplierId">Supplier ID</param>
        /// <returns>List of rates for the supplier</returns>
        [HttpGet("{supplierId}/rates")]
        public async Task<IActionResult> GetRatesBySupplier(int supplierId)
        {
            try
            {
                _logger.LogInformation("Getting rates for supplier {SupplierId}", supplierId);
                var rates = await _supplierService.GetRatesBySupplierIdAsync(supplierId);
                return Ok(rates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting rates for supplier {SupplierId}", supplierId);
                return StatusCode(500, "Internal server error occurred");
            }
        }

        /// <summary>
        /// Get rate by ID
        /// </summary>
        /// <param name="rateId">Rate ID</param>
        /// <returns>Rate details</returns>
        [HttpGet("rates/{rateId}")]
        public async Task<IActionResult> GetRateById(int rateId)
        {
            try
            {
                _logger.LogInformation("Getting rate {RateId}", rateId);
                var rate = await _supplierService.GetRateByIdAsync(rateId);

                if (rate == null)
                    return NotFound($"Rate with ID {rateId} not found");

                return Ok(rate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting rate {RateId}", rateId);
                return StatusCode(500, "Internal server error occurred");
            }
        }

        /// <summary>
        /// Create a new rate for a supplier
        /// </summary>
        /// <param name="createRateDto">Rate creation data</param>
        /// <returns>Created rate</returns>
        [HttpPost("rates")]
        public async Task<IActionResult> CreateRate([FromBody] CreateSupplierRateDto createRateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _logger.LogInformation("Creating new rate for supplier {SupplierId}", createRateDto.SupplierId);
                var createdRate = await _supplierService.CreateRateAsync(createRateDto);

                return CreatedAtAction(nameof(GetRateById),
                    new { rateId = createdRate.SupplierRateId }, createdRate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating rate for supplier {SupplierId}", createRateDto.SupplierId);
                return StatusCode(500, "Internal server error occurred");
            }
        }

        /// <summary>
        /// Update an existing rate
        /// </summary>
        /// <param name="rateId">Rate ID</param>
        /// <param name="updateRateDto">Rate update data</param>
        /// <returns>Updated rate</returns>
        [HttpPut("rates/{rateId}")]
        public async Task<IActionResult> UpdateRate(int rateId, [FromBody] UpdateSupplierRateDto updateRateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _logger.LogInformation("Updating rate {RateId}", rateId);
                var updatedRate = await _supplierService.UpdateRateAsync(rateId, updateRateDto);

                if (updatedRate == null)
                    return NotFound($"Rate with ID {rateId} not found");

                return Ok(updatedRate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating rate {RateId}", rateId);
                return StatusCode(500, "Internal server error occurred");
            }
        }

        /// <summary>
        /// Delete a rate
        /// </summary>
        /// <param name="rateId">Rate ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("rates/{rateId}")]
        public async Task<IActionResult> DeleteRate(int rateId)
        {
            try
            {
                _logger.LogInformation("Deleting rate {RateId}", rateId);
                var deleted = await _supplierService.DeleteRateAsync(rateId);

                if (!deleted)
                    return NotFound($"Rate with ID {rateId} not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting rate {RateId}", rateId);
                return StatusCode(500, "Internal server error occurred");
            }
        }
    }
}
