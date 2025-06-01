using Application.Exceptions;
using AutoMapper;
using Domain.IRepositories;
using Domain.IService;
using MediatR;

namespace Application.UseCases.UserDream.UserDreamAdd;

public class UserDreamAddCommandHandler(
    IUnitOfWork unitOfWork,
    IHttpContextService httpContextService,
    IMapper mapper
    ) : IRequestHandler<UserDreamAddCommand>
{
    public async Task Handle(UserDreamAddCommand request, CancellationToken cancellationToken)
    {
        var userId = httpContextService.GetCurrentUserId();
        if(userId is null) throw new UnauthorizedException("Invalid user id.");

        var dream = await unitOfWork.DreamRepository.GetAsync(request.DreamId, cancellationToken);
        if(dream is null) throw new NotFoundException("Dream not found.");

        await unitOfWork.UserDreamRepository.AddAsync(
            mapper.Map<Domain.Entity.UserDream>( (userId,request.DreamId) )
            , cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}