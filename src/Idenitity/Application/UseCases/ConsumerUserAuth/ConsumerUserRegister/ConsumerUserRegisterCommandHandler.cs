using Application.Exceptions;
using AutoMapper;
using Domain.IRepositories;
using Domain.IServices;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.UseCases.ConsumerUserAuth.ConsumerUserRegister;

public class ConsumerUserRegisterCommandHandler(
    IMapper mapper,
    IUnitOfWork unitOfWork,
    IPasswordManager passwordManager,
    IValidator<ConsumerUserRegisterCommand> commandValidator
    ) : IRequestHandler<ConsumerUserRegisterCommand>
{
    private const byte AcceptablePasswordLevel = 255;
    public async Task Handle(ConsumerUserRegisterCommand request, CancellationToken cancellationToken)
    {
        var validateResult = await commandValidator.ValidateAsync(request, cancellationToken);

        if (!validateResult.IsValid)
        {
            throw new DataValidationException(validateResult.ToString());
        }
        
        var user = await unitOfWork.ConsumerUserRepository
            .GetByEmailAsync(request.Model.Email, cancellationToken);
        if (user is not null) throw new AlreadyExistException("This Email Is Already Registered.");
        
        var passwordLevel = passwordManager.CheckPassword(request.Model.Password);
        if (passwordLevel < AcceptablePasswordLevel)
        {
            throw new DataValidationException("Passwords Must Be More Complex");
        }

        await unitOfWork.ConsumerUserRepository
            .AddAsync(
                mapper.Map<Domain.Entity.ConsumerUser>(request.Model,
                    opt => opt.Items["PasswordHasher"] = passwordManager),
                cancellationToken
            );
    
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}