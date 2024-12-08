using BLL.Exceptions;
using BLL.IService;
using Bogus;
using DAL.Entities;
using DAL.IRepositories;
using FluentAssertions;
using Moq;

namespace Tests.UnitTests.Service.ProviderService;

public class GetProviderTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IProviderRepository> _providerRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    
    private readonly IProviderService _providerService;
    
    public GetProviderTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _providerRepositoryMock = new Mock<IProviderRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        
        
        _unitOfWorkMock.Setup(u => u.ProviderRepository).Returns(_providerRepositoryMock.Object);
        
        _providerService= new BLL.Services.ProviderService(
            _unitOfWorkMock.Object,
            _passwordHasherMock.Object
        );
    }

    [Fact]
    public async Task GetProviderById_Success_WhenProviderExist()
    {
        var faker = new Faker();

        var existProvider = new Provider
        {
            Id = faker.Random.Guid(),
            Name = faker.Company.CompanyName(),
            Description = faker.Internet.UserAgent(),
            Raiting = faker.Random.Float(1, 10)
        };

        _providerRepositoryMock.Setup(x => x.GetProvider(existProvider.Id)).ReturnsAsync(existProvider);
        
        // Act

        var result = await _providerService.GetProvider(existProvider.Id);
        
        // Assert

        result.Should().BeEquivalentTo(existProvider);
    }
    
    [Fact]
    public async Task GetProviderById_Fail_ProviderNotFound()
    {
        // Arrange
        var faker = new Faker();
        
        // Act
        Func<Task> act = async () => await _providerService.GetProvider(faker.Random.Guid());
        
        // Assert

        await act.Should().ThrowAsync<NotFoundException>().WithMessage("Provider not found.");
    }

    [Fact]
    public async Task GetProviderByName_Success_WhenProviderExist()
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

        _providerRepositoryMock.Setup(x => x.GetProvider(existProvider.Name)).ReturnsAsync(existProvider);
        
        // Act

        var result = await _providerService.GetProvider(existProvider.Name);
        
        // Asesrt

        result.Should().BeEquivalentTo(existProvider);
    }

    [Fact]
    public async Task GetProviderByName_Fail_ProviderNotFound()
    {
        // Arrange
        var faker = new Faker();
        
        // Act
        Func<Task> act = async () => await _providerService.GetProvider(faker.Internet.UserAgent());
        
        // Assert

        await act.Should().ThrowAsync<NotFoundException>().WithMessage("Provider not found.");
    }
}