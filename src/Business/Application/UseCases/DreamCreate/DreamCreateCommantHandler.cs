using AutoMapper;
using Domain.IRepositories;
using MediatR;

namespace Application.UseCases.DreamCreate;

public class DreamCreateCommandHandler
    (
        IUnitOfWork unitOfWork,
        IMapper mapper
    ) : IRequestHandler<DreamCreateCommand>
{
    public async Task Handle(DreamCreateCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.DreamRepository.AddAsync(mapper.Map<Domain.Entity.Dream>(request.Dto), cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}