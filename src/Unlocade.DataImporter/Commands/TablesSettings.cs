using System.ComponentModel;

using Spectre.Console;
using Spectre.Console.Cli;

namespace Unlocade.DataImporter.Commands;

public class TablesSettings : CommandSettings
{
    [CommandOption("-f|--file <MDB_PATH>")]
    [Description("Path to the MDB File")]
    public string FilePath { get; init; } = string.Empty;

    [CommandOption("--json")]
    [Description("Output results as JSON")]
    public bool Json { get; init; }

    public override ValidationResult Validate()
    {
        if (string.IsNullOrWhiteSpace(FilePath))
            return ValidationResult.Error("You must specify --file <MDB_PATH>.");
        if (!File.Exists(FilePath))
            return ValidationResult.Error("MDB file not found: {FilePath}");

        return ValidationResult.Success();
    }
}