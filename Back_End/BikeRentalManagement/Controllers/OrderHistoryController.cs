// File: Controllers/OrderHistoryController.cs
using BikeRentalManagement.Models;
using BikeRentalManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class OrderHistoryController : ControllerBase
{
    private readonly OrderHistoryService _orderService;

    public OrderHistoryController(OrderHistoryService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public IActionResult AddOrderHistory([FromBody] OrderHistory order)
    {
        if (_orderService.AddOrderHistory(order))
        {
            return Ok("Order history entry added.");
        }

        return BadRequest("Failed to add order history entry.");
    }

    [HttpGet]
    public IActionResult GetAllOrderHistories()
    {
        return Ok(_orderService.GetAllOrderHistories());
    }
}
