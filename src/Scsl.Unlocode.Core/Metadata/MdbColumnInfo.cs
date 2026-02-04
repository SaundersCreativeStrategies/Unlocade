namespace Scsl.Unlocode.Core.Metadata;

public sealed class MdbColumnInfo
{
    public string Name { get; init; } = default!;
    public string DataType { get; init; } = default!;
    public int? Size { get; init; }
    public bool IsNullable { get; init; }

    public bool IsPrimaryKey { get; set; }
    public bool IsIndexed { get; set; }
    public bool IsUnique { get; set; }
    public bool IsForeignKey { get; set; }
    public string? ReferencesTable { get; set; }
    public string? ReferencesColumn { get; set; }

}