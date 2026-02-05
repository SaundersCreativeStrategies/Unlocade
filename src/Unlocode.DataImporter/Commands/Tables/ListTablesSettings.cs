using System.ComponentModel;

using Spectre.Console.Cli;

namespace Unlocode.DataImporter.Commands.Tables;

public sealed class ListTablesSettings : GlobalSettings
{
    [CommandOption("--json")]
    [Description("Output results as JSON")]
    public bool Json { get; init; }
}