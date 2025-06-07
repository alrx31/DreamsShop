using Domain.Entity;

namespace Application.DTO.ProducerUser;

public class ProducerUserAuthResponseDto
{
    public string? AccessToken { get; set; }
    public UserData? UserData { get; set; }
}