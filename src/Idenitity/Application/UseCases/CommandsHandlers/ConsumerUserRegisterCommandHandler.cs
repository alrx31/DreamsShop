using Application.Exceptions;
using Application.UseCases.Commands;
using AutoMapper;
using Domain.Entity;
using Domain.IRepositories;
using Domain.IServices;
using MediatR;

namespace Application.UseCases.CommandsHandlers;

public class ConsumerUserRegisterCommandHandler(
    IMapper mapper,
    IUnitOfWork unitOfWork,
    IPasswordManager passwordManager
    ) : IRequestHandler<ConsumerUserRegisterCommand>
{
    private const byte AcceptablePasswordLevel = 255;
    public async Task Handle(ConsumerUserRegisterCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.ConsumerUserRepository
            .GetByEmailAsync(request.Model.Email, cancellationToken);
        if (user is not null) throw new AlreadyExistException("This Email Is Already Registered.");
        
        if (request.Model.Password != request.Model.PasswordRepeat)
        {
            throw new InvalidDataModelException("Passwords do not match.");
        }
        
        var passwordLevel = passwordManager.CheckPassword(request.Model.Password);
        if (passwordLevel < AcceptablePasswordLevel)
        {
            throw new DataValidationException("Passwords Must Be More Complex");
        }
        
        await unitOfWork.ConsumerUserRepository
            .AddAsync(
                mapper.Map<ConsumerUser>(request.Model,
                opt => opt.Items["PasswordHasher"] = passwordManager),
                cancellationToken
                );
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        
    }
}