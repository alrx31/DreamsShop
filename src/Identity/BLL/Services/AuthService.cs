using AutoMapper;
using BLL.DTO;
using BLL.Exceptions;
using BLL.IService;
using DAL.Entities;
using DAL.IRepositories;
using DAL.IService;
using Microsoft.AspNetCore.Http;

namespace BLL.Services;

public class AuthService(
    IPasswordHasher passwordHasher,
    IMapper mapper,
    IUnitOfWork unitOfWork,
    IJwtProvider jwtService,
    IHttpContextAccessor httpContextAccessor)
    : IAuthService
{
    public async Task RegisterUser(RegisterUserDTO model)
    {
        var existUser = await unitOfWork.UserRepository.GetUser(model.Email);

        if (existUser is not null)
        {
            throw new AlreadyExistsException("This Email is not available");
        }
        
        var hashedPassword = passwordHasher.Generate(model.Password);
        
        var user = mapper.Map<User>(model);
        
        user.Password = hashedPassword;
        
        await unitOfWork.UserRepository.AddUser(user);
        
        var refreshTokenModel = new RefreshTokenModel
        {
            UserId = user.Id,
            Token = jwtService.GenerateRefreshToken(),
            ExpiryTime = DateTime.UtcNow.AddDays(7)
        };

        await unitOfWork.RefreshTokenRepository.AddRefreshToken(refreshTokenModel);
        
        await unitOfWork.CompleteAsync();
    }

    public async Task<(string,LoginResponseDTO)> LoginUser(LoginUserDTO model)
    {
        var response = new LoginResponseDTO();
        var user = await unitOfWork.UserRepository.GetUser(model.Email);

        if (
            user is null ||
            !passwordHasher.Verify(model.Password, user.Password)
            )
        {
            throw new ValidationDataException("Invalid login or password.");
        }
        
        response.isLoggedIn = true;
        response.User = mapper.Map<ResponseUser>(user);
        response.JwtToken = jwtService.GenerateJwtToken(user);
        var refreshToken = jwtService.GenerateRefreshToken();
        
        var identityUserTokenModel = await unitOfWork.RefreshTokenRepository.GetRefreshToken(user.Id);
        
        if (identityUserTokenModel is null)
        {
            await unitOfWork.RefreshTokenRepository.AddRefreshToken(new RefreshTokenModel
            {
                UserId = user.Id,
                Token = refreshToken,
                ExpiryTime = DateTime.UtcNow.AddDays(7)
            });
            
            await unitOfWork.CompleteAsync();
        }
        else
        {
            identityUserTokenModel.Token = refreshToken;
            identityUserTokenModel.ExpiryTime = DateTime.UtcNow.AddDays(7);
        }
        
        await unitOfWork.RefreshTokenRepository.UpdateRefreshToken(identityUserTokenModel!);
        
        await unitOfWork.CompleteAsync();
        
        return (refreshToken,response);
    }

    public async Task<(string, LoginResponseDTO)> RefreshToken(RefreshTokenDTO model)
    {
        var refreshToken = httpContextAccessor.HttpContext?.Request.Cookies["refreshToken"];
        
        var principal = jwtService.GetTokenPrincipal(model.JwtToken);
        var response = new LoginResponseDTO();
        
        if (principal?.Identity?.Name is null)
        {
            throw new BadRequestException("Invalid token");
        }
        
        var identityUser = await unitOfWork.RefreshTokenRepository.GetRefreshToken(principal.Identity.Name);
        
        if (
            identityUser is null ||
            string.IsNullOrEmpty(identityUser.Token) ||
            identityUser.ExpiryTime < DateTime.UtcNow ||
            identityUser.Token != refreshToken
            )
        {
            throw new BadRequestException("Invalid token");
        }

        var user = await unitOfWork.UserRepository.GetUser(identityUser.UserId);
        
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }
        
        response.isLoggedIn = true;
        
        response.User = mapper.Map<ResponseUser>(user);
        
        response.JwtToken = jwtService.GenerateJwtToken(user);
        
        refreshToken = jwtService.GenerateRefreshToken();

        var identityUserTokenModel = await unitOfWork.RefreshTokenRepository.GetRefreshToken(identityUser.UserId);

        if (identityUserTokenModel is null)
        {
            await unitOfWork.RefreshTokenRepository.AddRefreshToken(new RefreshTokenModel
            {
                UserId = identityUser.UserId,
                Token = refreshToken,
                ExpiryTime = DateTime.UtcNow.AddDays(7)
            });
            
            await unitOfWork.CompleteAsync();
        }
        else
        {
            identityUserTokenModel.Token = refreshToken;
            identityUserTokenModel.ExpiryTime = DateTime.UtcNow.AddDays(7);
        }

        await unitOfWork.RefreshTokenRepository.UpdateRefreshToken(identityUserTokenModel!);
        
        await unitOfWork.CompleteAsync();
        
        return (refreshToken, response);
    }

    public async Task LogoutUser(Guid userId)
    {
        var token = await unitOfWork.RefreshTokenRepository.GetRefreshToken(userId);

        token!.ExpiryTime = DateTime.UtcNow;

        await unitOfWork.RefreshTokenRepository.UpdateRefreshToken(token);

        await unitOfWork.CompleteAsync();
    }
}