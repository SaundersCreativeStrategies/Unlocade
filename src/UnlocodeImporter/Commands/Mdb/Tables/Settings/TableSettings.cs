using System.ComponentModel;

using Scsl.Unlocode.Infrastructure.Mdb;

using Spectre.Console;
using Spectre.Console.Cli;

using UnlocodeImporter.Presentation.Enums;

namespace UnlocodeImporter.Commands.Mdb.Tables.Settings;

public abstract class TableSettings : GlobalSettings
{
    [CommandOption("--max-width <WIDTH>")]
    [Description("Maximum column display width")]
    public int? MaxWidth { get; init; }

    [CommandOption("--truncate-mode <MODE>")]
    [Description("Truncation mode: strict | friendly (default: friendly)")]
    public TruncateMode TruncateMode { get; init; } = TruncateMode.Friendly;

    public override ValidationResult Validate()
    {
        var baseResult = base.Validate();
        if (!baseResult.Successful)
            return baseResult;

        if(MaxWidth is <= 0)
            return ValidationResult.Error("--max-width must greater than zero.");

        return ValidationResult.Success();
    }
}