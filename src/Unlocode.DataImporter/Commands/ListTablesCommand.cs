using System.Text.Json;

using Scsl.Unlocode.Infrastructure.Mdb.Metadata;

using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Json;

using Unlocode.DataImporter.Presentation;

namespace Unlocode.DataImporter.Commands;

public sealed class ListTablesCommand : Command<ListTablesSettings>
{
    private readonly ITableRenderer _renderer = new SpectreTableRenderer();

    public override int Execute(CommandContext context, ListTablesSettings settings, CancellationToken
            cancellationToken)
    {
        var reader = new MdbMetadataReader();
        var tables = reader.GetTables(settings.FilePath);

        if (settings.Json)
        {
            var json = JsonSerializer.Serialize(tables,
                new JsonSerializerOptions { WriteIndented = true }
            );

            AnsiConsole.Write(new JsonText(json));
        }
        else
        {
            _renderer.Render(
                tables,
                style: TableStyle.MySql,
                propertyFilter: TableRendererDefaults.SimpleTypesOnly,
                headerFormatter: TableRendererDefaults.MysqlHeader,
                valueFormatter: TableRendererDefaults.MySqlValueFormatter);
        }
        return 0;
    }
}