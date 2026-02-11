using System.Diagnostics;
using System.Reflection;

using Microsoft.IdentityModel.Protocols.OpenIdConnect;

using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Help;

using UnlocodeImporter.Commands;
using UnlocodeImporter.Commands.Table;

var app = new CommandApp();
var assembly = Assembly.GetExecutingAssembly();
var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

app.Configure(config =>
{
    config.SetApplicationName("UnlocodeImporter");
    config.SetApplicationVersion(fileVersionInfo.FileVersion!);
    config.ValidateExamples(); // Verify all WithExample calls are valid

    // Configure parsing behavior
    config.Settings.CaseSensitivity = CaseSensitivity.None;
    config.Settings.StrictParsing = true;

    // Customize help text styling
    config.Settings.HelpProviderStyles = new()
    {
        Description = new()
        {
            Header = "bold blue"
        },
        Options = new ()
        {
            RequiredOption = "bold red",
            DefaultValue = "dim"
        },
        Arguments = new ()
        {
            RequiredArgument = "bold green",
            OptionalArgument = "dim green"
        },
        Commands =  new ()
        {
            RequiredArgument =  "bold yellow",
        },
        Examples =  new ()
        {
            Arguments = "bold green"
        }
    };

#if Debug
    config.PropagateExceptions(); // Get full stack trace

#endif

    config.AddBranch("mdb", mdb =>
    {
        mdb.SetDescription("MDB table operations");

        mdb.AddBranch("table", tables =>
        {
            tables.SetDescription("");

            tables.AddCommand<ListTablesCommand>("list")
                .WithDescription("Display all table names available in the MDB file")
                .WithExample("mdb", "table", "list", "--file FILE");

            tables.AddCommand<TableSchemaCommand>("schema")
                .WithDescription("Show schema for a table")
                .WithExample("mdb", "table", "schema", "--file FILE", "--table TABLE");
        });
    });

    // (future)
    // config.AddCommand<ImportCommand>("import");
});

return app.Run(args);