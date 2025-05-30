using AutoMapper;
using Domain.IRepositories;
using MediatR;

namespace Application.UseCases.DreamCreate;

public class DreamCreateCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper
    ) : IRequestHandler<DreamCreateCommand>
{
    public async Task Handle(DreamCreateCommand request, CancellationToken cancellationToken)
    {
        var t = mapper.Map<Domain.Entity.Dream>(request);
        await unitOfWork.DreamRepository.AddAsync(t, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}