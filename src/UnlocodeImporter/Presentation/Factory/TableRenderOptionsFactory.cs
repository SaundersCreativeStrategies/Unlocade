using UnlocodeImporter.Commands;
using UnlocodeImporter.Presentation.Options;

namespace UnlocodeImporter.Presentation.Factory;

public sealed class TableRenderOptionsFactory
{
    public static TableRenderOptions From(GlobalSettings settings)
    {
        return settings.MaxWidth is null
            ? TableRenderOptions.Default
            : new TableRenderOptions
            {
                MaxColumnWidth = settings.MaxWidth.Value,
                TruncateMode =  settings.TruncateMode
            };
    }
}