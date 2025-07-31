using Domain.Model;

namespace Application.DTO;

public class DreamUpdateDto
{
    public string? Title { get; init; }
    public string? Description { get; init; }

    public FileModel? Image { get; init; }
}