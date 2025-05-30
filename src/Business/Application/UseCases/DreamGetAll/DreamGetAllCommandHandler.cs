using Domain.Entity;
using Domain.IRepositories;
using MediatR;

namespace Application.UseCases.DreamGetAll;

public class DreamGetAllCommandHandler(
    IUnitOfWork unitOfWork
    ): IRequestHandler<DreamGetAllCommand, List<Dream>>
{
    public async Task<List<Dream>> Handle(DreamGetAllCommand request, CancellationToken cancellationToken)
    {
        return await unitOfWork.DreamRepository.GetRangeAsync(request.StartIndex,request.Count,cancellationToken);
    }
}