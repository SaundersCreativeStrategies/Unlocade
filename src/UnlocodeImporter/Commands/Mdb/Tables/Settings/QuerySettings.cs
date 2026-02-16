using System.ComponentModel;

using Spectre.Console;
using Spectre.Console.Cli;

namespace UnlocodeImporter.Commands.Mdb.Tables.Settings;

public sealed class QuerySettings : TableSettings
{
    [CommandOption("-t|--table")]
    [Description("Table name to query")]
    public required string Table { get; init; }

    [CommandOption("--select <COLUMNS>")]
    [Description("Comma-separated list of columns to select")]
    public string? Select { get; init; }

    [CommandOption("--where <EXPRESSION>")]
    [Description("WHERE clause expression (without the WHERE keyword)")]
    public string? Where { get; init; }

    [CommandOption("--top <COUNT>")]
    [Description("Limits number of rows returned")]
    public int? Top { get; init; }

    [CommandOption("--sql <SQL>")]
    [Description("RAW SQL query (overriders --table, --select, --where, --top")]
    public string? Sql { get; init; }

    [CommandOption("--json")]
    [Description("Output result as JSON")]
    public bool Json { get; init; }

    public override ValidationResult Validate()
    {
        var baseResult = base.Validate();
        if (!baseResult.Successful)
            return baseResult;

        if (string.IsNullOrWhiteSpace(Sql) && string.IsNullOrWhiteSpace(Table))
        {
            return ValidationResult.Error("You must specify either --sql or --table.");
        }

        if (Top is <= 0)
        {
            return ValidationResult.Error("--top must be greater than zero.");
        }

        return ValidationResult.Success();
    }
}