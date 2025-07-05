using Domain.Model;

namespace Application.DTO;

public class DreamCreateDto
{
    public required string Title { get; init; }
    public string? Description { get; init; }
    public Guid? ProducerId { get; init; }
    public decimal? Rating { get; init; }
    public FileModel? Image { get; init; }
}