using Application.Exceptions;
using Domain.IRepositories;
using MediatR;

namespace Application.UseCases.Category.CategoryUpdate;

public class CategoryUpdateCommandHandler(
    IUnitOfWork unitOfWork
    ) : IRequestHandler<CategoryUpdateCommand>
{
    public async Task Handle(CategoryUpdateCommand request, CancellationToken cancellationToken)
    {
        var category = await unitOfWork.CategoryRepository.GetAsync(request.CategoryId, cancellationToken);
        if (category is null) throw new NotFoundException("Category not found.");

        if (!string.IsNullOrWhiteSpace(request.Dto.Description))
        {
            category.Description = request.Dto.Description;
        }

        if (!string.IsNullOrWhiteSpace(request.Dto.Title))
        {
            category.Title = request.Dto.Title;
        }
        
        await unitOfWork.CategoryRepository.UpdateAsync(category, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}