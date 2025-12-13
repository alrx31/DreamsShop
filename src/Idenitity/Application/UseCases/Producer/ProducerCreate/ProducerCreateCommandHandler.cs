using Application.Exceptions;
using AutoMapper;
using Domain.IRepositories;
using Domain.IServices;
using FluentValidation;
using MediatR;

namespace Application.UseCases.Producer.ProducerCreate;

public class ProducerCreateCommandHandler(
    IMapper mapper,
    IUnitOfWork unitOfWork,
    IPasswordManager passwordManager,
    IValidator<ProducerCreateCommand> commandValidator
) : IRequestHandler<ProducerCreateCommand, Guid>
{
    public async Task<Guid> Handle(ProducerCreateCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await commandValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) throw new DataValidationException(validationResult.ToString());
        
        var existingProducerUser = await unitOfWork.ProducerUserRepository
            .GetByEmailAsync(request.Dto.ProducerUser!.Email, cancellationToken);
        if (existingProducerUser is not null)
        {
            throw new AlreadyExistException("Producer user with this email already exists.");
        }

        var existingProducer = await unitOfWork.ProducerRepository
            .GetByTitleAsync(request.Dto.Title, cancellationToken);
        if (existingProducer is not null)
        {
            throw new AlreadyExistException("Producer with this title already exists.");
        }

        var producer = mapper.Map<Domain.Entity.Producer>(request.Dto);
        await unitOfWork.ProducerRepository.AddAsync(producer, cancellationToken);

        var user = mapper.Map<Domain.Entity.ProducerUser>(request.Dto.ProducerUser!, opts =>
        {
            opts.Items["PasswordHasher"] = passwordManager;
        });

        user.Role = Domain.Entity.Roles.ProducerAdmin;
        user.ProducerId = producer.Id; 

        await unitOfWork.ProducerUserRepository.AddAsync(user, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return producer.Id;
    }
}
