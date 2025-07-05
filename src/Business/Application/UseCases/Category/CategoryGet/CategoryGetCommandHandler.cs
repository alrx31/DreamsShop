using Application.Exceptions;
using Domain.IRepositories;
using MediatR;

namespace Application.UseCases.Category.CategoryGet;

public class CategoryGetCommandHandler(
    IUnitOfWork unitOfWork
    ) : IRequestHandler<CategoryGetCommand, Domain.Entity.Category?>
{
    public async Task<Domain.Entity.Category?> Handle(CategoryGetCommand request, CancellationToken cancellationToken)
    {
        var category = await unitOfWork.CategoryRepository.GetAsync([request.CategoryId],cancellationToken);
        if (category is null) throw new NotFoundException("Category not found.");
        
        return category;
    }
}