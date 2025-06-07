using Application.Exceptions;
using AutoMapper;
using Domain.Entity;
using Domain.IRepositories;
using Domain.IServices;
using FluentValidation;
using MediatR;

namespace Application.UseCases.ProducerUserAuth.ProducerUserRegister;

public class ProducerUserRegisterCommandHandler(
    IUnitOfWork unitOfWork,
    IValidator<ProducerUserRegisterCommand> commandValidator,
    IMapper mapper,
    IPasswordManager passwordManager
    ) : IRequestHandler<ProducerUserRegisterCommand>
{
    public async Task Handle(ProducerUserRegisterCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await commandValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) throw new DataValidationException(validationResult.ToString());
        
        var existUser = await unitOfWork.ProducerUserRepository.GetByEmailAsync(request.Dto.Email, cancellationToken);
        if (existUser is not null) throw new AlreadyExistException("Producer user already exist.");

        var user = mapper.Map<ProducerUser>(request, opts =>
        {
            opts.Items["PasswordHasher"] = passwordManager;
        });
        user.Role = Roles.Provider;
        
        await unitOfWork.ProducerUserRepository.AddAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}