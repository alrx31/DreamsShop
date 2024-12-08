using BLL.DTO;
using BLL.Exceptions;
using BLL.IService;
using Bogus;
using DAL.Entities;
using DAL.IRepositories;
using FluentAssertions;
using Moq;

namespace Tests.UnitTests.Service.UserService;

public class ChangeUserRoleTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IProviderRepository> _producerRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    
    private readonly IUserService _userService;
    
    public ChangeUserRoleTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _producerRepositoryMock = new Mock<IProviderRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        
        
        _unitOfWorkMock.Setup(u => u.UserRepository).Returns(_userRepositoryMock.Object);
        _unitOfWorkMock.Setup(u => u.ProviderRepository).Returns(_producerRepositoryMock.Object);
        
        _userService = new BLL.Services.UserService(
            _unitOfWorkMock.Object,
            _passwordHasherMock.Object
        );
    }

    [Fact]
    public async Task ChangeUserRole_Success_WhenRequesterAdmin()
    {
        // Arrange
        var faker = new Faker();

        var existUser = new User
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
            Role = Roles.ADMIN
        };
        
        var changeDTO = new ChangeUserRoleDTO
        {
            role = Roles.ADMIN,
            RequestorId = requester.Id,
            Password = requester.Password
        };

        _userRepositoryMock.Setup(x => x.GetUser(requester.Id)).ReturnsAsync(requester);

        _userRepositoryMock.Setup(x => x.GetUser(existUser.Id)).ReturnsAsync(existUser);

        _passwordHasherMock.Setup(x=>x.Verify(changeDTO.Password,requester.Password)).Returns(true);
        
        // Act

        await _userService.ChangeUserRole(changeDTO, existUser.Id);
        
        // Assert
        
        _userRepositoryMock.Verify(x => x.GetUser(requester.Id),Times.Once);

        _userRepositoryMock.Verify(x => x.GetUser(existUser.Id),Times.Once);

        _passwordHasherMock.Verify(x=>x.Verify(changeDTO.Password,requester.Password), Times.Once);
    }

    [Fact]
    public async Task ChangeUserRole_Success_WhenRequesterProviderAdmininSameProvider()
    {
        // Arrange
        var faker = new Faker();

        var producer = new Provider
        {
            Id = faker.Random.Guid(),
            Name = faker.Internet.UserName(),
            Description = faker.Internet.Email(),
            Raiting = faker.Random.Float(1, 10),
            Staff = new List<User>()
        };
        
        var existUser = new User
        {
            Id = faker.Random.Guid(),
            Email = faker.Internet.Email(),
            Password = faker.Internet.Password(),
            Name = faker.Internet.UserName(),
            Role = Roles.PROVIDER,
            ProducerId = producer.Id
        };

        var requester = new User
        {
            Id = faker.Random.Guid(),
            Email = faker.Internet.Email(),
            Password = faker.Internet.Password(),
            Name = faker.Internet.UserName(),
            Role = Roles.PROVIDER_ADMIN,
            ProducerId = producer.Id
        };

        
        
        var changeDTO = new ChangeUserRoleDTO
        {
            role = Roles.PROVIDER,
            RequestorId = requester.Id,
            Password = requester.Password
        };

        _userRepositoryMock.Setup(x => x.GetUser(requester.Id)).ReturnsAsync(requester);

        _userRepositoryMock.Setup(x => x.GetUser(existUser.Id)).ReturnsAsync(existUser);

        _passwordHasherMock.Setup(x => x.Verify(changeDTO.Password, requester.Password)).Returns(true);
        
        // Act

        await _userService.ChangeUserRole(changeDTO, existUser.Id);
        
        // Arrange
        
        _userRepositoryMock.Verify(x => x.GetUser(requester.Id),Times.Once);

        _userRepositoryMock.Verify(x => x.GetUser(existUser.Id),Times.Once);

        _passwordHasherMock.Verify(x=>x.Verify(changeDTO.Password,requester.Password), Times.Once);
    }

    [Fact]
    public async Task ChangeUserRole_Fail_RequesterNotExist()
    {
        // Arrange

        var changeDTO = new ChangeUserRoleDTO
        {
            Password = "",
            RequestorId = new Guid(),
            role = Roles.CONSUMER
        };
        
        // Act

        Func<Task> act = async () => await _userService.ChangeUserRole(changeDTO, new Guid());
        
        // Asesrt

        await act.Should().ThrowAsync<NotFoundException>().WithMessage("Requester not found.");
    }

    [Fact]
    public async Task ChangeUserRole_Fail_PasswordIncorrect()
    {
        // Arrange
        var faker = new Faker();

        var requester = new User
        {
            Id = faker.Random.Guid(),
            Email = faker.Internet.Email(),
            Password = faker.Internet.Password(),
            Name = faker.Internet.UserName(),
            Role = Roles.PROVIDER,
        };

        var changeDTO = new ChangeUserRoleDTO
        {
            Password = faker.Internet.Password(),
            RequestorId = requester.Id,
            role = Roles.CONSUMER
        };
        
        _userRepositoryMock.Setup(x => x.GetUser(requester.Id)).ReturnsAsync(requester);
        
        // Act
        
        Func<Task> act = async () => await _userService.ChangeUserRole(changeDTO, new Guid());
        
        // Asesrt

        await act.Should().ThrowAsync<ForbiddenException>().WithMessage("Invalid password.");
    }

    [Fact]
    public async Task ChangeUserRole_Fail_UserNotFound()
    {
        // Arrange
        var faker = new Faker();

        var requester = new User
        {
            Id = faker.Random.Guid(),
            Email = faker.Internet.Email(),
            Password = faker.Internet.Password(),
            Name = faker.Internet.UserName(),
            Role = Roles.PROVIDER,
        };

        var changeDTO = new ChangeUserRoleDTO
        {
            Password = faker.Internet.Password(),
            RequestorId = requester.Id,
            role = Roles.CONSUMER
        };
        
        _userRepositoryMock.Setup(x => x.GetUser(requester.Id)).ReturnsAsync(requester);

        _passwordHasherMock.Setup(x => x.Verify(changeDTO.Password, requester.Password)).Returns(true);
        
        // Act
        
        Func<Task> act = async () => await _userService.ChangeUserRole(changeDTO, new Guid());
        
        // Asesrt

        await act.Should().ThrowAsync<NotFoundException>().WithMessage("User not found.");
    }

    [Fact]
    public async Task ChangeUserRole_Fail_NotEnoughtRights()
    {
        // Arrange
        var faker = new Faker();

        var user = new User
        {
            Id = faker.Random.Guid(),
            Email = faker.Internet.Email(),
            Password = faker.Internet.Password(),
            Name = faker.Internet.UserName(),
            Role = Roles.PROVIDER,
        };
        
        var requester = new User
        {
            Id = faker.Random.Guid(),
            Email = faker.Internet.Email(),
            Password = faker.Internet.Password(),
            Name = faker.Internet.UserName(),
            Role = Roles.PROVIDER,
        };

        var changeDTO = new ChangeUserRoleDTO
        {
            Password = faker.Internet.Password(),
            RequestorId = requester.Id,
            role = Roles.CONSUMER
        };
        
        _userRepositoryMock.Setup(x => x.GetUser(requester.Id)).ReturnsAsync(requester);

        _passwordHasherMock.Setup(x => x.Verify(changeDTO.Password, requester.Password)).Returns(true);

        _userRepositoryMock.Setup(x => x.GetUser(user.Id)).ReturnsAsync(user);
        
        // Act
        
        Func<Task> act = async () => await _userService.ChangeUserRole(changeDTO, user.Id);
        
        // Asesrt

        await act.Should().ThrowAsync<ForbiddenException>().WithMessage("You do not have enough rights.");
    }
}