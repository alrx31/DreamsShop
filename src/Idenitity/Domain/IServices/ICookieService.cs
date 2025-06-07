using Domain.Model;

namespace Domain.IServices;

public interface ICookieService
{
    void SetCookie(string key, string value);
    string? GetCookie(string key);
    void DeleteCookie(string key);
}