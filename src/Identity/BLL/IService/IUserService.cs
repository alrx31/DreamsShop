using BLL.DTO;
using DAL.Entities;

namespace BLL.IService;

public interface IUserService
{
    Task<User?> GetUserById(Guid userId);
    Task DeleteUser(DeleteUserDTO deleteUserDto);
    Task UpdateUser(UpdateUserDTO updateUserDto);
}