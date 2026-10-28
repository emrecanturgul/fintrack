using FinTrack.Domain.Common;
using FinTrack.Domain.Enums;

namespace FinTrack.Domain.Entities;

public class Transaction : Entity
{
    public Guid UserId { get; init; }
    public Guid AccountId { get; init; }
    public DateOnly Date { get; set; }
    public required string Description { get; set; }
    public decimal Amount { get; set; }
    public required string Currency { get; set; }
    public decimal AmountInBase { get; set; }
    public decimal FxRate { get; set; }
    public Guid? CategoryId { get; set; }
    public CategorySource? CategorySource { get; set; }
    public string? Merchant { get; set; }
    public string? Note { get; set; }
    public required string DedupHash { get; init; }
    public Guid? ImportJobId { get; init; }
}
