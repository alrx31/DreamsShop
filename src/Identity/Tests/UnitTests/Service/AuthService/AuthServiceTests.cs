using AutoMapper;
using BLL.DTO;
using BLL.Exceptions;
using BLL.IService;
using Bogus;
using DAL.Entities;
using DAL.IRepositories;
using DAL.IService;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Tests.UnitTests.Service.AuthService;

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
        
        _authService = new BLL.Services.AuthService(
            _passwordHasherMock.Object, 
            _mapperMock.Object,
            _unitOfWorkMock.Object,
            _jwtServiceMock.Object,
            _httpContextAccessorMock.Object
            );
    }
    
    // TODO: Add Tests for LoginUser
    // TODO: Add Tests for RefreshToken methods    
    // TODO: Add Tests for Logout methods   

}