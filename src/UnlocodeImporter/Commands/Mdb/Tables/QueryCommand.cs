using System.Text.Json;

using Scsl.Unlocode.Core.Diagnostics;
using Scsl.Unlocode.Core.Query;
using Scsl.Unlocode.Infrastructure.Mdb.Metadata;

using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Json;

using UnlocodeImporter.Commands.Mdb.Tables.Settings;
using UnlocodeImporter.Diagnostics;
using UnlocodeImporter.Presentation;
using UnlocodeImporter.Presentation.Enums;
using UnlocodeImporter.Presentation.Factory;

namespace UnlocodeImporter.Commands.Mdb.Tables;

public class QueryCommand : Command<QuerySettings>
{
    private readonly ITableRenderer _renderer = new SpectreTableRenderer();

    public override int Execute(CommandContext context, QuerySettings settings, CancellationToken cancellationToken)
    {
        var diagnostics = new ConsoleDiagnosticsSink(settings.Verbose);

        try
        {
            using (new DiagnosticsScope(
                       diagnostics!,
                       DiagnosticsEvents.MdbQueryStart,
                       DiagnosticsEvents.MdbQueryCompleted,
                       "Executing MDB Query command"))
            {
                var reader = new MdbMetadataReader(diagnostics);

                var request = new MdbQueryRequest
                {
                    MdbPath = settings.FilePath,
                    Sql = settings.Sql,
                    Table = settings.Table,
                    Select = settings.Select,
                    Where = settings.Where,
                    Top = settings.Top
                };

                var results = reader.ExecuteQuery(request);

                if (settings.Json)
                {
                    var json = JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true });

                    AnsiConsole.Write(new JsonText(json));
                    return 0;
                }

                var renderOptions = TableRenderOptionsFactory.From(settings);

                _renderer.Render(results, style: TableStyle.MySql, options: renderOptions, headerFormatter:
                    TableRendererDefaults.MysqlHeader, valueFormatter: TableRendererDefaults.MySqlValueFormatter);

                return 0;
            }
        }
        catch (Exception ex)
        {
            return CommandExceptionHandler.Handle(ex, diagnostics, settings.Verbose);
        }
    }
}