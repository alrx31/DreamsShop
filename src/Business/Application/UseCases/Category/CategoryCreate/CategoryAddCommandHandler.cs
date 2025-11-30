using Application.UseCases.Base;

namespace Application.UseCases.Category.CategoryCreate;

public class CategoryAddCommandHandler : BaseRequestHandler<CategoryAddCommand, Guid>
{
    public override async Task<Guid> Handle(CategoryAddCommand request, CancellationToken cancellationToken)
    {
        var id = (await UnitOfWork.CategoryRepository.AddAsync(
            Mapper.Map<Domain.Entity.Category>(request),
            cancellationToken)).CategoryId;
        await UnitOfWork.SaveChangesAsync(cancellationToken);
        
        return id;
    }
}