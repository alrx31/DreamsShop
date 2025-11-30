using Application.Exceptions;
using Application.UseCases.Base;
using Domain.IService;

namespace Application.UseCases.Category.CategoryGet;

public class CategoryGetCommandHandler(
    ICacheService<Guid, Domain.Entity.Category> cacheService
    ) : BaseRequestHandler<CategoryGetCommand, Domain.Entity.Category?>
{
    public override async Task<Domain.Entity.Category?> Handle(CategoryGetCommand request, CancellationToken cancellationToken)
    {

        var cachedCategory = await cacheService.GetAsync(request.CategoryId);
        if (cachedCategory is not null) return cachedCategory;

        var category = await UnitOfWork.CategoryRepository.GetAsync([request.CategoryId],cancellationToken);
        if (category is null) throw new NotFoundException("Category not found.");

        await cacheService.SetAsync(request.CategoryId, category);
        return category;
    }
}