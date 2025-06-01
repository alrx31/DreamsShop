namespace Domain.Entity;

public class UserDream
{
    public required Guid UserId { get; set; }
    public required Guid DreamId { get; set; }
    public Dream? Dream { get; set; }
}