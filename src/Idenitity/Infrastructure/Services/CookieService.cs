using Domain.IServices;
using Domain.Model;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class CookieService
    (
        IHttpContextAccessor contextAccessor
    ): ICookieService
{
    private readonly CookieOptions cookieOptions = new()
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict
    };

    public void SetCookie(string key, string value)
    {
        contextAccessor.HttpContext?.Response.Cookies.Append(key, value, cookieOptions);
    }

    public string? GetCookie(string key)
    {
        return contextAccessor.HttpContext?.Request.Cookies[key];
    }

    public void DeleteCookie(string key)
    {
        contextAccessor.HttpContext?.Response.Cookies.Delete(key);
    }
}