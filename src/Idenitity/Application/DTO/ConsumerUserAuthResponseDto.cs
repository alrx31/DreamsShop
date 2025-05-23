using Domain.Entity;

namespace Application.DTO;

public class ConsumerUserAuthResponseDto
{
    public string AccessToken { get; set; }
    
    public ConsumerUserData UserData { get; set; }
}