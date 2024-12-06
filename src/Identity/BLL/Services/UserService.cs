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
        
        var requester = await unitOfWork.UserRepository.GetUser(deleteUserDto.requestorId);

        var producer = await unitOfWork.ProducerRepository.GetProducerByAdmin(requester!.Id);

        if (
            requester.Role == Roles.ADMIN ||
            requester.Id == deleteUserDto.userId ||
            (requester.Role == Roles.PROVIDER_ADMIN && producer!.Staff.Contains(user) && producer.Staff.Contains(requester))
            )
        {
            if (!passwordHasher.Verify(deleteUserDto.password,requester.Password))
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
        var requester = await unitOfWork.UserRepository.GetUser(updateUserDto.RequestorId);

        if (requester is null)
        {
            throw new ForbiddenException("Invalid Requester.");
        }

        if (!passwordHasher.Verify(updateUserDto.Password, requester.Password))
        {
            throw new ForbiddenException("Invalid password.");
        }
        
        var userToUpdate = await unitOfWork.UserRepository.GetUser(updateUserDto.UserDTO.Email);

        if (userToUpdate is null)
        {
            throw new NotFoundException("Update user not found.");
        }

        if (
            userToUpdate.Id != requester.Id &&
            requester.Role != Roles.ADMIN &&
            (userToUpdate.Role != Roles.PROVIDER ||
             userToUpdate.ProducerId != requester.ProducerId ||
             requester.Role != Roles.PROVIDER_ADMIN
             )
            )
        {
            throw new ForbiddenException("You do not have enough rights.");
        }

        userToUpdate.Email = updateUserDto.UserDTO.Email;
        userToUpdate.Name = updateUserDto.UserDTO.Name;
        userToUpdate.Password = passwordHasher.Generate(updateUserDto.UserDTO.Password);

        await unitOfWork.UserRepository.UpdateUser(userToUpdate);

        await unitOfWork.CompleteAsync();
    }

    public Task ChangeUserRole(ChangeUserRoleDTO updateModel)
    {
        throw new NotImplementedException("change user role");
    }
}