using System.Diagnostics;
using System.Reflection;

using Spectre.Console.Cli;

using UnlocodeImporter.Commands.Mdb.Tables;

var app = new CommandApp();
var assembly = Assembly.GetExecutingAssembly();
var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

app.Configure(config =>
{
    config.SetApplicationName("UnlocodeImporter");
    config.SetApplicationVersion(fileVersionInfo.ProductVersion!);

    // Configure parsing behavior
    config.Settings.CaseSensitivity = CaseSensitivity.None;
    config.Settings.StrictParsing = true;
    config.Settings.MaximumIndirectExamples = 0;
    config.Settings.ShowOptionDefaultValues = true;

    // Verify all WithExample calls are valid
    //config.ValidateExamples();

#if Debug
    config.PropagateExceptions(); // Get full stack trace
#endif

    config.AddBranch("mdb", mdb =>
    {
        mdb.SetDescription("Provide tools that list available tables and inspect detailed schema metadata in an MDB database.");

        mdb.AddBranch("table", tables =>
        {
            tables.AddCommand<ListTablesCommand>("list")
                .WithDescription("List all user-defined tables available in the MDB file.")
                .WithExample("mdb", "table", "list", "--file", "data.mdb")
                .WithExample("mdb", "table", "list", "--file", "data.mdb", "--json")
                .WithExample("mdb", "table", "list", "--file", "data.mdb", "--verbose")
                .WithExample("mdb", "table", "list", "--file", "data.mdb", "--max-width", "40",
                    "--truncate-mode", "strict");

            tables.AddCommand<TableSchemaCommand>("schema")
                .WithDescription("Show the structure of a table, including its columns and how the data is defined.")
                .WithExample("mdb", "table", "schema", "--file", "data.mdb", "--table", "tableName")
                .WithExample("mdb", "table", "schema", "--file", "data.mdb", "--table", "tableName", "--json")
                .WithExample("mdb", "table", "schema", "--file", "data.mdb", "--table", "tableName", "--verbose")
                .WithExample("mdb", "table", "schema", "--file", "data.mdb", "--table", "tableName", "--max-width",
                    "40", "--truncated-mode", "strict");
        });
    });

    // (future)
    // config.AddCommand<ImportCommand>("import");
});

return app.Run(args);