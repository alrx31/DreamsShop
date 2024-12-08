using BLL.DTO;
using BLL.Exceptions;
using BLL.IService;
using Bogus;
using DAL.Entities;
using DAL.IRepositories;
using FluentAssertions;
using Moq;

namespace Tests.UnitTests.Service.ProviderService;

public class RegisterProviderTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IProviderRepository> _providerRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    
    private readonly IProviderService _providerService;
    
    public RegisterProviderTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _providerRepositoryMock = new Mock<IProviderRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        
        
        _unitOfWorkMock.Setup(u => u.ProviderRepository).Returns(_providerRepositoryMock.Object);
        _unitOfWorkMock.Setup(u => u.UserRepository).Returns(_userRepositoryMock.Object);
        
        _providerService= new BLL.Services.ProviderService(
            _unitOfWorkMock.Object,
            _passwordHasherMock.Object
        );
    }

    [Fact]
    public async Task RegisterProvider_Success()
    {
        // Arrange

        var faker = new Faker();

        var requester = new User
        {
            Id = faker.Random.Guid(),
            Email = faker.Internet.Email(),
            Password = faker.Internet.Password(),
            Name = faker.Internet.UserName(),
            Role = Roles.PROVIDER,
        };

        var RegisterDTO = new RegisterProviderDTO
        {
            Name = faker.Company.CompanyName(),
            Description = faker.Internet.UserAgent(),
            AdminId = requester.Id
        };

        _userRepositoryMock.Setup(x => x.GetUser(requester.Id)).ReturnsAsync(requester);

        _providerRepositoryMock.Setup(x => x.GetProvider(RegisterDTO.Name)).ReturnsAsync((Provider)null);
        
        // Act

        await _providerService.RegisterProvider(RegisterDTO);
        
        // Assert
        
        _userRepositoryMock.Verify(x => x.GetUser(requester.Id),Times.Once);

        _providerRepositoryMock.Verify(x => x.GetProvider(RegisterDTO.Name), Times.Once());
    }

    [Fact]
    public async Task RegisterProvider_Fail_NameIsNotAvailable()
    { 
        // Arrange
        var faker = new Faker();
        
        var existProvider = new Provider
        {
            Id = faker.Random.Guid(),
            Name = faker.Company.CompanyName(),
            Description = faker.Internet.UserAgent(),
            Raiting = faker.Random.Float(1, 10)
        };
        
        var requester = new User
        {
            Id = faker.Random.Guid(),
            Email = faker.Internet.Email(),
            Password = faker.Internet.Password(),
            Name = faker.Internet.UserName(),
            Role = Roles.PROVIDER,
        };

        var RegisterDTO = new RegisterProviderDTO
        {
            Name = existProvider.Name,
            Description = faker.Internet.UserAgent(),
            AdminId = requester.Id
        };

        _userRepositoryMock.Setup(x => x.GetUser(requester.Id)).ReturnsAsync(requester);

        _providerRepositoryMock.Setup(x => x.GetProvider(RegisterDTO.Name)).ReturnsAsync(existProvider);

        // Act

        Func<Task> act = async () => await _providerService.RegisterProvider(RegisterDTO);
        
        // Assert

        await act.Should().ThrowAsync<NotFoundException>().WithMessage("Provider not found.");
    }

    [Fact]
    public async Task RegisterProvider_Fail_RequesterNotFound()
    {
        // Arrange
        var faker = new Faker();
        
        var RegisterDTO = new RegisterProviderDTO
        {
            Name = faker.Internet.UserName(),
            Description = faker.Internet.UserAgent(),
            AdminId = faker.Random.Guid()
        };
        
        // Act
        
        Func<Task> act = async () => await _providerService.RegisterProvider(RegisterDTO);
        
        // Assert

        await act.Should().ThrowAsync<NotFoundException>().WithMessage("Requester not found.");
    }

    [Fact]
    public async Task RegisterProvider_Fail_RequesterCantCreateEvenMoreProviders()
    {
        // Arrange
        var faker = new Faker();
        
        var existProvider = new Provider
        {
            Id = faker.Random.Guid(),
            Name = faker.Company.CompanyName(),
            Description = faker.Internet.UserAgent(),
            Raiting = faker.Random.Float(1, 10)
        };
        
        var requester = new User
        {
            Id = faker.Random.Guid(),
            Email = faker.Internet.Email(),
            Password = faker.Internet.Password(),
            Name = faker.Internet.UserName(),
            Role = Roles.PROVIDER,
            ProducerId = existProvider.Id
        };

        var RegisterDTO = new RegisterProviderDTO
        {
            Name = faker.Name.FullName(),
            Description = faker.Internet.UserAgent(),
            AdminId = requester.Id
        };

        _userRepositoryMock.Setup(x => x.GetUser(requester.Id)).ReturnsAsync(requester);

        _providerRepositoryMock.Setup(x => x.GetProvider((Guid)requester.ProducerId)).ReturnsAsync(existProvider);

        // Act

        Func<Task> act = async () => await _providerService.RegisterProvider(RegisterDTO);
        
        // Assert

        await act.Should().ThrowAsync<NotFoundException>().WithMessage("You can't create more than 1 provider.");
    }
}