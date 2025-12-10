using Application.UseCases.ConsumerUserAuth.ConsumerUserRegister;
using Application.DTO.ConsumerUser;
using Application.Exceptions;
using AutoMapper;
using Bogus;
using Domain.Entity;
using Domain.IRepositories;
using Domain.IServices;
using FluentAssertions;
using FluentValidation;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests.UnitTests.UseCases.ConsumerUserAuth;

public class ConsumerUserRegisterCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IConsumerUserRepository> _consumerUserRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IPasswordManager> _passwordManagerMock;
    private readonly Mock<IValidator<ConsumerUserRegisterCommand>> _validatorMock;
    private readonly ConsumerUserRegisterCommandHandler _handler;

    public ConsumerUserRegisterCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _consumerUserRepositoryMock = new Mock<IConsumerUserRepository>();
        _mapperMock = new Mock<IMapper>();
        _passwordManagerMock = new Mock<IPasswordManager>();
        _validatorMock = new Mock<IValidator<ConsumerUserRegisterCommand>>();
        
        _unitOfWorkMock.Setup(u => u.ConsumerUserRepository).Returns(_consumerUserRepositoryMock.Object);
        
        _handler = new ConsumerUserRegisterCommandHandler(
            _mapperMock.Object,
            _unitOfWorkMock.Object,
            _passwordManagerMock.Object,
            _validatorMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldRegisterUser_WhenDataIsValid()
    {
        // Arrange
        var faker = new Faker();
        var password = faker.Internet.Password();
        var registerDto = new ConsumerUserRegisterDto
        {
            Email = faker.Internet.Email(),
            Password = password,
            PasswordRepeat = password,
            Name = faker.Name.FullName()
        };
        var command = new ConsumerUserRegisterCommand(registerDto);
        var validationResult = new FluentValidation.Results.ValidationResult();
        var user = new ConsumerUser
        {
            Id = faker.Random.Guid(),
            Email = registerDto.Email,
            Name = registerDto.Name,
            Password = password,
            Role = Roles.Consumer
        };

        _validatorMock.Setup(v => v.ValidateAsync(command, CancellationToken.None)).ReturnsAsync(validationResult);
        _consumerUserRepositoryMock.Setup(r => r.GetByEmailAsync(registerDto.Email, CancellationToken.None))
            .ReturnsAsync((ConsumerUser?)null);
        _passwordManagerMock.Setup(p => p.CheckPassword(registerDto.Password)).Returns(255);
        _mapperMock.Setup(m => m.Map<ConsumerUser>(registerDto, It.IsAny<Action<IMappingOperationOptions<object, ConsumerUser>>>())).Returns(user);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _consumerUserRepositoryMock.Verify(r => r.AddAsync(user, CancellationToken.None), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowDataValidationException_WhenValidationFails()
    {
        // Arrange
        var faker = new Faker();
        var password = faker.Internet.Password();
        var registerDto = new ConsumerUserRegisterDto
        {
            Email = faker.Internet.Email(),
            Password = password,
            PasswordRepeat = password,
            Name = faker.Name.FullName()
        };
        var command = new ConsumerUserRegisterCommand(registerDto);
        var validationResult = new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Email", "Email is not valid") });

        _validatorMock.Setup(v => v.ValidateAsync(command, CancellationToken.None)).ReturnsAsync(validationResult);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<DataValidationException>();
    }
    
    [Fact]
    public async Task Handle_ShouldThrowAlreadyExistException_WhenUserAlreadyExists()
    {
        // Arrange
        var faker = new Faker();
        var password = faker.Internet.Password();
        var registerDto = new ConsumerUserRegisterDto
        {
            Email = faker.Internet.Email(),
            Password = password,
            PasswordRepeat = password,
            Name = faker.Name.FullName()
        };
        var command = new ConsumerUserRegisterCommand(registerDto);
        var validationResult = new FluentValidation.Results.ValidationResult();
        var user = new ConsumerUser
        {
            Id = faker.Random.Guid(),
            Email = registerDto.Email,
            Name = registerDto.Name,
            Password = password,
            Role = Roles.Consumer
        };

        _validatorMock.Setup(v => v.ValidateAsync(command, CancellationToken.None)).ReturnsAsync(validationResult);
        _consumerUserRepositoryMock.Setup(r => r.GetByEmailAsync(registerDto.Email, CancellationToken.None)).ReturnsAsync(user);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AlreadyExistException>();
    }
    
    [Fact]
    public async Task Handle_ShouldThrowDataValidationException_WhenPasswordIsWeak()
    {
        // Arrange
        var faker = new Faker();
        var password = faker.Internet.Password();
        var registerDto = new ConsumerUserRegisterDto
        {
            Email = faker.Internet.Email(),
            Password = password,
            PasswordRepeat = password,
            Name = faker.Name.FullName()
        };
        var command = new ConsumerUserRegisterCommand(registerDto);
        var validationResult = new FluentValidation.Results.ValidationResult();

        _validatorMock.Setup(v => v.ValidateAsync(command, CancellationToken.None)).ReturnsAsync(validationResult);
        _consumerUserRepositoryMock.Setup(r => r.GetByEmailAsync(registerDto.Email, CancellationToken.None))
            .ReturnsAsync((ConsumerUser?)null);
        _passwordManagerMock.Setup(p => p.CheckPassword(registerDto.Password)).Returns(10);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<DataValidationException>();
    }
}
