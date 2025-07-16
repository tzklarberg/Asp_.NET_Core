
using CoreProject.interfaces;
using CoreProject.Models;
using CoreProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
namespace CoreProject.Controllers;
using Serilog;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private IGenericService<User> userService;
    private IGenericService<Shoes> shoesService;
    private ActiveUser activeUser;
    private readonly ILogger<UserController> _logger;

    public UserController(IGenericService<User> userService, IGenericService<Shoes> shoesService, ActiveUser au, ILogger<UserController> logger)
    {
        _logger = logger;
        _logger.LogInformation("start userController Constructor");
        this.userService = userService;
        this.shoesService = shoesService;
        this.activeUser = au;
        _logger.LogInformation("end userController Constructor");
    }

    [HttpGet("{id}")]
    public ActionResult<User> Get(int id)
    {
        _logger.LogInformation("start userController Get{"+id+"}");
        if (!Request.Headers.TryGetValue("Authorization", out var token))
        {
            _logger.LogInformation("in userController Get{"+id+"} - Unauthorized");
            return Unauthorized();
        }
        var tokenValue = token.ToString().Replace("Bearer ", "");

        var id2 = activeUser.GetActiveUser(tokenValue).Id;
        User user = userService.Get(id2);
        if (user == null){
            _logger.LogInformation("in userController Get{"+id+"} - NotFound user");
            return NotFound();
        }
        _logger.LogInformation("end userController Get{"+id+"}");
        return user;
    }

    [HttpGet()]
    [Authorize(Policy = "ADMIN")]
    public ActionResult<IEnumerable<User>> Get()
    {
        _logger.LogInformation("start userController Get");
        List<User> list = userService.Get();
        _logger.LogInformation("end userController Get");
        return list;
    }

    [HttpPost()]
    [Authorize(Policy = "ADMIN")]
    public ActionResult Post(User user)
    {
        _logger.LogInformation("start userController Post");
        int result = userService.Add(user);
        if (result == -1)
        {
            _logger.LogInformation("in userController Post - absent user name or user password");
            return BadRequest();
        }
        _logger.LogInformation("end userController Post");
        return CreatedAtAction(nameof(Post), new { Id = result });
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, User user)
    {
        bool result = userService.Update(id,user);
        if(result){
            return NoContent();
        }
        return BadRequest();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "ADMIN")]
    public ActionResult Delete(int id)
    {
        _logger.LogInformation("id");//what between _logger to Log?
        _logger.LogInformation("start userController Delete");

        // var item = Get(id);
        // if (item == null)
        // {
        //     return NotFound();
        // }
        // var user = item.Value as User;
        // string name = user.Name;
        var shoesResult = shoesService.Get();
        List<Shoes> shoes = shoesResult as List<Shoes>;

        shoes.RemoveAll(shoe => shoe.UserId == id && shoesService.Delete(shoe.Id));
        bool result = userService.Delete(id);
        if (result)
        {
            _logger.LogInformation("end userController Delete");
            return Ok();
        }
        _logger.LogInformation("in userController Delete - Notfound");
        return NotFound();
    }


}

