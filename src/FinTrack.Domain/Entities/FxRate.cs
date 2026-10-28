using FinTrack.Domain.Common;

namespace FinTrack.Domain.Entities;

public class FxRate : Entity
{
    public required string Base { get; init; }
    public required string Quote { get; init; }
    public decimal Rate { get; init; }
    public DateOnly Date { get; init; }
    public required string Source { get; init; }
}
