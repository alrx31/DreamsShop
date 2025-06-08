using Domain.Model;
using MediatR;

namespace Application.UseCases.Dreams.DreamCreate;

public record DreamCreateCommand(string Title, string? Description, Guid? ProducerId, decimal? Rating, FileModel? Image)
    : IRequest<Guid>
{
    public Guid? ProducerId { get; set; } = ProducerId;
};