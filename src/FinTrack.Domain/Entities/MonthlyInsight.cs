using FinTrack.Domain.Common;

namespace FinTrack.Domain.Entities;

public class MonthlyInsight : Entity
{
    public Guid UserId { get; init; }
    public DateOnly Month { get; init; }
    public required string SummaryText { get; set; }
    public required string Model { get; set; }
    public DateTimeOffset CreatedAt { get; init; }
}
