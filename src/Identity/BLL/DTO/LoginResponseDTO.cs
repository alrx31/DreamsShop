using DAL.Entities;

namespace BLL.DTO;

public class LoginResponseDTO
{
    public bool isLoggedIn { get; set; } = false;
    
    public string JwtToken { get; set; }
    public ResponseUser User { get; set; }
}

public class ResponseUser
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public Roles Role { get; set; }
}