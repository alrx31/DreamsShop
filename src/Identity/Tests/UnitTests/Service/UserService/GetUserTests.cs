using BLL.Exceptions;
using BLL.IService;
using DAL.Entities;
using DAL.IRepositories;
using FluentAssertions;
using Moq;

namespace Tests.UnitTests.Service.UserService;

public class GetUserTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IProducerRepository> _producerRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    
    private readonly IUserService _userService;
    
    public GetUserTests()
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
    public async Task GetUserById_UserExists_ReturnsUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId
        };
        
        _userRepositoryMock.Setup(u => u.GetUser(userId)).ReturnsAsync(user);
        
        // Act
        var result = await _userService.GetUserById(userId);
        
        // Assert
        result.Should().BeEquivalentTo(user);
    }
    
    [Fact]
    public async Task GetUserById_UserDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        
        // Act
        Func<Task> action = async () => await _userService.GetUserById(userId);
        
        // Assert
        await action.Should().ThrowAsync<NotFoundException>();
    }
}