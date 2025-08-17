using Application.DTO.ProducerUser;

namespace Application.DTO.Producer;

public class ProducerCreateDTO
{
    public required string Title { get; set; }
    public string? Description { get; set; }

    public ProducerUserRegisterDto? ProducerUser { get; set; }
}
