namespace BLL.DTO;

public class UpdateProviderDTO
{
    public  RegisterProviderDTO ProviderDTO { get; set; }
    public Guid RequesterId { get; set; }
    public string Password { get; set; }
}