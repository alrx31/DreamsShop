using Application.DTO.Producer;
using Application.DTO.ProducerUser;
using Application.MappingProfiles;
using Application.UseCases.Producer.ProducerCreate;
using Application.UseCases.ProducerUserAuth.ProducerUserRegister;
using AutoMapper;
using Domain.Entity;
using FluentAssertions;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Tests.IntegrationTests.Fixtures;
using Xunit;
using ApplicationValidator = Application.UseCases.Producer.ProducerCreate.ProducerCreateCommandValidator;

namespace Tests.IntegrationTests.UseCases.Producer;

[Collection("IntegrationTests")]
public class ProducerCreateCommandHandlerIntegrationTests
{
    private readonly PostgreSqlFixture _fixture;

    public ProducerCreateCommandHandlerIntegrationTests(PostgreSqlFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Handle_ShouldPersistProducerAndAdminUser()
    {
        await _fixture.ResetDatabaseAsync();
        await using var context = _fixture.CreateContext();

        var mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProducerMapperProfile>();
            cfg.AddProfile<ProducerUserMapperProfile>();
        }).CreateMapper();

        var unitOfWork = new UnitOfWork(
            context,
            new ConsumerUserRepository(context),
            new ProducerUserRepository(context),
            new ProducerRepository(context));

        var handler = new ProducerCreateCommandHandler(
            mapper,
            unitOfWork,
            new PasswordManager(),
            new ApplicationValidator());

        var dto = new ProducerCreateDTO
        {
            Title = "Integration Studio",
            Description = "Integration description",
            ProducerUser = new ProducerUserRegisterDto
            {
                Email = "owner@test.com",
                Name = "Owner",
                Password = "SecureP@ss1",
                PasswordRepeat = "SecureP@ss1"
            }
        };

        var command = new ProducerCreateCommand(dto, new ProducerUserRegisterCommand(dto.ProducerUser!));

        var producerId = await handler.Handle(command, CancellationToken.None);

        var createdProducer = await context.Producer.FindAsync(producerId);
        var createdUser = await context.ProducerUser.FirstOrDefaultAsync(u => u.ProducerId == producerId);

        createdProducer.Should().NotBeNull();
        createdProducer!.Title.Should().Be(dto.Title);

        createdUser.Should().NotBeNull();
        createdUser!.Email.Should().Be(dto.ProducerUser!.Email);
        createdUser.ProducerId.Should().Be(producerId);
        createdUser.Role.Should().Be(Roles.ProducerAdmin);
        createdUser.Password.Should().NotBeNullOrWhiteSpace();
    }
}
