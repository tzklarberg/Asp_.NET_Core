using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CoreProject.Models;
using CoreProject.Services;
using CoreProject.interfaces;
using Serilog;


namespace CoreProject.Controllers;
[Route("[controller]")]
public class LoginController: ControllerBase
{
    private IGenericService<User> userService;
    private readonly JsonService<User> jsonService;
    private readonly ILogger<LoginController> _logger;

    public LoginController(IGenericService<User> userService, JsonService<User> jsonService, ILogger<LoginController> logger) { 
        _logger = logger;
        _logger.LogInformation($"start LoginController Constructor");
        this.userService = userService;
        this.jsonService = jsonService;
        _logger.LogInformation($"end LoginController Constructor");
    }

    [HttpPost]
    [Route("[action]")]
    public ActionResult<String> Login([FromBody] User user)
    {
        _logger.LogInformation($"start LoginController Login, user = {user.Name}");
        var users = jsonService.GetItems();
        var currentUser = users.FirstOrDefault(u => u.Name == user.Name && u.Password == user.Password);
        
        if(currentUser == null){
            _logger.LogInformation($"in LoginController Login - Unauthorized, user = {user.Name}");
            return Unauthorized();
        }
        var claims = new List<Claim>();
        if ((user.Name == "Yaffi Altman" 
        && user.Password == "YP")
        ||(user.Name == "Tzipi Klarberg" 
        && user.Password == "TzipiPassword"))
        {
            _logger.LogInformation($"in LoginController Login - user ADMIN, user = {user.Name}");
            claims.Add(new Claim("type", "ADMIN"));
            claims.Add(new Claim("id", currentUser.Id.ToString()));
            claims.Add(new Claim("name", currentUser.Name));
        }
        else
        {
            _logger.LogInformation($"in LoginController Login - user USER, user = {user.Name}");
            claims.Add(new Claim("type", "USER"));
            claims.Add(new Claim("id", currentUser.Id.ToString()));
            claims.Add(new Claim("name", currentUser.Name));
        }
        var token = TokenService.GetToken(claims);
        // Response.Cookies.Append("token", TokenService.WriteToken(token));
        string aa = TokenService.WriteToken(token);
        _logger.LogInformation($"end LoginController Login, user = {user.Name}");
        return Ok(TokenService.WriteToken(token));
    }

    // [HttpPost]
    // [Route("[action]")]
    // [Authorize(Policy = "Admin")]
    // public IActionResult GenerateBadge([FromBody] Agent Agent)
    // {
    //     var claims = new List<Claim>
    //         {
    //             new Claim("type", "Agent")
    //         };

    //     var token = TokenService.GetToken(claims);

    //     return new OkObjectResult(TokenService.WriteToken(token));
    // }
}