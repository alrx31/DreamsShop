using Application.Exceptions;
using Domain.IRepositories;
using MediatR;

namespace Application.UseCases.Category.CategoryRemove;

public class CategoryRemoveCommandHandler(
    IUnitOfWork unitOfWork
    ) : IRequestHandler<CategoryRemoveCommand>
{
    public async Task Handle(CategoryRemoveCommand request, CancellationToken cancellationToken)
    {
        var category = await unitOfWork.CategoryRepository.GetAsync([request.CategoryId], cancellationToken);
        if (category is null) throw new NotFoundException("Category not found.");
        
        await unitOfWork.CategoryRepository.DeleteAsync(category, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}