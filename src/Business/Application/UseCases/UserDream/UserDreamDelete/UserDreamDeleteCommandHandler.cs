using Application.Exceptions;
using AutoMapper;
using Domain.IRepositories;
using Domain.IService;
using MediatR;

namespace Application.UseCases.UserDream.UserDreamDelete;

public class UserDreamDeleteCommandHandler(
    IUnitOfWork unitOfWork,
    IHttpContextService httpContextService
    ) : IRequestHandler<UserDreamDeleteCommand>
{
    public async Task Handle(UserDreamDeleteCommand request, CancellationToken cancellationToken)
    {
        var userId = httpContextService.GetCurrentUserId();
        if (userId is null) throw new UnauthorizedException("Invalid user id.");

        var userDream = await unitOfWork.UserDreamRepository.GetAsync(request.DreamId, cancellationToken);
        if (userDream is null) throw new NotFoundException("Dream not found.");
        
        await unitOfWork.UserDreamRepository.DeleteAsync(userDream, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}