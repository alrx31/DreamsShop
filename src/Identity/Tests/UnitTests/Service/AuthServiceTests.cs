using AutoMapper;
using BLL.DTO;
using BLL.Exceptions;
using BLL.IService;
using BLL.Services;
using Bogus;
using DAL.Entities;
using DAL.IRepositories;
using DAL.IService;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Tests.UnitTests.Service;

public class AuthServiceTests
{
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IJwtProvider> _jwtServiceMock;
    
    private readonly IAuthService _authService;

    public AuthServiceTests(){
        
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _jwtServiceMock = new Mock<IJwtProvider>();
        
        _unitOfWorkMock.SetupGet(x => x.UserRepository).Returns(_userRepositoryMock.Object);
        
        _mapperMock = new Mock<IMapper>();
        
        _authService = new AuthService(
            _passwordHasherMock.Object, 
            _mapperMock.Object,
            _unitOfWorkMock.Object,
            _jwtServiceMock.Object,
            _httpContextAccessorMock.Object
            );
    }

    [Fact]
    public async Task RegisterUser_Fail_WhenEmailIsNotAvaible()
    {
        var faker = new Faker();

        var RegisterDTO = new RegisterUserDTO
        {
            Email = faker.Person.Email,
            Name = faker.Internet.UserName(),
            Password = faker.Internet.Password()
        };
        
        _userRepositoryMock.Setup(x => x.GetUser(RegisterDTO.Email)).ReturnsAsync(new User());
        
        // Act
        
        Func<Task> act = async () => await _authService.RegisterUser(RegisterDTO);
        
        // Assert

        await act.Should().ThrowAsync<AlreadyExistsException>();
    }
    
    [Fact]
    public async Task RegisterUser_Success_ShouldAddUserToDb()
    {
        var faker = new Faker();

        var RegisterDTO = new RegisterUserDTO
        {
            Email = faker.Person.Email,
            Name = faker.Internet.UserName(),
            Password = faker.Internet.Password()
        };
        
        _userRepositoryMock.Setup(x => x.GetUser(RegisterDTO.Email)).ReturnsAsync((User)null);
        
        _passwordHasherMock.Setup(x => x.Generate(RegisterDTO.Password)).Returns(faker.Internet.Password());
        
        _mapperMock.Setup(x => x.Map<User>(RegisterDTO)).Returns(new User());
        
        // Act
        
        await _authService.RegisterUser(RegisterDTO);
        
        // Assert

        _userRepositoryMock.Verify(x => x.AddUser(It.IsAny<User>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CompleteAsync(), Times.Once);
    }
    
    // TODO: Add Tests for LoginUser
    // TODO: Add Tests for RefreshToken methods    
    // TODO: Add Tests for Logout methods    

}