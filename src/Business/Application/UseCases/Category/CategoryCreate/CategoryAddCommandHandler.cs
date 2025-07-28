using AutoMapper;
using Domain.IRepositories;
using MediatR;

namespace Application.UseCases.Category.CategoryCreate;

public class CategoryAddCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper
    ) : IRequestHandler<CategoryAddCommand, Guid>
{
    public async Task<Guid> Handle(CategoryAddCommand request, CancellationToken cancellationToken)
    {
        var id = await unitOfWork.CategoryRepository.AddAsync(
            mapper.Map<Domain.Entity.Category>(request),
            cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return id;
    }
}