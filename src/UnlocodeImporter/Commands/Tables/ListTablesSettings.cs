using System.ComponentModel;

using Spectre.Console.Cli;

namespace UnlocodeImporter.Commands.Tables;

public sealed class ListTablesSettings : GlobalSettings
{
    [CommandOption("--json")]
    [Description("Output results as JSON")]
    public bool Json { get; init; }
}