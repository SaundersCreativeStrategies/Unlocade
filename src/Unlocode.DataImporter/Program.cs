using Spectre.Console.Cli;

using Unlocode.DataImporter.Commands;

var app = new CommandApp();

app.Configure(config =>
{
    config.SetApplicationName("Unlocode Importer");

    config.AddBranch("tables", tables =>
    {
        tables.AddCommand<ListTablesCommand>("list")
            .WithDescription("Display all table names available in the MDB file");
    });
});

return app.Run(args);