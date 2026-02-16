using System.Diagnostics;
using System.Reflection;

using Spectre.Console.Cli;

using UnlocodeImporter.Commands.Mdb.Tables;

try
{
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

        config.AddBranch("mdb", mdb =>
        {
            mdb.SetDescription("Explore and query tables in an MDB database.");

            mdb.AddBranch("table", tables =>
            {
                tables.SetDescription("List, inspect, and query tables in an MDB file.");

                tables.AddCommand<ListTablesCommand>("list")
                    .WithDescription("List all user-defined tables available in the MDB file.")
                    .WithExample("mdb", "table", "list", "--file", "data.mdb")
                    .WithExample("mdb", "table", "list", "--file", "data.mdb", "--json")
                    .WithExample("mdb", "table", "list", "--file", "data.mdb", "--verbose")
                    .WithExample("mdb", "table", "list", "--file", "data.mdb", "--max-width", "40",
                        "--truncate-mode", "strict");

                tables.AddCommand<TableSchemaCommand>("schema")
                    .WithDescription(
                        "Inspect a table’s schema, including columns, data types, keys, and relationships.")
                    .WithExample("mdb", "table", "schema", "--file", "data.mdb", "--table", "tableName")
                    .WithExample("mdb", "table", "schema", "--file", "data.mdb", "--table", "tableName", "--json")
                    .WithExample("mdb", "table", "schema", "--file", "data.mdb", "--table", "tableName", "--verbose")
                    .WithExample("mdb", "table", "schema", "--file", "data.mdb", "--table", "tableName", "--max-width",
                        "40", "--truncated-mode", "strict");

                tables.AddCommand<QueryCommand>("query")
                    .WithDescription("Execute a SELECT query against a table in the MDB database")
                    .WithExample(
                        "mdb", "table", "query",
                        "--file", "data.mdb",
                        "--table", "tableName",
                        "--top", "10")
                    .WithExample(
                        "mdb", "table", "query",
                        "--file", "data.mdb",
                        "--sql", "SELECT TOP 5 * FROM tableName")
                    .WithExample(
                        "mdb", "table", "query",
                        "--file", "data.mdb",
                        "--table", "tableName",
                        "--where", "Country = 'PH'");
            });
        });

        // (future)
        // config.AddCommand<ImportCommand>("import");

        // Verify all WithExample calls are valid
       // config.ValidateExamples();

#if Debug
    config.PropagateExceptions(); // Get full stack trace
#endif
    });

    return app.Run(args);
}
catch (Exception e)
{
    Console.Error.WriteLine("Fatal error:");
    Console.Error.WriteLine(e.Message);

    return -1;
}