using BikeRentalManagement.Models;
using BikeRentalManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class RentalController : ControllerBase
{
    private readonly RentalService _rentalService;
    private readonly RentalRequestService _rentalRequestService;

    public RentalController(RentalService rentalService, RentalRequestService rentalRequestService)
    {
        _rentalService = rentalService;
        _rentalRequestService = rentalRequestService;
    }
    [HttpPost]
    public IActionResult AddRental([FromBody] Rental rental)
    {
        if (_rentalService.AddRental(rental))
        {
            return Ok("Rental added successfully.");
        }

        return BadRequest("Failed to add rental.");
    }

    [HttpDelete("{rentalId}")]
    public IActionResult DeleteRental(int rentalId)
    {
        if (_rentalService.DeleteRental(rentalId))
        {
            return Ok("Rental deleted successfully.");
        }

        return NotFound("Rental not found.");
    }

    [HttpGet("{rentalId}")]
    public IActionResult GetRentalById(int rentalId)
    {
        var rental = _rentalService.GetRentalById(rentalId);
        if (rental == null)
        {
            return NotFound("Rental not found.");
        }

        return Ok(rental);
    }

    [HttpGet]
    public IActionResult GetAllRentals()
    {
        return Ok(_rentalService.GetAllRentals());
    }

    [HttpGet("overdue")]
    public async Task<ActionResult<IEnumerable<Rental>>> GetOverdueRentals()
    {
        var overdueRentals = await _rentalService.GetOverdueRentals();
        return Ok(overdueRentals);
    }

    
    [HttpPost("approve/{requestId}")]
    public async Task<ActionResult> ApproveRentalRequest(int requestId)
    {
        var success = await _rentalRequestService.ApproveRentalRequest(requestId);
        if (!success) return BadRequest("Failed to approve rental request.");

        return Ok("Rental request approved.");
    }


}
