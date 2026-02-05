using Spectre.Console.Cli;

using Unlocode.DataImporter.Commands;
using Unlocode.DataImporter.Commands.Tables;

var app = new CommandApp();

app.Configure(config =>
{
    config.SetApplicationName("Unlocode Importer");
    config.SetApplicationVersion(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString()!);

    config.AddBranch("tables", tables =>
    {
        tables.SetDescription("MDB table operations");

        tables.AddCommand<ListTablesCommand>("list")
            .WithDescription("Display all table names available in the MDB file");

        tables.AddCommand<TableSchemaCommand>("schema")
            .WithDescription("Show schema for a table");
    });

    // (future)
    // config.AddCommand<ImportCommand>("import");

    // ─────────────────────────
    // Global behavior
    // ───────────────────────
    config.PropagateExceptions();
    config.ValidateExamples();
});

return app.Run(args);