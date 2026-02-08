using UnlocodeImporter.Presentation.Enums;

namespace UnlocodeImporter.Presentation.Options;

public sealed class TableRenderOptions
{
    public static readonly TableRenderOptions Default = new();

    public TruncateMode TruncateMode { get; init; } = TruncateMode.Friendly;

    /// <summary>
    /// Maximum number of characters per column.
    /// Null = unlimited (MySQL default behavior when not set).
    /// </summary>
    public int? MaxColumnWidth { get; init; }

    /// <summary>
    /// Truncation suffix (MySQL uses nothing, but ellipsis is more readable)
    /// </summary>
    public string TruncationSuffix { get; init; } = "…";
}