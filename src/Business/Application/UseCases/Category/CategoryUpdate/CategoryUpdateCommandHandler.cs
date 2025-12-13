using Application.DTO;
using Application.Exceptions;
using Application.UseCases.Base;

namespace Application.UseCases.Category.CategoryUpdate;

public class CategoryUpdateCommandHandler : BaseRequestHandler<CategoryUpdateCommand, MediatR.Unit>
{
    public override async Task<MediatR.Unit> Handle(CategoryUpdateCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto ?? new CategoryUpdateDto();

        var category = await UnitOfWork.CategoryRepository.GetAsync([request.CategoryId], cancellationToken);
        if (category is null) throw new NotFoundException("Category not found.");

        if (!string.IsNullOrWhiteSpace(dto.Description))
        {
            category.Description = dto.Description;
        }

        if (!string.IsNullOrWhiteSpace(dto.Title))
        {
            category.Title = dto.Title;
        }
        
        await UnitOfWork.CategoryRepository.UpdateAsync(category, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);

        return MediatR.Unit.Value;
    }
}