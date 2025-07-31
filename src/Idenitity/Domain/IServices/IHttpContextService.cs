namespace Domain.IServices;

public interface IHttpContextService
{
    Guid? GetCurrentUserId();
}