using Application.DTO;
using Application.UseCases.Commands;
using Application.UseCases.CommandsHandlers;
using AutoMapper;
using Bogus;
using Domain.Entity;
using Domain.IRepositories;
using Domain.IRepositories.Base;
using Domain.IServices;
using FluentAssertions;
using Moq;

namespace Tests.UnitTests.Services.AuthServiceTests;

public class ConsumerUser_RegisterTests : BaseServiceTest<ConsumerUserRegisterCommand>
{
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IConsumerUserRepository> _consumerUserRepository;
    private readonly Mock<IPasswordManager> _passwordChecker;

    public ConsumerUser_RegisterTests()
    {
        _mapper = new Mock<IMapper>();
        _consumerUserRepository = new Mock<IConsumerUserRepository>();
        _passwordChecker = new Mock<IPasswordManager>();
        
        _unitOfWork = new Mock<IUnitOfWork>();

        _unitOfWork.Setup(x=>x.ConsumerUserRepository).Returns(_consumerUserRepository.Object);
        
        _handler = new ConsumerUserRegisterCommandHandler(
            _mapper.Object,
            _unitOfWork.Object,
            _passwordChecker.Object
            );
    }
    
    [Fact]
    public async Task RegisterAsync_ShouldRegister_WhenDataIsValid()
    {
        // Arrange
        var faker = new Faker();
        var password = faker.Internet.Password();
        var passwordLevel = faker.Random.Byte(200);
        
        var registerUserCommand = new ConsumerUserRegisterCommand(new ConsumerUserRegisterDto
        {
            Email = faker.Person.Email,
            Password = password,
            PasswordRepeat = password,
            Name = faker.Person.FullName,
            Role = Roles.CONSUMER
        });

        var userModel = new ConsumerUser
        {
            Id = default,
            Email = null,
            Password = null,
            Name = null,
            Role = Roles.ADMIN
        };
        

        _unitOfWork.Setup(x => x.ConsumerUserRepository.GetByEmailAsync(registerUserCommand.Model.Email, CancellationToken.None))
            .ReturnsAsync((null) as ConsumerUser);

        _passwordChecker.Setup(x => x.CheckPassword(password))
            .Returns(passwordLevel);
        
        // Act
        await _handler.Handle(registerUserCommand, CancellationToken.None);

        // Assert
        _passwordChecker.Verify(x=>x.CheckPassword(password), Times.Once);
        _consumerUserRepository.Verify(x=>x.GetByEmailAsync(registerUserCommand.Model.Email,CancellationToken.None),Times.Once);
        _consumerUserRepository.Verify(x=>x.AddAsync(It.IsAny<ConsumerUser>(),CancellationToken.None),Times.Once);
    }
    
}