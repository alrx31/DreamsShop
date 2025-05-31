using Application.Exceptions;
using AutoMapper;
using Domain.Entity;
using Domain.IRepositories;
using Domain.IService;
using MediatR;

namespace Application.UseCases.Dreams.DreamCreate;

public class DreamCreateCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IHttpContextService httpContextService
    ) : IRequestHandler<DreamCreateCommand>
{
    public async Task Handle(DreamCreateCommand request, CancellationToken cancellationToken)
    {
        var userId = httpContextService.GetCurrentUserId();
        if(userId is null) throw new UnauthorizedException("Invalid user id.");
        
        request.Dto.ProducerId = userId;
        
        await unitOfWork.DreamRepository.AddAsync(
            mapper.Map<Dream>(request),
            cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}