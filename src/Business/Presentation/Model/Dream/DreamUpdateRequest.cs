using System;

namespace Presentation.Model.Dream;

public class DreamUpdateRequest
{
    public string? Title { get; init; }
    public string? Description { get; init; }
    public IFormFile? Image { get; init; }
}
