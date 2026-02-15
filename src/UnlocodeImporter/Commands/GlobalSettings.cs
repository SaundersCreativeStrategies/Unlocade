using System.ComponentModel;

using Scsl.Unlocode.Infrastructure.Mdb;

using Spectre.Console;
using Spectre.Console.Cli;

namespace UnlocodeImporter.Commands;

public abstract class GlobalSettings : CommandSettings
{
    private static readonly HashSet<string> ValidAccessExtensions =
        new(StringComparer.OrdinalIgnoreCase) { ".mdb", ".accdb", ".mde", ".accde" };

    [CommandOption("-f|--file <MDB_FILE>")]
    [Description("Path to the MDB database file")]
    public string FilePath { get; init; } = string.Empty;

    [CommandOption("-v|--verbose")]
    [Description("Enable verbose diagnostics output")]
    public bool Verbose { get; init; }

    public override ValidationResult Validate()
    {
        if (string.IsNullOrWhiteSpace(FilePath))
            return ValidationResult.Error("You must specify --file <MDB_FILE>.");

        if (!File.Exists(FilePath))
            return ValidationResult.Error("MDB file not found: {FilePath}");

        var extension = Path.GetExtension(FilePath);
        if (!ValidAccessExtensions.Contains(extension))
            return ValidationResult.Error($"Invalid Access database file extension '{extension}'. " +
                                          $"Supported extensions: .mdb, .accdb, .mde, .accde");

        // Provider availability check (AFTER extension)
        if (!AccessProviderAvailability.IsAceInstalled())
        {
            return ValidationResult.Error(
                "This Access database requires the Microsoft ACE OLE DB provider, " +
                "but it is not installed on this machine.\n\n" +
                "Install: https://www.microsoft.com/en-us/download/details.aspx?id=54920");
        }

        return ValidationResult.Success();
    }
}