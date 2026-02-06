using System.ComponentModel;

using Scsl.Unlocode.Infrastructure.Mdb;

using Spectre.Console;
using Spectre.Console.Cli;

using Unlocode.DataImporter.Presentation.Enums;

namespace Unlocode.DataImporter.Commands;

public abstract class GlobalSettings : CommandSettings
{
    private static readonly HashSet<string> ValidAccessExtensions =
        new(StringComparer.OrdinalIgnoreCase) { ".mdb", ".accdb", ".mde", ".accde" };

    [CommandOption("-f|--file <MDB_PATH>")]
    [Description("Path to the MDB database file")]
    public string FilePath { get; init; } = string.Empty;

    [CommandOption("-c|--config <CONFIG_PATH>")]
    [Description("External database connection config (JSON or YAML)")]
    public string? ConfigPath { get; init; }

    [CommandOption("--dry-run")]
    [Description("Run without writing to the external database")]
    public bool DryRun { get; init; }

    [CommandOption("--verbose")]
    [Description("Enable verbose diagnostics output")]
    public bool Verbose { get; init; }

    [CommandOption("--max-width <WIDTH>")]
    [Description("Maximum column display width")]
    public int? MaxWidth { get; init; }

    [CommandOption("--truncate-mode <MODE>")]
    [Description("Truncation mode: strict | friendly (default: friendly)")]
    public TruncateMode TruncateMode { get; init; } = TruncateMode.Friendly;

    public override ValidationResult Validate()
    {
        if (string.IsNullOrWhiteSpace(FilePath))
            return ValidationResult.Error("You must specify --file <MDB_PATH>.");

        if (!File.Exists(FilePath))
            return ValidationResult.Error("MDB file not found: {FilePath}");

        if (!string.IsNullOrWhiteSpace(ConfigPath) && !File.Exists(ConfigPath))
            return ValidationResult.Error("Config file not found: {ConfigPath}");

        if(MaxWidth is <= 0)
            return ValidationResult.Error("--max-width must greater than zero.");

        var extension = Path.GetExtension(FilePath);
        if (!ValidAccessExtensions.Contains(extension))
            return ValidationResult.Error($"Invalid Access database file extension '{extension}'. " +
                $"Supported extensions: .mdb, .accdb, .mde, .accde");

        // Provider availability check (AFTER extension)
        if (!AccessProviderAvailability.IsAceInstalled())
        {
            return ValidationResult.Error(
                "This Access database requires the Microsoft ACE OLE DB provider, " +
                "but it is not installed on this manchine.\n\n" +
                "Install: https://www.microsoft.com/en-us/download/details.aspx?id=54920");
        }

        return ValidationResult.Success();
    }
}