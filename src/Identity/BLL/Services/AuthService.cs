using AutoMapper;
using BLL.DTO;
using BLL.Exceptions;
using BLL.IService;
using DAL.Entities;
using DAL.IRepositories;
using DAL.IService;
using Microsoft.AspNetCore.Http;

namespace BLL.Services;

public class AuthService : IAuthService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IJwtProvider _jwtService;
    
    public AuthService(
        IPasswordHasher passwordHasher, 
        IMapper mapper,
        IUnitOfWork unitOfWork, IJwtProvider jwtService, IHttpContextAccessor httpContextAccessor)
    {
        _passwordHasher = passwordHasher;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task RegisterUser(RegisterUserDTO model)
    {
        var ExistUser = await _unitOfWork.UserRepository.GetUser(model.Email);

        if (ExistUser is not null)
        {
            throw new AlreadyExistsException("This Email is not avaible");
        }
        
        var hashedPassword = _passwordHasher.Generate(model.Password);
        
        var user = _mapper.Map<User>(model);
        
        user.Password = hashedPassword;
        
        await _unitOfWork.UserRepository.AddUser(user);
        
        var RefreshTokenModel = new RefreshTokenModel
        {
            UserId = user.Id,
            Token = _jwtService.GenerateRefreshToken(),
            ExpiryTime = DateTime.UtcNow.AddDays(7)
        };
        // TODO: Add RefreshTokenModel to database
        await _unitOfWork.CompleteAsync();
    }

    public async Task<(string,LoginResponseDTO)> LoginUser(LoginUserDTO model)
    {
        var response = new LoginResponseDTO();
        var user = await _unitOfWork.UserRepository.GetUser(model.Email);

        if (
            user is null ||
            !_passwordHasher.Verify(model.Password, user.Password)
            )
        {
            throw new ValidationDataException("Invalid login or password.");
        }
        
        response.isLoggedIn = true;
        response.User = _mapper.Map<ResponseUser>(user);
        response.JwtToken = _jwtService.GenerateJwtToken(user);
        var RefreshToken = _jwtService.GenerateRefreshToken();
        
        // TODO: Add RefreshToken to database
        
        return (RefreshToken,response);
    }

    public async Task<(string, LoginResponseDTO)> RefreshToken(RefreshTokenDTO model)
    {
        // TODO: Implement RefreshToken method
        throw new NotImplementedException();
    }
}