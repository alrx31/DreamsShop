using Domain.IRepositories;
using MediatR;

namespace Application.UseCases.DreamGetCount;

public class DreamGetCountCommandHandler (
    IUnitOfWork unitOfWork
    ): IRequestHandler<DreamGetCountCommand, int?>
{
    public async Task<int?> Handle(DreamGetCountCommand request, CancellationToken cancellationToken)
    {
        return await  unitOfWork.DreamRepository.GetCountAsync(cancellationToken);
    }
}