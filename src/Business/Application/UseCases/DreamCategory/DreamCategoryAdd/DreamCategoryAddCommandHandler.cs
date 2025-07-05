using Application.Exceptions;
using AutoMapper;
using Domain.IRepositories;
using MediatR;

namespace Application.UseCases.DreamCategory.DreamCategoryAdd;

public class DreamCategoryAddCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper
    ) : IRequestHandler<DreamCategoryAddCommand>
{
    public async Task Handle(DreamCategoryAddCommand request, CancellationToken cancellationToken)
    {
        var dream = await unitOfWork.DreamRepository.GetAsync([request.DreamId], cancellationToken);
        if (dream is null) throw new NotFoundException("Dream not found.");
        
        var category = await unitOfWork.CategoryRepository.GetAsync([request.CategoryId], cancellationToken);
        if (category is null) throw new NotFoundException("Category not found.");
        
        await unitOfWork.DreamCategoryRepository
            .AddAsync(mapper.Map<Domain.Entity.DreamCategory>(request), cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}