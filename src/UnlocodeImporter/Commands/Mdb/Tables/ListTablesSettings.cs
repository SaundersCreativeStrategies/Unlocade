using System.ComponentModel;

using Spectre.Console.Cli;

namespace UnlocodeImporter.Commands.Mdb.Tables;

public sealed class ListTablesSettings : TableSettings
{
    [CommandOption("--json")]
    [Description("Output results as JSON")]
    public bool Json { get; init; }
}