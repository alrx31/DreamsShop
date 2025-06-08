using Domain.Entity;

namespace Application.DTO;

public class DreamResponseDto
{
    public required Guid Id { get; init; }
    
    public required string Title { get; set; }
    public required string Description { get; set; }
    public Guid? ProducerId { get; set; }
    public decimal? Rating { get; set; }
    
    public ICollection<CategoryResponseDto>? Categories { get; set; }
    
    public string? ImageBase64 { get; set; }
    public string? ImageContentType { get; set; }
}