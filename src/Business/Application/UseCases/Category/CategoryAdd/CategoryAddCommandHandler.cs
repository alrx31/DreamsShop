using AutoMapper;
using Domain.IRepositories;
using MediatR;

namespace Application.UseCases.Category.CategoryAdd;

public class CategoryAddCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper
    ) : IRequestHandler<CategoryAddCommand>
{
    public async Task Handle(CategoryAddCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.CategoryRepository.AddAsync(
            mapper.Map<Domain.Entity.Category>(request),
            cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}