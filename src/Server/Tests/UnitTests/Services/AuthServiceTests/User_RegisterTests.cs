using Application.DTO;
using Application.UseCases.Commands;
using Application.UseCases.CommandsHandlers;
using AutoMapper;
using Bogus;
using Domain.Entity;
using Domain.IRepositories;
using Domain.IRepositories.Base;
using Moq;

namespace Tests.UnitTests.Services.AuthServiceTests;

public class User_RegisterTests : BaseServiceTest
{
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IConsumerUserRepository> _consumerUserRepository;
    
    private readonly ConsumerUserRegisterCommandHandler _handler;

    public User_RegisterTests()
    {
        _mapper = new Mock<IMapper>();
        _consumerUserRepository = new Mock<IConsumerUserRepository>();
        
        _unitOfWork = new Mock<IUnitOfWork>();

        _unitOfWork.Setup(x=>x.ConsumerUserRepository).Returns(_consumerUserRepository.Object);
        
        _handler = new ConsumerUserRegisterCommandHandler(_mapper.Object,_unitOfWork.Object);
    }
    
    [Fact]
    public async Task RegisterAsync_ShouldRegister_WhenDataIsValid()
    {
        // Arrange
        var faker = new Faker();
        var password = faker.Internet.Password();
        var registerUserCommand = new ConsumerUserRegisterCommand(new ConsumerUserRegisterDto
        {
            Email = faker.Person.Email,
            Password = password,
            PasswordRepeat = password,
            Name = faker.Person.FullName,
            Role = Roles.CONSUMER
        });
        
        //_unitOfWork.Setup(x=>x.ConsumerUserRepository.Get)

    }
}