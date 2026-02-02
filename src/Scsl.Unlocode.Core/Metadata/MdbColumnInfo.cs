namespace Scsl.Unlocode.Core.Metadata;

public sealed class MdbColumnInfo
{
    public string Name { get; init; } = default!;
    public string DataType { get; init; } = default!;
    public int? Size { get; init; }
    public bool IsNullable { get; init; }
}