using Application.Exceptions;
using Application.UseCases.Base;

namespace Application.UseCases.Category.CategoryUpdate;

public class CategoryUpdateCommandHandler : BaseRequestHandler<CategoryUpdateCommand, MediatR.Unit>
{
    public override async Task<MediatR.Unit> Handle(CategoryUpdateCommand request, CancellationToken cancellationToken)
    {
        var category = await UnitOfWork.CategoryRepository.GetAsync([request.CategoryId], cancellationToken);
        if (category is null) throw new NotFoundException("Category not found.");

        if (!string.IsNullOrWhiteSpace(request.Dto.Description))
        {
            category.Description = request.Dto.Description;
        }

        if (!string.IsNullOrWhiteSpace(request.Dto.Title))
        {
            category.Title = request.Dto.Title;
        }
        
        await UnitOfWork.CategoryRepository.UpdateAsync(category, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);

        return MediatR.Unit.Value;
    }
}