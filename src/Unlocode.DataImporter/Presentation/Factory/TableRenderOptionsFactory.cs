using Unlocode.DataImporter.Commands;
using Unlocode.DataImporter.Presentation.Options;

namespace Unlocode.DataImporter.Presentation.Factory;

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