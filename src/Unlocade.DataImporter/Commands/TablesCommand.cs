using System.Text.Json;

using Scsl.Unlocode.Infrastructure.Mdb;

using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Json;

namespace Unlocade.DataImporter.Commands;

public sealed class TablesCommand : Command<TablesSettings>
{
    public override int Execute(CommandContext context, TablesSettings settings, CancellationToken cancellationToken)
    {
        var reader = new MdbSchemaReader();
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
            var renderer = new Presentation.MdbTableRenderer();
            renderer.Render(tables);
        }
        return 0;
    }
}