using Domain.IServices;
using Domain.Model;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class CookieService
    (
        IHttpContextAccessor contextAccessor
    ): ICookieService
{
    public void SetCookie(CookieModel model)
    {
        contextAccessor.HttpContext?.Response.Cookies.Append(model.Key, model.Value ?? string.Empty);
    }

    public CookieModel? GetCookie(string key)
    {
        return new CookieModel
        {
            Key = key,
            Value = contextAccessor.HttpContext?.Request.Cookies[key]
        };
    }

    public void DeleteCookie(string key)
    {
        contextAccessor.HttpContext?.Response.Cookies.Delete(key);
    }

    public void UpdateCookie(CookieModel model)
    {
        contextAccessor.HttpContext?.Response.Cookies.Append(model.Key, model.Value ?? string.Empty);
    }
}