using Application.UseCases.Base;

namespace Application.UseCases.Dreams.DreamGetCount;

public class DreamGetCountCommandHandler : BaseRequestHandler<DreamGetCountCommand, int?>
{
    public override async Task<int?> Handle(DreamGetCountCommand request, CancellationToken cancellationToken)
    {
        return await UnitOfWork.DreamRepository.GetCountAsync(cancellationToken);
    }
}