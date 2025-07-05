using Application.Exceptions;
using Domain.IRepositories;
using Domain.IService;
using MediatR;

namespace Application.UseCases.Category.CategoryRemove;

public class CategoryRemoveCommandHandler(
    IUnitOfWork unitOfWork,
    ICacheService<Guid, Domain.Entity.Category> cacheService
    ) : IRequestHandler<CategoryRemoveCommand>
{
    public async Task Handle(CategoryRemoveCommand request, CancellationToken cancellationToken)
    {
        var category = await unitOfWork.CategoryRepository.GetAsync([request.CategoryId], cancellationToken);
        if (category is null) throw new NotFoundException("Category not found.");
        
        await cacheService.RemoveAsync(request.CategoryId);
     
        await unitOfWork.CategoryRepository.DeleteAsync(category, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}