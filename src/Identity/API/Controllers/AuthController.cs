using BLL.DTO;
using BLL.IService;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(
    IAuthService authService,
    IHttpContextAccessor httpContextAccessor
    )
    : ControllerBase
{
    [HttpPut]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDTO model)
    {
        await authService.RegisterUser(model);
        
        return Ok();
    }
    
    [HttpPost]
    public async Task<IActionResult> LoginUser([FromBody] LoginUserDTO model)
    {
        var (token, response) = await authService.LoginUser(model);

        SetRefreshTokenCookie(token);
        
        return Ok(response);
    }
    
    [HttpPatch]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO model)
    {
        var (token, response) = await authService.RefreshToken(model);

        SetRefreshTokenCookie(token);
        
        return Ok(response);
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> LogoutUser(Guid userId)
    {
        await authService.LogoutUser(userId);
        
        ClearRefreshTokenCookie();
        
        return Ok();
    }
    
    
    private void SetRefreshTokenCookie(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Expires = DateTime.UtcNow.AddDays(7)
        };

        httpContextAccessor.HttpContext?.Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }
    
    

    private void ClearRefreshTokenCookie()
    {
        httpContextAccessor.HttpContext?.Response.Cookies.Delete("refreshToken");
    }
}