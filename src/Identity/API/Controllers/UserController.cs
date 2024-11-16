using BLL.DTO;
using BLL.IService;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/{controller}")]
public class UserController:ControllerBase
{
    private readonly IUserService _userService;
    
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPut]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDTO model)
    {
        await _userService.RegisterUser(model);
        
        return Ok();
    }
    
    
    [HttpPost]
    public async Task<IActionResult> LoginUser([FromBody] LoginUserDTO model)
    {
        return Ok();
    }   
}