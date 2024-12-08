using BLL.DTO;
using BLL.Exceptions;
using BLL.IService;
using Bogus;
using DAL.Entities;
using DAL.IRepositories;
using FluentAssertions;
using Moq;

namespace Tests.UnitTests.Service.UserService;

public class DeleteUserTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IProducerRepository> _producerRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    
    private readonly IUserService _userService;
    
    public DeleteUserTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _producerRepositoryMock = new Mock<IProducerRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        
        
        _unitOfWorkMock.Setup(u => u.UserRepository).Returns(_userRepositoryMock.Object);
        _unitOfWorkMock.Setup(u => u.ProducerRepository).Returns(_producerRepositoryMock.Object);
        
        _userService = new BLL.Services.UserService(
            _unitOfWorkMock.Object,
            _passwordHasherMock.Object
        );
    }
    
    [Fact]
    public async Task DeleteUser_Success_WhenRequesterAdmin()
    {
        // Arrange
        var faker = new Faker();
        var userAdmin = new User
        {
            Id = Guid.NewGuid(),
            Role = Roles.ADMIN,
            Password = faker.Internet.Password(),
            Email = faker.Internet.Email(),
        };
        
        var userToDelete = new User
        {
            Id = Guid.NewGuid(),
            Password = faker.Internet.Password(),
            Email = faker.Internet.Email(),
        };
        
        var deleteUserDto = new DeleteUserDTO
        {
            userId = userToDelete.Id,
            requestorId = userAdmin.Id,
            password = userAdmin.Password
        };
        
        _userRepositoryMock.Setup(u => u.GetUser(userToDelete.Id)).ReturnsAsync(userToDelete);

        _userRepositoryMock.Setup(u => u.GetUser(userAdmin.Id)).ReturnsAsync(userAdmin);
        
        _passwordHasherMock.Setup(p => p.Verify(userAdmin.Password, userAdmin.Password)).Returns(true);
        
        // Act
        await _userService.DeleteUser(deleteUserDto);
        
        // Assert
        _userRepositoryMock.Verify(u => u.DeleteUser(userToDelete), Times.Once);
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
    }
    
    [Fact]
    public async Task DeleteUser_Success_WhenRequesterSameUser()
    {
        // Arrange
        
        var faker = new Faker();
        var user = new User
        {
            Id = Guid.NewGuid(),
            Role = Roles.ADMIN,
            Password = faker.Internet.Password(),
            Email = faker.Internet.Email(),
        };
        
        var deleteUserDto = new DeleteUserDTO
        {
            userId = user.Id,
            requestorId = user.Id,
            password = user.Password
        };
        
        _userRepositoryMock.Setup(u => u.GetUser(user.Id)).ReturnsAsync(user);
        
        _passwordHasherMock.Setup(p => p.Verify(user.Password, user.Password)).Returns(true);
        
        // Act
        await _userService.DeleteUser(deleteUserDto);
        
        // Assert
        
        _userRepositoryMock.Verify(u => u.DeleteUser(user), Times.Once);
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteUser_Success_WhenRequesterAdminInUserCompany()
    {
        // Arrange
        var faker = new Faker();
        var userAdmin = new User
        {
            Id = Guid.NewGuid(),
            Role = Roles.PROVIDER_ADMIN,
            Password = faker.Internet.Password(),
            Email = faker.Internet.Email(),
        };
        
        var userToDelete = new User
        {
            Id = Guid.NewGuid(),
            Password = faker.Internet.Password(),
            Email = faker.Internet.Email(),
        };
        
        var producer = new Producer
        {
            Id = Guid.NewGuid(),
            Staff = new List<User>
            {
                userAdmin,
                userToDelete
            }
        };
        
        var deleteUserDto = new DeleteUserDTO
        {
            userId = userToDelete.Id,
            requestorId = userAdmin.Id,
            password = userAdmin.Password
        };
        
        _userRepositoryMock.Setup(u => u.GetUser(userToDelete.Id)).ReturnsAsync(userToDelete);
        
        _userRepositoryMock.Setup(u => u.GetUser(userAdmin.Id)).ReturnsAsync(userAdmin);
        
        _producerRepositoryMock.Setup(p => p.GetProducerByAdmin(userAdmin.Id)).ReturnsAsync(producer);
        
        _passwordHasherMock.Setup(p => p.Verify(userAdmin.Password, userAdmin.Password)).Returns(true);
        
        // Act
        
        await _userService.DeleteUser(deleteUserDto);
        
        // Assert
        
        _userRepositoryMock.Verify(u => u.DeleteUser(userToDelete), Times.Once);
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteUser_Fail_WhenRequesterHaveNotAccess()
    {
        // Arrange
        var faker = new Faker();
        var userAdmin = new User
        {
            Id = Guid.NewGuid(),
            Role = Roles.CONSUMER,
            Password = faker.Internet.Password(),
            Email = faker.Internet.Email(),
        };
        
        var userToDelete = new User
        {
            Id = Guid.NewGuid(),
            Password = faker.Internet.Password(),
            Email = faker.Internet.Email(),
        };
        
        var deleteUserDto = new DeleteUserDTO
        {
            userId = userToDelete.Id,
            requestorId = userAdmin.Id,
            password = userAdmin.Password
        };
        
        _userRepositoryMock.Setup(u => u.GetUser(userToDelete.Id)).ReturnsAsync(userToDelete);
        
        _userRepositoryMock.Setup(u => u.GetUser(userAdmin.Id)).ReturnsAsync(userAdmin);
        
        // Act
        
        Func<Task> action = async () => await _userService.DeleteUser(deleteUserDto);
        
        // Assert
        
        await action.Should().ThrowAsync<ForbiddenException>().WithMessage("You are not allowed to delete this user");
    }
    
    [Fact]
    public async Task DeleteUser_Fail_WhenPasswordIsIncorrect()
    {
        // Arrange
        var faker = new Faker();
        var userAdmin = new User
        {
            Id = Guid.NewGuid(),
            Role = Roles.ADMIN,
            Password = faker.Internet.Password(),
            Email = faker.Internet.Email(),
        };
        
        var userToDelete = new User
        {
            Id = Guid.NewGuid(),
            Password = faker.Internet.Password(),
            Email = faker.Internet.Email(),
        };
        
        var deleteUserDto = new DeleteUserDTO
        {
            userId = userToDelete.Id,
            requestorId = userAdmin.Id,
            password = userAdmin.Password
        };
        
        _userRepositoryMock.Setup(u => u.GetUser(userToDelete.Id)).ReturnsAsync(userToDelete);
        
        _userRepositoryMock.Setup(u => u.GetUser(userAdmin.Id)).ReturnsAsync(userAdmin);
        
        _passwordHasherMock.Setup(p => p.Verify(userAdmin.Password, userAdmin.Password)).Returns(false);
        
        // Act
        
        Func<Task> action = async () => await _userService.DeleteUser(deleteUserDto);
        
        // Assert
        
        await action.Should().ThrowAsync<ForbiddenException>().WithMessage("Password is incorrect");
    }

    [Fact]
    public async Task DeleteUser_Fail_WhenUserNotExist()
    {
        // Arrange
        var faker = new Faker();
        var userAdmin = new User
        {
            Id = Guid.NewGuid(),
            Role = Roles.ADMIN,
            Password = faker.Internet.Password(),
            Email = faker.Internet.Email(),
        };
        
        var deleteUserDto = new DeleteUserDTO
        {
            userId = Guid.NewGuid(),
            requestorId = userAdmin.Id,
            password = userAdmin.Password
        };
        
        _userRepositoryMock.Setup(u => u.GetUser(userAdmin.Id)).ReturnsAsync(userAdmin);
        
        _passwordHasherMock.Setup(p => p.Verify(userAdmin.Password, userAdmin.Password)).Returns(true);
        
        // Act
        
        Func<Task> action = async () => await _userService.DeleteUser(deleteUserDto);
        
        // Assert
        
        await action.Should().ThrowAsync<NotFoundException>().WithMessage("User not found");
    }
}