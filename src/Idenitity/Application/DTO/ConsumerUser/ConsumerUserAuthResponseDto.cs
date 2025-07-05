using Domain.Entity;

namespace Application.DTO.ConsumerUser;

public class ConsumerUserAuthResponseDto
{
    public string? AccessToken { get; set; }
    public UserData? UserData { get; set; }
}