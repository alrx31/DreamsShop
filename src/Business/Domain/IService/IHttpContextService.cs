namespace Domain.IService;

public interface IHttpContextService
{
    Guid? GetCurrentUserId();
}