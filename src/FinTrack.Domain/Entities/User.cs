using FinTrack.Domain.Common;

namespace FinTrack.Domain.Entities;

public class User : Entity
{
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required string DisplayName { get; set; }
    public string BaseCurrency { get; set; } = "TRY";
    public DateTimeOffset CreatedAt { get; init; }
}
