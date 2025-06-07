namespace Application.DTO;

public class CategoryResponseDto
{
    public required Guid CategoryId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
}