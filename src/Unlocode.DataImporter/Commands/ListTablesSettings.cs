using System.ComponentModel;

using Spectre.Console;
using Spectre.Console.Cli;

namespace Unlocode.DataImporter.Commands;

public sealed class ListTablesSettings : GlobalSettings
{
    [CommandOption("--json")]
    [Description("Output results as JSON")]
    public bool Json { get; init; }
}