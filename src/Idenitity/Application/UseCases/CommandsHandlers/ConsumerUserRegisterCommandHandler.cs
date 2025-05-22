using Application.UseCases.Commands;
using AutoMapper;
using Domain.IRepositories;
using Domain.IService;
using MediatR;

namespace Application.UseCases.CommandsHandlers;

public class ConsumerUserRegisterCommandHandler(
    IMapper mapper,
    IUnitOfWork unitOfWork,
    IPasswordChecker passwordChecker
    ) : IRequestHandler<ConsumerUserRegisterCommand>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPasswordChecker _passwordChecker = passwordChecker;

    public Task Handle(ConsumerUserRegisterCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}