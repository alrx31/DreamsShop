namespace Presentation.Model.DreamModel;

public class DreamCreateRequest
{
    public required string Title { get; set; }

    public string? Description { get; set; }

    public Guid? ProducerId { get; set; }

    public decimal? Rating { get; set; }
    
    public IFormFile? Image { get; set; }
}