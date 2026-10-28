using FinTrack.Domain.Common;
using FinTrack.Domain.Enums;

namespace FinTrack.Domain.Entities;

public class Account : Entity
{
    public Guid UserId { get; init; }
    public required string Name { get; set; }
    public string? BankName { get; set; }
    public AccountType Type { get; set; }
    public required string Currency { get; set; }
}
