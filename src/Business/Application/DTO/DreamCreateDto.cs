namespace Application.DTO;

public class DreamCreateDto
{
    public required string Title { get; set; }

    public string? Description { get; set; }

    public Guid? ProducerId { get; set; }

    public decimal? Rating { get; set; }
}