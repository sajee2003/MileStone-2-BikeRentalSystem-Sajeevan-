// File: Controllers/RentalRequestController.cs
using BikeRentalManagement.Models;
using BikeRentalManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class RentalRequestController : ControllerBase
{
    private readonly RentalRequestService _requestService;

    public RentalRequestController(RentalRequestService requestService)
    {
        _requestService = requestService;
    }

    [HttpPost]
    public IActionResult AddRentalRequest([FromBody] RentalRequest request)
    {
        if (_requestService.AddRentalRequest(request))
        {
            return Ok("Rental request submitted successfully.");
        }

        return BadRequest("Failed to submit rental request.");
    }

    [HttpPut("{requestId}/status")]
    public IActionResult UpdateRentalRequestStatus(int requestId, [FromBody] UpdateStatusRequest statusRequest)
    {
        if (_requestService.UpdateRentalRequestStatus(requestId, statusRequest.Status, statusRequest.ApprovalDate))
        {
            return Ok("Rental request status updated.");
        }

        return BadRequest("Failed to update rental request status.");
    }

    [HttpGet("{requestId}")]
    public IActionResult GetRentalRequestById(int requestId)
    {
        var request = _requestService.GetRentalRequestById(requestId);
        if (request == null)
        {
            return NotFound("Rental request not found.");
        }

        return Ok(request);
    }

    [HttpGet]
    public IActionResult GetAllRentalRequests()
    {
        return Ok(_requestService.GetAllRentalRequests());
    }
}
