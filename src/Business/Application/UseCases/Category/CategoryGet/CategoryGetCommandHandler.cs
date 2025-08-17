using Application.Exceptions;
using Domain.IRepositories;
using Domain.IService;
using MediatR;

namespace Application.UseCases.Category.CategoryGet;

public class CategoryGetCommandHandler(
    IUnitOfWork unitOfWork,
    ICacheService<Guid, Domain.Entity.Category> cacheService
    ) : IRequestHandler<CategoryGetCommand, Domain.Entity.Category?>
{
    public async Task<Domain.Entity.Category?> Handle(CategoryGetCommand request, CancellationToken cancellationToken)
    {

        var cachedCategory = await cacheService.GetAsync(request.CategoryId);
        if (cachedCategory is not null) return cachedCategory;

        var category = await unitOfWork.CategoryRepository.GetAsync([request.CategoryId],cancellationToken);
        if (category is null) throw new NotFoundException("Category not found.");

        await cacheService.SetAsync(request.CategoryId, category);
        return category;
    }
}