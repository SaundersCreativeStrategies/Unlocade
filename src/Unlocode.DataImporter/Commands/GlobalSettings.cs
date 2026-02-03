using System.ComponentModel;

using Spectre.Console;
using Spectre.Console.Cli;

namespace Unlocode.DataImporter.Commands;

public abstract class GlobalSettings : CommandSettings
{
    [CommandOption("-f|--file <MDB_PATH>")]
    [Description("Path to the MDB database file")]
    public string FilePath { get; init; } = string.Empty;

    [CommandOption("--verbose")]
    [Description("Enable verbose diagnostics output")]
    public bool Verbose { get; init; }

    public override ValidationResult Validate()
    {
        if (string.IsNullOrWhiteSpace(FilePath))
            return ValidationResult.Error("You must specify --file <MDB_PATH>.");
        if (!File.Exists(FilePath))
            return ValidationResult.Error("MDB file not found: {FilePath}");

        return ValidationResult.Success();
    }
}