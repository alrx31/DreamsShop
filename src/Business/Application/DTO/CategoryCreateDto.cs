namespace Application.DTO;

public class CategoryCreateDto
{
    public required string Title { get; init; }
    public string? Description { get; init; }
}