using BLL.DTO;
using BLL.Exceptions;
using BLL.IService;
using Bogus;
using DAL.Entities;
using DAL.IRepositories;
using FluentAssertions;
using Moq;

namespace Tests.UnitTests.Service.UserService;

public class UpdateUserTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IProducerRepository> _producerRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    
    private readonly IUserService _userService;
    
    public UpdateUserTests()
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
    public async Task UpdateUser_Success_WhenRequesterUpdateSelf()
    {
        // Arrange
        var faker = new Faker();
        
        var exitstUser = new User
        {
            Id = faker.Random.Guid(),
            Email = faker.Internet.Email(),
            Password = faker.Internet.Password(),
            Name = faker.Internet.UserName(),
            Role = Roles.CONSUMER
        };

        var oldPassword = exitstUser.Password;
        
        var updateDTO = new UpdateUserDTO
        {
            Password = exitstUser.Password,
            RequestorId = exitstUser.Id,
            UserDTO = new RegisterUserDTO
            {
                Email = faker.Internet.Email(),
                Password = faker.Internet.Password(),
                Name = faker.Internet.UserName()
            }
        };
        
        _userRepositoryMock.Setup(x => x.GetUser(updateDTO.RequestorId)).ReturnsAsync(exitstUser);
    
        _passwordHasherMock.Setup(x => x.Verify(updateDTO.Password,It.IsIn(exitstUser.Password,oldPassword))).Returns(true);

        _userRepositoryMock.Setup(x => x.GetUser(exitstUser.Id)).ReturnsAsync(exitstUser);

        _passwordHasherMock.Setup(x => x.Generate(updateDTO.UserDTO.Password)).Returns(updateDTO.UserDTO.Password);

        _userRepositoryMock.Setup(x => x.UpdateUser(exitstUser)).Returns(Task.CompletedTask);
        
        
        // Act

        await _userService.UpdateUser(updateDTO,exitstUser.Id);
        
        // Assert
        _passwordHasherMock.Verify(x => x.Verify(updateDTO.Password,oldPassword),Times.Once);
        
        _userRepositoryMock.Verify(x=>x.GetUser(updateDTO.RequestorId),Times.AtLeastOnce);
        _userRepositoryMock.Verify(x => x.GetUser(exitstUser.Id),Times.AtLeastOnce);
        _passwordHasherMock.Verify(x => x.Generate(updateDTO.UserDTO.Password),Times.Once);
        _userRepositoryMock.Verify(x => x.UpdateUser(exitstUser), Times.Once);
    }

    [Fact]
    public async Task UpdateUser_Fail_WhenRequesterNotFould()
    {
        // Arrange
        var faker = new Faker();
        
        var updateDTO = new UpdateUserDTO
        {
            Password = faker.Internet.Password(),
            RequestorId = faker.Random.Guid(),
            UserDTO = new RegisterUserDTO
            {
                Email = faker.Internet.Email(),
                Password = faker.Internet.Password(),
                Name = faker.Internet.UserName()
            }
        };
        
        // Act
        
        Func<Task> act = async () => await _userService.UpdateUser(updateDTO, new Guid());
        
        // Assert

        await act.Should().ThrowAsync<ForbiddenException>()
            .WithMessage("Invalid Requester.");
    }

    [Fact]
    public async Task UpdateUser_Fail_WhenInvalidPassword()
    {
        var faker = new Faker();
        
        var exitstUser = new User
        {
            Id = faker.Random.Guid(),
            Email = faker.Internet.Email(),
            Password = faker.Internet.Password(),
            Name = faker.Internet.UserName(),
            Role = Roles.CONSUMER
        };
        
        var updateDTO = new UpdateUserDTO
        {
            Password = exitstUser.Password,
            RequestorId = exitstUser.Id,
            UserDTO = new RegisterUserDTO
            {
                Email = faker.Internet.Email(),
                Password = faker.Internet.Password(),
                Name = faker.Internet.UserName()
            }
        };
        
        _userRepositoryMock.Setup(x => x.GetUser(updateDTO.RequestorId)).ReturnsAsync(exitstUser);
        
        _passwordHasherMock.Setup(x => x.Verify(updateDTO.Password,exitstUser.Password)).Returns(false);
        
        // Act
        
        Func<Task> act = async () => await _userService.UpdateUser(updateDTO, new Guid());
        
        // Assert
        
        await act.Should().ThrowAsync<ForbiddenException>()
            .WithMessage("Invalid password.");
    }
    
    [Fact]
    public async Task UpdateUser_Fail_WhenUserNotExist()
    {
        // Arrange
        var faker = new Faker();
        
        var exitstUser = new User
        {
            Id = faker.Random.Guid(),
            Email = faker.Internet.Email(),
            Password = faker.Internet.Password(),
            Name = faker.Internet.UserName(),
            Role = Roles.CONSUMER
        };
        
        var updateDTO = new UpdateUserDTO
        {
            Password = exitstUser.Password,
            RequestorId = exitstUser.Id,
            UserDTO = new RegisterUserDTO
            {
                Email = faker.Internet.Email(),
                Password = faker.Internet.Password(),
                Name = faker.Internet.UserName()
            }
        };
        
        _userRepositoryMock.Setup(x => x.GetUser(updateDTO.RequestorId)).ReturnsAsync(exitstUser);
        
        _passwordHasherMock.Setup(x => x.Verify(updateDTO.Password,exitstUser.Password)).Returns(true);
        
        // Act
        
        Func<Task> act = async () => await _userService.UpdateUser(updateDTO, new Guid());
        
        // Assert

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Update user not found.");
    }

    [Fact]
    public async Task UpdateUser_Fail_WhenRequesterNotHaveRight()
    {
        var faker = new Faker();
        
        var exitstUser = new User
        {
            Id = faker.Random.Guid(),
            Email = faker.Internet.Email(),
            Password = faker.Internet.Password(),
            Name = faker.Internet.UserName(),
            Role = Roles.CONSUMER
        };

        var requester = new User
        {
            Id = faker.Random.Guid(),
            Email = faker.Internet.Email(),
            Password = faker.Internet.Password(),
            Name = faker.Internet.UserName(),
            Role = Roles.CONSUMER
        };

        var oldPassword = exitstUser.Password;
        
        var updateDTO = new UpdateUserDTO
        {
            Password = requester.Password,
            RequestorId = requester.Id,
            UserDTO = new RegisterUserDTO
            {
                Email = faker.Internet.Email(),
                Password = faker.Internet.Password(),
                Name = faker.Internet.UserName()
            }
        };
        
        _userRepositoryMock.Setup(x => x.GetUser(updateDTO.RequestorId)).ReturnsAsync(requester);
    
        _passwordHasherMock.Setup(x => x.Verify(updateDTO.Password,It.IsIn(requester.Password,oldPassword))).Returns(true);

        _userRepositoryMock.Setup(x => x.GetUser(exitstUser.Id)).ReturnsAsync(exitstUser);
        
        // Act
        
        Func<Task> act = async () => await _userService.UpdateUser(updateDTO,exitstUser.Id);
        
        // Assert

        await act.Should().ThrowAsync<ForbiddenException>()
            .WithMessage("You do not have enough rights.");
    }
}