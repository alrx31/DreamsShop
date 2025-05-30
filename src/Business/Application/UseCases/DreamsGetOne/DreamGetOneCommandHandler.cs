using AutoMapper;
using Domain.Entity;
using Domain.IRepositories;
using MediatR;

namespace Application.UseCases.DreamsGetOne;

public class DreamGetOneCommandHandler
    (
        IUnitOfWork unitOfWork,
        IMapper mapper
    ) : IRequestHandler<DreamGetOneCommand, Dream?>
{
    public async Task<Dream?> Handle(DreamGetOneCommand request, CancellationToken cancellationToken)
    {
        return await unitOfWork.DreamRepository.GetAsync(request.DreamId, cancellationToken);
    }
}