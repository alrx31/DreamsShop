using Domain.Model;

namespace Domain.IServices;

public interface ICookieService
{
    void SetCookie(CookieModel model);
    
    CookieModel? GetCookie(string key);
    
    void DeleteCookie(string key);
    
    void UpdateCookie(CookieModel model);
}