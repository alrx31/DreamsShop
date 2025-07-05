using Application.Exceptions;
using AutoMapper;
using Domain.IRepositories;
using MediatR;

namespace Application.UseCases.DreamCategory.DreamCategoryDelete;

public class DreamCategoryDeleteCommandHandler (
    IUnitOfWork unitOfWork
    ) : IRequestHandler<DreamCategoryDeleteCommand>
{
    public async Task Handle(DreamCategoryDeleteCommand request, CancellationToken cancellationToken)
    {
        var dreamCategory = await unitOfWork.DreamCategoryRepository.GetAsync([request.DreamId,request.CategoryId], cancellationToken);
        if(dreamCategory is null) throw new NotFoundException("Dream category not found.");
        
        await unitOfWork.DreamCategoryRepository.DeleteAsync(dreamCategory, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}