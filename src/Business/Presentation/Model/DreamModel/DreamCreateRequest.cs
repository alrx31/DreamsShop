namespace Presentation.Model.DreamModel;

public class DreamCreateRequest
{
    public required string Title { get; init; }
    public string? Description { get; init; }
    public Guid? ProducerId { get; init; }
    public decimal? Rating { get; init; }
    public IFormFile? Image { get; init; }
}