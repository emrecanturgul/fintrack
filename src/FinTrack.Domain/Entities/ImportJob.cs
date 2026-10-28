using FinTrack.Domain.Common;
using FinTrack.Domain.Enums;

namespace FinTrack.Domain.Entities;

public class ImportJob : Entity
{
    public Guid UserId { get; init; }
    public Guid AccountId { get; init; }
    public required string FileName { get; init; }
    public required string FileHash { get; init; }
    public required string Format { get; set; }
    public ImportStatus Status { get; set; } = ImportStatus.Queued;
    public int TotalRows { get; set; }
    public int ImportedRows { get; set; }
    public int SkippedRows { get; set; }
    public string? Error { get; set; }
    public DateTimeOffset CreatedAt { get; init; }
}
