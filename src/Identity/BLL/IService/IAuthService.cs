using BLL.DTO;

namespace BLL.IService;

public interface IAuthService
{
    Task RegisterUser(RegisterUserDTO model);
    
    Task<(string,LoginResponseDTO)> LoginUser(LoginUserDTO model);
    
    Task <(string,LoginResponseDTO)> RefreshToken(RefreshTokenDTO model);

    Task LogoutUser(Guid userId);
}