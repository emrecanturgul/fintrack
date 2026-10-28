using FinTrack.Domain.Common;

namespace FinTrack.Domain.Entities;

public class ApiKey : Entity
{
    public Guid UserId { get; init; }
    public required string Name { get; set; }
    public required string Prefix { get; init; }
    public required string KeyHash { get; init; }
    public List<string> Scopes { get; set; } = [];
    public DateTimeOffset? LastUsedAt { get; set; }
    public DateTimeOffset? RevokedAt { get; set; }
}
