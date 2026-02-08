using System.Text.Json;

using Scsl.Unlocode.Core.Diagnostics;
using Scsl.Unlocode.Core.Metadata;
using Scsl.Unlocode.Infrastructure.Mdb.Metadata;
using Scsl.Unlocode.Infrastructure.Mdb.Resolution;

using Spectre.Console;
using Spectre.Console.Cli;

using UnlocodeImporter.Presentation.Enums;
using UnlocodeImporter.Diagnostics;
using UnlocodeImporter.Presentation;
using UnlocodeImporter.Presentation.Factory;

namespace UnlocodeImporter.Commands.Tables;

public class TableSchemaCommand : Command<TableSchemaSettings>
{
    private readonly ITableRenderer _renderer = new SpectreTableRenderer();

    public override int Execute(CommandContext context, TableSchemaSettings settings,
        CancellationToken cancellationToken)
    {
        var diagnostics = new ConsoleDiagnosticsSink(settings.Verbose);

        try
        {
            var reader = new MdbMetadataReader(diagnostics);

            // ── MDB schema read (timed)
            IReadOnlyList<MdbColumnInfo> schema;
            using (new DiagnosticsScope(
                       diagnostics, DiagnosticsEvents.MdbReadSchemaStart,
                       DiagnosticsEvents.MdbReadSchemaCompleted,
                       $"Reading schema from table '{settings.Table}'"))
            {
                // Get table list first
                var tables = reader.GetTables(settings.FilePath);

                // Resolve table name safely
                var resolvedTable = TableNameResolver.Resolve(
                    settings.Table, tables, diagnostics);

                schema = reader.GetTableSchema(settings.FilePath, resolvedTable);
            }

            diagnostics.LogInfo(DiagnosticsEvents.MdbSchemaColumnsFound, $"Columns found: {schema.Count}");

            // ── JSON output path
            if (settings.Json)
            {
                diagnostics.LogInfo(DiagnosticsEvents.CliJsonOutput, "Writing schema output as JSON");

                var json = JsonSerializer.Serialize(schema, new JsonSerializerOptions { WriteIndented = true });
                AnsiConsole.Write(json);
                return 0;
            }

            // ── Table rendering path
            var renderOptions = TableRenderOptionsFactory.From(settings);
            if (settings.Verbose && renderOptions.MaxColumnWidth is not null)
            {
                diagnostics.LogInfo(
                    DiagnosticsEvents.CliTruncationEnabled,
                    $"Column truncation enabled (max-width={renderOptions.MaxColumnWidth}, mode={renderOptions.TruncateMode})");
            }

            using (new DiagnosticsScope(
                       diagnostics,
                       DiagnosticsEvents.CliRenderStart,
                       DiagnosticsEvents.CliRenderCompleted,
                       "Rendering schema table"))
            {
                _renderer.Render(
                    schema,
                    style: TableStyle.MySql,
                    options: renderOptions,
                    propertyFilter: TableRendererDefaults.SimpleTypesOnly,
                    valueFormatter: TableRendererDefaults.MySqlValueFormatter);
            }

            return 0;
        }
        catch (Exception ex)
        {
            return CommandExceptionHandler.Handle(ex, diagnostics, settings.Verbose);
        }
        finally
        {
            diagnostics.WritePerformanceSummary();
        }
    }
}