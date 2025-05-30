using AutoMapper;
using Domain.Entity;
using Domain.IRepositories;
using MediatR;

namespace Application.UseCases.CommandHandler;

public class DreamCreateCommandHandler
    (
        IUnitOfWork unitOfWork,
        IMapper mapper
    ) : IRequestHandler<DreamCreateCommand>
{
    public async Task Handle(DreamCreateCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.DreamRepository.AddAsync(mapper.Map<Dream>(request.Dto), cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}