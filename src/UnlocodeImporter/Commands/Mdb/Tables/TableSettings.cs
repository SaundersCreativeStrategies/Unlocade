using System.ComponentModel;

using Scsl.Unlocode.Infrastructure.Mdb;

using Spectre.Console;
using Spectre.Console.Cli;

using UnlocodeImporter.Presentation.Enums;

namespace UnlocodeImporter.Commands.Mdb.Tables;

public abstract class TableSettings : GlobalSettings
{
    private static readonly HashSet<string> ValidAccessExtensions =
        new(StringComparer.OrdinalIgnoreCase) { ".mdb", ".accdb", ".mde", ".accde" };

    [CommandOption("-f |--file <MDB_FILE>")]
    [Description("Path to the MDB database file")]
    public string FilePath { get; init; } = string.Empty;

    [CommandOption("--max-width <WIDTH>")]
    [Description("Maximum column display width")]
    public int? MaxWidth { get; init; }

    [CommandOption("--truncate-mode <MODE>")]
    [Description("Truncation mode: strict | friendly (default: friendly)")]
    public TruncateMode TruncateMode { get; init; } = TruncateMode.Friendly;

    public override ValidationResult Validate()
    {
        // Provider availability check (AFTER extension)
        if (!AccessProviderAvailability.IsAceInstalled())
        {
            return ValidationResult.Error(
                "This Access database requires the Microsoft ACE OLE DB provider, " +
                "but it is not installed on this machine.\n\n" +
                "Install: https://www.microsoft.com/en-us/download/details.aspx?id=54920");
        }

        var extension = Path.GetExtension(FilePath);
        if (!ValidAccessExtensions.Contains(extension))
            return ValidationResult.Error($"Invalid Access database file extension '{extension}'. " +
                                          $"Supported extensions: .mdb, .accdb, .mde, .accde");

        if (string.IsNullOrWhiteSpace(FilePath))
            return ValidationResult.Error("You must specify --file <MDB_FILE>.");

        if (!File.Exists(FilePath))
            return ValidationResult.Error("MDB file not found: {FilePath}");

        if(MaxWidth is <= 0)
            return ValidationResult.Error("--max-width must greater than zero.");

        return ValidationResult.Success();
    }
}