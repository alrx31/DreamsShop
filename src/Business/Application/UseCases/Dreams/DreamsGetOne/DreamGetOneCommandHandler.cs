using Application.DTO;
using Application.Exceptions;
using Domain.IRepositories;
using Domain.IService;
using MediatR;

namespace Application.UseCases.Dreams.DreamsGetOne;

public class DreamGetOneCommandHandler(
        IUnitOfWork unitOfWork,
        IFileStorageService fileStorageService
    ) : IRequestHandler<DreamGetOneCommand, DreamGetDto?>
{
    public async Task<DreamGetDto?> Handle(DreamGetOneCommand request, CancellationToken cancellationToken)
    {
        var dream = await unitOfWork.DreamRepository.GetAsync([request.DreamId], cancellationToken);
        if (dream is null) throw new NotFoundException("Dream not found.");
        
        var dreamImg = await fileStorageService.DownloadFileAsync(dream.ImageFileName, cancellationToken);
        using var stream = new MemoryStream();
        await dreamImg.Content!.CopyToAsync(stream, cancellationToken);
        var imageBytes = stream.ToArray();

        var answerDto = new DreamGetDto
        {
            Id = dream.Id,
            Title = dream.Title,
            Description = dream.Description,
            ProducerId = dream.ProducerId,
            Rating = dream.Rating,

            ImageBase64 = Convert.ToBase64String(imageBytes),
            ImageContentType = dreamImg.ContentType
        };
        
        return answerDto;
    }
}