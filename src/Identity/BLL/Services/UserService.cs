using AutoMapper;
using BLL.DTO;
using BLL.Exceptions;
using BLL.IService;
using DAL.Entities;
using DAL.IRepositories;
using DAL.IService;

namespace BLL.Services;

public class UserService : IUserService
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IJwtProvider _jwtService;
    
    public UserService(
        IPasswordHasher passwordHasher, 
        IMapper mapper,
        IUnitOfWork unitOfWork, IJwtProvider jwtService)
    {
        _passwordHasher = passwordHasher;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
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
        
        
        
        return (RefreshToken,response);
    }
}