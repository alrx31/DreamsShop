using Application.Exceptions;
using Domain.Entity;
using Domain.IRepositories;
using MediatR;

namespace Application.UseCases.Dreams.DreamsGetOne;

public class DreamGetOneCommandHandler(
        IUnitOfWork unitOfWork
    ) : IRequestHandler<DreamGetOneCommand, Dream?>
{
    public async Task<Dream?> Handle(DreamGetOneCommand request, CancellationToken cancellationToken)
    {
        var dream = await unitOfWork.DreamRepository.GetAsync([request.DreamId], cancellationToken);
        if (dream is null) throw new NotFoundException("Dream not found.");
        
        return dream;
    }
}