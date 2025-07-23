using SurveyBackend.Api.Interfaces;
using SurveyBackend.Domain.Entities.CreateDto;

namespace SurveyBackend.Api.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class VendorController : ControllerBase
{
    private readonly IVendorService _vendorService;

    public VendorController(IVendorService vendorService)
    {
        _vendorService = vendorService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateVendor([FromBody] VendorCreateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        try
        {
            var vendor = await _vendorService.CreateVendorAsync(dto);
            return CreatedAtAction(nameof(GetVendorById), new { id = vendor.vendor_id }, vendor);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while creating the vendor: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetVendors()
    {
        try
        {
            var vendors = await _vendorService.GetVendorsAsync();
            return Ok(vendors);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while retrieving vendors: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetVendorById(int id)
    {
        try
        {
            var vendor = await _vendorService.GetVendorByIdAsync(id);
            if (vendor == null)
            {
                return NotFound($"Vendor with ID {id} not found.");
            }

            return Ok(vendor);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while retrieving the vendor: {ex.Message}");
        }
    }
    
    [HttpGet("{vendorId}/survey-details")]
    public async Task<IActionResult> GetVendorSurveyDetails(int vendorId)
    {
        try
        {
            var details = await _vendorService.GetVendorSurveyDetailsAsync(vendorId);
            return Ok(details);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while retrieving survey details: {ex.Message}");
        }
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateVendor(int id, [FromBody] VendorCreateDTO dto)
    {
        try
        {
            var updatedVendor = await _vendorService.UpdateVendorAsync(id, dto);
            if (updatedVendor == null)
            {
                return NotFound($"Vendor with ID {id} not found for update.");
            }

            return Ok(updatedVendor);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message }); // 409 Conflict
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while updating the vendor: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVendor(int id)
    {
        try
        {
            var deleted = await _vendorService.DeleteVendorAsync(id);
            if (!deleted)
            {
                return NotFound($"Vendor with ID {id} not found for deletion.");
            }

            return Ok(new { message = "Vendor deleted successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while deleting the vendor: {ex.Message}");
        }
    }
}
