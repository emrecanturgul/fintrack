using FinTrack.Domain.Common;

namespace FinTrack.Domain.Entities;

public class Budget : Entity
{
    public Guid UserId { get; init; }
    public Guid CategoryId { get; set; }
    public decimal MonthlyLimit { get; set; }
    public required string Currency { get; set; }
    public int AlertAt { get; set; } = 80;
}
