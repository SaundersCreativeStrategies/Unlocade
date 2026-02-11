using System.ComponentModel;

using Spectre.Console;
using Spectre.Console.Cli;

namespace UnlocodeImporter.Commands.Mdb.Table;

public sealed class ListTablesSettings : TableSettings
{
    [CommandOption("--json")]
    [Description("Output results as JSON")]
    public bool Json { get; init; }
}