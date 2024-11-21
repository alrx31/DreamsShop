using BLL.DTO;
using BLL.IService;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController:ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAuthService _authService;
    
    public AuthController(IAuthService authService, IHttpContextAccessor httpContextAccessor)
    {
        _authService = authService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPut]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDTO model)
    {
        await _authService.RegisterUser(model);
        
        return Ok();
    }
    
    
    [HttpPost]
    public async Task<IActionResult> LoginUser([FromBody] LoginUserDTO model)
    {
        var (token, response) = await _authService.LoginUser(model);

        SetRefreshTokenCookie(token);
        
        return Ok(response);
    }
    
    [HttpPatch]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO model)
    {
        var (token, response) = await _authService.RefreshToken(model);

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