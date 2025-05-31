using Application.Exceptions;
using AutoMapper;
using Domain.Entity;
using Domain.IRepositories;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Application.UseCases.ProducerUserAuth.ProducerUserRegister;

public class ProducerUserRegisterCommandHandler(
    IUnitOfWork unitOfWork,
    IValidator<ProducerUserRegisterCommand> commandValidator,
    IMapper mapper
    ) : IRequestHandler<ProducerUserRegisterCommand>
{
    public async Task Handle(ProducerUserRegisterCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await commandValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new DataValidationException(validationResult.ToString());
        }
        
        var existUser = await unitOfWork.ProducerUserRepository.GetByEmailAsync(request.Dto.Email, cancellationToken);
        if (existUser is not null) throw new AlreadyExistException("Producer user already exist.");

        var user = mapper.Map<ProducerUser>(request.Dto);
        
        await unitOfWork.ProducerUserRepository.AddAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}