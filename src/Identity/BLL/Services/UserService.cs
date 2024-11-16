using AutoMapper;
using BLL.DTO;
using BLL.Exceptions;
using BLL.IService;
using DAL.Entities;
using DAL.IRepositories;

namespace BLL.Services;

public class UserService : IUserService
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public UserService(
        IPasswordHasher passwordHasher, 
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _passwordHasher = passwordHasher;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
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
}