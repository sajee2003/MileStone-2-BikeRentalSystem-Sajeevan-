// File: Controllers/MotorbikeController.cs
using BikeRentalManagement.Models;
using BikeRentalManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class MotorbikeController : ControllerBase
{
    private readonly MotorbikeService _motorbikeService;

    public MotorbikeController(MotorbikeService motorbikeService)
    {
        _motorbikeService = motorbikeService;
    }

    [HttpPost]
    public IActionResult AddMotorbike([FromBody] Motorbike motorbike)
    {
        if (_motorbikeService.AddMotorbike(motorbike))
        {
            return Ok("Motorbike added successfully.");
        }

        return BadRequest("Failed to add motorbike.");
    }

    [HttpGet("{motorbikeId}")]
    public IActionResult GetMotorbikeById(int motorbikeId)
    {
        var motorbike = _motorbikeService.GetMotorbikeById(motorbikeId);
        if (motorbike == null)
        {
            return NotFound("Motorbike not found.");
        }

        return Ok(motorbike);
    }

    [HttpPut]
    public IActionResult UpdateMotorbike([FromBody] Motorbike motorbike)
    {
        if (_motorbikeService.UpdateMotorbike(motorbike))
        {
            return Ok("Motorbike updated successfully.");
        }

        return BadRequest("Failed to update motorbike.");
    }

    [HttpDelete("{motorbikeId}")]
    public IActionResult DeleteMotorbike(int motorbikeId)
    {
        if (_motorbikeService.DeleteMotorbike(motorbikeId))
        {
            return Ok("Motorbike deleted successfully.");
        }

        return NotFound("Motorbike not found.");
    }

    [HttpGet]
    public IActionResult GetAllMotorbikes()
    {
        return Ok(_motorbikeService.GetAllMotorbikes());
    }
}
