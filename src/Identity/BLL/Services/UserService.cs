using BLL.DTO;
using BLL.Exceptions;
using BLL.IService;
using DAL.Entities;
using DAL.IRepositories;

namespace BLL.Services;

public class UserService
    (
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher
    ) : IUserService
{
    public async Task<User?> GetUserById(Guid userId)
    {
        var user = await unitOfWork.UserRepository.GetUser(userId);

        if (user is null)
        {
            throw new NotFoundException("User not found");
        }

        return user;
    }

    public async Task DeleteUser(DeleteUserDTO deleteUserDto)
    {
        var user = await unitOfWork.UserRepository.GetUser(deleteUserDto.userId);
        
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }
        
        var requestor = await unitOfWork.UserRepository.GetUser(deleteUserDto.requestorId);

        var producer = await unitOfWork.ProducerRepository.GetProducerByAdmin(requestor!.Id);

        if (
            requestor.Role == Roles.ADMIN ||
            requestor.Id == deleteUserDto.userId ||
            (requestor.Role == Roles.PROVIDER_ADMIN && producer!.Staff.Contains(user) && producer.Staff.Contains(requestor))
            )
        {
            if (!passwordHasher.Verify(deleteUserDto.password,requestor.Password))
            {
                throw new ForbiddenException("Password is incorrect");
            }
            
            await unitOfWork.UserRepository.DeleteUser(user);
            
            await unitOfWork.CompleteAsync();
        }
        else
        {
            throw new ForbiddenException("You are not allowed to delete this user");
        }
    }

    public async Task UpdateUser(UpdateUserDTO updateUserDto)
    {
        throw new NotImplementedException();
    }
}