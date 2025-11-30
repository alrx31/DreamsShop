using Application.UseCases.Base;

namespace Application.UseCases.Category.CategoryGetAll;

public class CategoryGetAllCommandHandler : BaseRequestHandler<CategoryGetAllCommand, List<Domain.Entity.Category>>
{
    public override async Task<List<Domain.Entity.Category>> Handle(CategoryGetAllCommand request, CancellationToken cancellationToken)
    {
        return await UnitOfWork.CategoryRepository.GetAsync<Domain.Entity.Category>(cancellationToken:cancellationToken);
    }
}