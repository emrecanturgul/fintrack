using FinTrack.Domain.Common;
using FinTrack.Domain.Enums;

namespace FinTrack.Domain.Entities;

public class Subscription : Entity
{
    public Guid UserId { get; init; }
    public required string Merchant { get; set; }
    public decimal Amount { get; set; }
    public required string Currency { get; set; }
    public SubscriptionPeriod Period { get; set; }
    public DateOnly NextExpectedDate { get; set; }
    public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Detected;
}
