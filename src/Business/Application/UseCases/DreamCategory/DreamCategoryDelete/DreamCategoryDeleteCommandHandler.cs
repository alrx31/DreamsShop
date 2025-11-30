using Application.Exceptions;
using Application.UseCases.Base;
using Domain.Specifications;

namespace Application.UseCases.DreamCategory.DreamCategoryDelete;

public class DreamCategoryDeleteCommandHandler : BaseRequestHandler<DreamCategoryDeleteCommand, MediatR.Unit>
{
    public override async Task<MediatR.Unit> Handle(DreamCategoryDeleteCommand request, CancellationToken cancellationToken)
    {
        var dreamCategory = await UnitOfWork.DreamCategoryRepository.GetAsync([request.DreamId,request.CategoryId], cancellationToken);
        if(dreamCategory is null) throw new NotFoundException("Dream category not found.");
        
        await UnitOfWork.DreamCategoryRepository.DeleteAsync(dreamCategory, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);

        return MediatR.Unit.Value;
    }
}