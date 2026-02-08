using System.ComponentModel;

using Spectre.Console;
using Spectre.Console.Cli;

namespace UnlocodeImporter.Commands.Tables;

public class TableSchemaSettings : GlobalSettings
{
    [CommandOption("-t|--table <TABLE>")]
    [Description("Table name (quote if it contains spaces, e.g. \"Example Table\")")]
    public string Table { get; init; } = string.Empty;

    [CommandOption("--json")]
    [Description("Output results as JSON")]
    public bool Json { get; init; }

    public override ValidationResult Validate()
    {
        var baseResult = base.Validate();
        if (!baseResult.Successful)
            return baseResult;

        if (string.IsNullOrWhiteSpace(Table))
            return ValidationResult.Error("You must specify --table <TABLE>.");

        if (Table.Contains(' ') && Table.Any(c => c is '“' or '”' or '‘' or '’'))
        {
            return ValidationResult.Error(
                "Invalid quotes detected. Use plain ASCII quotes: " +
                "--table \"example table\" or --table 'example table'.");
        }

        return ValidationResult.Success();
    }
}