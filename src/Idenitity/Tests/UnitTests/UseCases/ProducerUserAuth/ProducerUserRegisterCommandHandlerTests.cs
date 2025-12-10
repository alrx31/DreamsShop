using Application.UseCases.ProducerUserAuth.ProducerUserRegister;
using Application.DTO.ProducerUser;
using Application.Exceptions;
using AutoMapper;
using Bogus;
using Domain.Entity;
using Domain.IRepositories;
using Domain.IServices;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests.UnitTests.UseCases.ProducerUserAuth;

public class ProducerUserRegisterCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IProducerUserRepository> _producerUserRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IPasswordManager> _passwordManagerMock;
    private readonly Mock<IValidator<ProducerUserRegisterCommand>> _validatorMock;
    private readonly ProducerUserRegisterCommandHandler _handler;

    public ProducerUserRegisterCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _producerUserRepositoryMock = new Mock<IProducerUserRepository>();
        _mapperMock = new Mock<IMapper>();
        _passwordManagerMock = new Mock<IPasswordManager>();
        _validatorMock = new Mock<IValidator<ProducerUserRegisterCommand>>();
        
        _unitOfWorkMock.Setup(u => u.ProducerUserRepository).Returns(_producerUserRepositoryMock.Object);
        
        _handler = new ProducerUserRegisterCommandHandler(
            _unitOfWorkMock.Object,
            _validatorMock.Object,
            _mapperMock.Object,
            _passwordManagerMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldRegisterUser_WhenDataIsValid()
    {
        // Arrange
        var faker = new Faker();
        var password = faker.Internet.Password();
        var registerDto = new ProducerUserRegisterDto
        {
            Email = faker.Internet.Email(),
            Password = password,
            PasswordRepeat = password,
            Name = faker.Name.FullName()
        };
        var command = new ProducerUserRegisterCommand(registerDto);
        var validationResult = new FluentValidation.Results.ValidationResult();
        var user = new ProducerUser
        {
            Id = faker.Random.Guid(),
            ProducerId = faker.Random.Guid(),
            Email = registerDto.Email,
            Name = registerDto.Name,
            Password = password,
            Role = Roles.Producer
        };

        _validatorMock.Setup(v => v.ValidateAsync(command, CancellationToken.None)).ReturnsAsync(validationResult);
        _producerUserRepositoryMock.Setup(r => r.GetByEmailAsync(registerDto.Email, CancellationToken.None))
            .ReturnsAsync((ProducerUser?)null);
        _passwordManagerMock.Setup(p => p.CheckPassword(password)).Returns(255);
        _mapperMock.Setup(m => m.Map<ProducerUser>(command.Dto, It.IsAny<Action<IMappingOperationOptions<object, ProducerUser>>>())).Returns(user);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _producerUserRepositoryMock.Verify(r => r.AddAsync(user, CancellationToken.None), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowDataValidationException_WhenValidationFails()
    {
        // Arrange
        var faker = new Faker();
        var password = faker.Internet.Password();
        var registerDto = new ProducerUserRegisterDto
        {
            Email = faker.Internet.Email(),
            Password = password,
            PasswordRepeat = password,
            Name = faker.Name.FullName()
        };
        var command = new ProducerUserRegisterCommand(registerDto);
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
        var registerDto = new ProducerUserRegisterDto
        {
            Email = faker.Internet.Email(),
            Password = password,
            PasswordRepeat = password,
            Name = faker.Name.FullName()
        };
        var command = new ProducerUserRegisterCommand(registerDto);
        var validationResult = new FluentValidation.Results.ValidationResult();
        var user = new ProducerUser
        {
            Id = faker.Random.Guid(),
            ProducerId = faker.Random.Guid(),
            Email = registerDto.Email,
            Name = registerDto.Name,
            Password = password,
            Role = Roles.Producer
        };

        _validatorMock.Setup(v => v.ValidateAsync(command, CancellationToken.None)).ReturnsAsync(validationResult);
        _producerUserRepositoryMock.Setup(r => r.GetByEmailAsync(registerDto.Email, CancellationToken.None)).ReturnsAsync(user);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AlreadyExistException>();
    }
    
    [Fact]
    public async Task Handle_ShouldRegisterUser_WhenPasswordCheckIsLow()
    {
        // Arrange
        var faker = new Faker();
        var password = faker.Internet.Password();
        var registerDto = new ProducerUserRegisterDto
        {
            Email = faker.Internet.Email(),
            Password = password,
            PasswordRepeat = password,
            Name = faker.Name.FullName()
        };
        var command = new ProducerUserRegisterCommand(registerDto);
        var validationResult = new FluentValidation.Results.ValidationResult();
        var specificCommand = new ProducerUserRegisterCommand(registerDto);

        _validatorMock.Setup(v => v.ValidateAsync(specificCommand, CancellationToken.None)).ReturnsAsync(validationResult);
        _producerUserRepositoryMock.Setup(r => r.GetByEmailAsync(specificCommand.Dto.Email, CancellationToken.None))
            .ReturnsAsync((ProducerUser?)null);
        _passwordManagerMock.Setup(p => p.CheckPassword(password)).Returns(10);
        _mapperMock.Setup(m => m.Map<ProducerUser>(specificCommand.Dto, It.IsAny<Action<IMappingOperationOptions<object, ProducerUser>>>()))
            .Returns(new ProducerUser
            {
                Id = faker.Random.Guid(),
                ProducerId = faker.Random.Guid(),
                Email = specificCommand.Dto.Email,
                Name = specificCommand.Dto.Name,
                Password = specificCommand.Dto.Password,
                Role = Roles.Producer
            });

        // Act
        await _handler.Handle(specificCommand, CancellationToken.None);

        // Assert
        _producerUserRepositoryMock.Verify(r => r.AddAsync(It.IsAny<ProducerUser>(), CancellationToken.None), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
    }
}