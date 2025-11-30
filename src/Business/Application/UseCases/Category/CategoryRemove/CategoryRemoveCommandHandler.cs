using Application.Exceptions;
using Application.UseCases.Base;
using Domain.IService;

namespace Application.UseCases.Category.CategoryRemove;

public class CategoryRemoveCommandHandler(
    ICacheService<Guid, Domain.Entity.Category> cacheService
    ) : BaseRequestHandler<CategoryRemoveCommand, MediatR.Unit>
{
    public override async Task<MediatR.Unit> Handle(CategoryRemoveCommand request, CancellationToken cancellationToken)
    {
        var category = await UnitOfWork.CategoryRepository.GetAsync([request.CategoryId], cancellationToken);
        if (category is null) throw new NotFoundException("Category not found.");
        
        await cacheService.RemoveAsync(request.CategoryId);
     
        await UnitOfWork.CategoryRepository.DeleteAsync(category, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);

        return MediatR.Unit.Value;
    }
}