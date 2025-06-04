namespace Domain.Entity;

public class UserDream
{
    public required Guid UserId { get; init; }
    public required Guid DreamId { get; init; }
    
    public Dream? Dream { get; init; }
}