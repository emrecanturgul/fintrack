using FinTrack.Domain.Common;
using FinTrack.Domain.Enums;

namespace FinTrack.Domain.Entities;

public class CategoryRule : Entity
{
    public Guid UserId { get; init; }
    public required string Pattern { get; set; }
    public Guid CategoryId { get; set; }
    public int Priority { get; set; }
    public RuleSource CreatedFrom { get; init; }
}
