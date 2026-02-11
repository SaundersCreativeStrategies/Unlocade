using System.ComponentModel;

using Spectre.Console;
using Spectre.Console.Cli;

namespace UnlocodeImporter.Commands;

public abstract class GlobalSettings : CommandSettings
{
    [CommandOption("-v | --verbose")]
    [Description("Enable verbose diagnostics output")]
    public bool Verbose { get; init; }
}