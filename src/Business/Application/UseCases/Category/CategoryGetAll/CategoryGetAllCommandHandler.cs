using Domain.IRepositories;
using MediatR;

namespace Application.UseCases.Category.CategoryGetAll;

public class CategoryGetAllCommandHandler(
    IUnitOfWork unitOfWork
    ) : IRequestHandler<CategoryGetAllCommand, List<Domain.Entity.Category>>
{
    public async Task<List<Domain.Entity.Category>> Handle(CategoryGetAllCommand request, CancellationToken cancellationToken)
    {
        return await unitOfWork.CategoryRepository.GetAllAsync(cancellationToken);
    }
}