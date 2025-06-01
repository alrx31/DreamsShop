using System.ComponentModel.DataAnnotations;

namespace Domain.Entity;

public class UserDream
{
    [Key]
    public required Guid Id { get; init; }
    public required Guid UserId { get; init; }
    public required Guid DreamId { get; init; }
    public Dream? Dream { get; init; }
}