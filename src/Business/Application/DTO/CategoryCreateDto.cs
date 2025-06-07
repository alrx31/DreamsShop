namespace Application.DTO;

public class CategoryCreateDto
{
    public required string Title { get; set; }
    
    public string? Description { get; set; }
}