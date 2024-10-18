using BikeRentalManagement.Models;
using BikeRentalManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class RentalController : ControllerBase
{
    private readonly RentalService _rentalService;

    public RentalController(RentalService rentalService)
    {
        _rentalService = rentalService;
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
}
