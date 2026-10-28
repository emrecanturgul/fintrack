using FinTrack.Domain.Common;

namespace FinTrack.Domain.Entities;

public class RefreshToken : Entity
{
    public Guid UserId { get; init; }
    public required string TokenHash { get; init; }
    public DateTimeOffset ExpiresAt { get; init; }
    public DateTimeOffset? RevokedAt { get; set; }
    public Guid? ReplacedById { get; set; }
}
