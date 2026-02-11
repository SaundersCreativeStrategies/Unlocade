using System.ComponentModel;

using Spectre.Console;
using Spectre.Console.Cli;

namespace UnlocodeImporter.Commands.Import;

public abstract class ImportSettings : GlobalSettings
{
    [CommandOption("-c|--config <CONFIG_PATH>")]
    [Description("External database connection config (JSON or YAML)")]
    public string? ConfigPath { get; init; }

    [CommandOption("--dry-run")]
    [Description("Run without writing to the external database")]
    public bool DryRun { get; init; }

    public override ValidationResult Validate()
    {

        if (!string.IsNullOrWhiteSpace(ConfigPath) && !File.Exists(ConfigPath))
            return ValidationResult.Error("Config file not found: {ConfigPath}");

        return ValidationResult.Success();
    }
}