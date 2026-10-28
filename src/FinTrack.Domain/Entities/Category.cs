using FinTrack.Domain.Common;

namespace FinTrack.Domain.Entities;

public class Category : Entity
{
    public Guid? UserId { get; init; }
    public required string Name { get; set; }
    public Guid? ParentId { get; set; }
    public string? Icon { get; set; }
}
