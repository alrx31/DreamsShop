using BLL.DTO;
using BLL.IService;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController:ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserService _userService;
    
    public UserController(IUserService userService, IHttpContextAccessor httpContextAccessor)
    {
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
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
        var (token, response) = await _userService.LoginUser(model);

        SetRefreshTokenCookie(token);
        
        return Ok(response);
    }
    
    
    private void SetRefreshTokenCookie(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Expires = DateTime.UtcNow.AddDays(7)
        };

        _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }

    private void ClearRefreshTokenCookie()
    {
        _httpContextAccessor.HttpContext.Response.Cookies.Delete("refreshToken");
    }
}