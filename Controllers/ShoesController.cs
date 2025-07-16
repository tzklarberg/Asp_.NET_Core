using Microsoft.AspNetCore.Mvc;
using CoreProject.Models;
using CoreProject.interfaces;
using CoreProject.Services;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Serilog;


namespace CoreProject.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ShoesController : ControllerBase
{

    private ActiveUser activeUser;
    private IGenericService<Shoes> shoesService;
    private readonly ILogger<ShoesController> _logger;


    public ShoesController(IGenericService<Shoes> shoesService, ActiveUser au, ILogger<ShoesController> logger)
    {
        // System.Console.WriteLine("ctor controller");
        _logger = logger;
        _logger.LogInformation("start shoesController constructor");
        this.activeUser = au;
        this.shoesService = shoesService;
        _logger.LogInformation($"end shoesController constructor, user = {activeUser.Name}");
    }

    [HttpGet("{id}")]
    public ActionResult<Shoes> Get(int id)
    {
        _logger.LogInformation($"start shoesController Get{"+id+"}, user = {activeUser.Name}");
        if (!Request.Headers.TryGetValue("Authorization", out var token))
        {
            _logger.LogInformation($"in shoesController Get{"+id+"} - Unauthorized, user = {activeUser.Name}");
            return Unauthorized();
        }

        var tokenValue = token.ToString().Replace("Bearer ", "");

        var id2 = activeUser.GetActiveUser(tokenValue).Id;

        Shoes shoes = shoesService.Get(id2);
        if (shoes == null || shoes.UserId != id2)
        {
            _logger.LogInformation($"in shoesController Get{"+id+"} - NotFound shoes, user = {activeUser.Name}");
            return NotFound();
        }
        _logger.LogInformation($"end shoesController Get{"+id+"}, user = {activeUser.Name}");
        return shoes;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Shoes>> Get()
    {
        _logger.LogInformation($"start shoesController Get, user = {activeUser.Name}");
        // if (!Request.Headers.TryGetValue("Authorization", out var token))
        // {
        //     _logger.LogInformation("in shoesController Get - Unauthorized");
        //     return Unauthorized();
        // }

        // var tokenValue = token.ToString().Replace("Bearer ", "");

        // var id = activeUser.GetActiveUser(tokenValue).Id;

        var filteredShoes = shoesService.Get();//.Where(s => s.UserId == id);
        _logger.LogInformation($"end shoesController Get, user = {activeUser.Name}");
        return Ok(filteredShoes);
    }

    [HttpPost()]
    public ActionResult Post(Shoes shoes)
    {
        _logger.LogInformation($"start shoesController Post, user = {activeUser.Name}");
        //new shoes for the active user
        if (!Request.Headers.TryGetValue("Authorization", out var token))
        {
            _logger.LogInformation($"in shoesController Post - Unauthorized, user = {activeUser.Name}");
            return Unauthorized();
        }

        var tokenValue = token.ToString().Replace("Bearer ", "");
        var id = activeUser.GetActiveUser(tokenValue).Id;

        shoes.UserId = id;
        int result = shoesService.Add(shoes);
        if(result == -1)
        {
            _logger.LogInformation($"in shoesController Post - absent the shoes name, user = {activeUser.Name}");
            return BadRequest();
        }
        _logger.LogInformation($"end shoesController Post, user = {activeUser.Name}");
        return CreatedAtAction(nameof(Post), new { Id= result});
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, Shoes shoes)
    {
        _logger.LogInformation($"start shoesController Put, user = {activeUser.Name}");
        if (!Request.Headers.TryGetValue("Authorization", out var token))
        {
            _logger.LogInformation($"in shoesController Put - Unauthorized, user = {activeUser.Name}");
            return Unauthorized();
        }

        var tokenValue = token.ToString().Replace("Bearer ", "");
        var userId = activeUser.GetActiveUser(tokenValue).Id;

        shoes.UserId = userId;
        bool result = shoesService.Update(id, shoes);
        if(result){
            _logger.LogInformation($"end shoesController Put, user = {activeUser.Name}");
            return NoContent();
        }
        _logger.LogInformation($"in shoesController Put - Absent details of shoes or shoes not found, user = {activeUser.Name}");
        return BadRequest();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        _logger.LogInformation($"start shoesController Delete, user = {activeUser.Name}");
        bool result = shoesService.Delete(id);
        if(result){
            _logger.LogInformation($"end shoesController Delete, user = {activeUser.Name}");
            return Ok();
        }
        _logger.LogInformation($"in shoesController Delete - NotFound, user = {activeUser.Name}");
        return NotFound();
    } 

}
