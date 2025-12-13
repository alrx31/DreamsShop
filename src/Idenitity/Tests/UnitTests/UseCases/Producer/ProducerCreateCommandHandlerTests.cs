using Application.DTO.Producer;
using Application.DTO.ProducerUser;
using Application.Exceptions;
using Application.UseCases.Producer.ProducerCreate;
using Application.UseCases.ProducerUserAuth.ProducerUserRegister;
using AutoMapper;
using Domain.Entity;
using Domain.IRepositories;
using Domain.IServices;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace Tests.UnitTests.UseCases.Producer;

public class ProducerCreateCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IProducerRepository> _producerRepository = new();
    private readonly Mock<IProducerUserRepository> _producerUserRepository = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IPasswordManager> _passwordManager = new();
    private readonly Mock<IValidator<ProducerCreateCommand>> _validator = new();
    private readonly ProducerCreateCommandHandler _handler;

    public ProducerCreateCommandHandlerTests()
    {
        _unitOfWork.Setup(u => u.ProducerRepository).Returns(_producerRepository.Object);
        _unitOfWork.Setup(u => u.ProducerUserRepository).Returns(_producerUserRepository.Object);

        _handler = new ProducerCreateCommandHandler(
            _mapper.Object,
            _unitOfWork.Object,
            _passwordManager.Object,
            _validator.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateProducerAndUser()
    {
        var dto = new ProducerCreateDTO
        {
            Title = "Studio",
            Description = "Desc",
            ProducerUser = new ProducerUserRegisterDto
            {
                Email = "producer@test.com",
                Name = "Producer",
                Password = "password",
                PasswordRepeat = "password"
            }
        };
        var command = new ProducerCreateCommand(dto, new ProducerUserRegisterCommand(dto.ProducerUser!));
        var producer = new Domain.Entity.Producer { Id = Guid.NewGuid(), Title = dto.Title, Description = dto.Description };
        var user = new ProducerUser { Id = Guid.NewGuid(), ProducerId = producer.Id, Email = dto.ProducerUser!.Email, Name = dto.ProducerUser.Name, Password = "hashed", Role = Roles.Producer };

        _validator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _producerUserRepository.Setup(r => r.GetByEmailAsync(dto.ProducerUser!.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ProducerUser?)null);
        _producerRepository.Setup(r => r.GetByTitleAsync(dto.Title, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entity.Producer?)null);
        _mapper.Setup(m => m.Map<Domain.Entity.Producer>(dto)).Returns(producer);
        _mapper.Setup(m => m.Map<ProducerUser>(dto.ProducerUser, It.IsAny<Action<IMappingOperationOptions<object, ProducerUser>>>()))
            .Returns(user);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().Be(producer.Id);
        _producerRepository.Verify(r => r.AddAsync(producer, It.IsAny<CancellationToken>()), Times.Once);
        _producerUserRepository.Verify(r => r.AddAsync(user, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenValidationFails()
    {
        var invalidUser = new ProducerUserRegisterDto
        {
            Email = "invalid",
            Name = "name",
            Password = "pwd",
            PasswordRepeat = "pwd"
        };
        var command = new ProducerCreateCommand(
            new ProducerCreateDTO { Title = "", ProducerUser = invalidUser },
            new ProducerUserRegisterCommand(invalidUser));

        _validator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(new[] { new ValidationFailure("Title", "required") }));

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<DataValidationException>();
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenUserExists()
    {
        var dto = new ProducerCreateDTO
        {
            Title = "Studio",
            ProducerUser = new ProducerUserRegisterDto
            {
                Email = "producer@test.com",
                Name = "Producer",
                Password = "password",
                PasswordRepeat = "password"
            }
        };
        var command = new ProducerCreateCommand(dto, new ProducerUserRegisterCommand(dto.ProducerUser!));

        _validator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _producerUserRepository.Setup(r => r.GetByEmailAsync(dto.ProducerUser!.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ProducerUser
            {
                Id = Guid.NewGuid(),
                ProducerId = Guid.NewGuid(),
                Email = "exists@test.com",
                Name = "Exists",
                Password = "hashed",
                Role = Roles.Producer
            });

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<AlreadyExistException>();
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenProducerTitleExists()
    {
        var dto = new ProducerCreateDTO
        {
            Title = "Studio",
            ProducerUser = new ProducerUserRegisterDto
            {
                Email = "producer@test.com",
                Name = "Producer",
                Password = "password",
                PasswordRepeat = "password"
            }
        };
        var command = new ProducerCreateCommand(dto, new ProducerUserRegisterCommand(dto.ProducerUser!));

        _validator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _producerUserRepository.Setup(r => r.GetByEmailAsync(dto.ProducerUser!.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ProducerUser?)null);
        _producerRepository.Setup(r => r.GetByTitleAsync(dto.Title, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Domain.Entity.Producer
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description
            });

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<AlreadyExistException>();
    }
}
