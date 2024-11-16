using BLL.DTO;

namespace BLL.IService;

public interface IUserService
{
    Task RegisterUser(RegisterUserDTO model);
    
    Task<(string,LoginResponseDTO)> LoginUser(LoginUserDTO model);
}