// File: Controllers/UserController.cs
using BikeRentalManagement.Models;
using BikeRentalManagement.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] BikeRentalManagement.Models.LoginRequest request)
    {
        var user = _userService.Login(request.Username,request.Password,request.Role);
        if (user == null)
        {
            return Unauthorized("Invalid username, password, or role.");
        }

        return Ok(user);
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] User user)
    {
        if (_userService.Register(user))
        {
            return Ok("User registered successfully.");
        }

        return BadRequest("User registration failed. Username may already exist.");
    }

    [HttpGet("{userId}")]
    public IActionResult GetUserById(int userId)
    {
        var user = _userService.GetUserById(userId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        return Ok(user);
    }

    [HttpPut]
    public IActionResult UpdateUser([FromBody] User user)
    {
        if (_userService.UpdateUser(user))
        {
            return Ok("User updated successfully.");
        }

        return BadRequest("User update failed.");
    }

    [HttpDelete("{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        if (_userService.DeleteUser(userId))
        {
            return Ok("User deleted successfully.");
        }

        return NotFound("User not found.");
    }

    [HttpGet]
    public IActionResult GetAllUsers()
    {
        return Ok(_userService.GetAllUsers());
    }
}
