using Scsl.Unlocode.Core.Diagnostics;

using Spectre.Console;

namespace UnlocodeImporter.Diagnostics;

internal static class SpectreDiagnosticsFormatter
{
    public static string Format(DiagnosticsRecord record)
    {
        var time = record.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");

        var levelText = record.Level switch
        {
            DiagnosticsLevel.Info => "[blue][[INFO]][/]",
            DiagnosticsLevel.Warning => "[yellow][[WARNING]][/]",
            DiagnosticsLevel.Error => "[red][[ERROR]][/]",
            _ => "[[LOG]]"
        };

        var message = Markup.Escape(record.Message);

        if (record.Exception is not null)
        {
            message +=
                $" [grey]({Markup.Escape(record.Exception.GetType().Name)}: " +
                $"{Markup.Escape(record.Exception.Message)})[/]";
        }

        return $"[grey][[{time}]][/]" +
               $" {levelText} " +
               $"[cyan]{record.EventId.Name}[/]" +
               $"[grey]({record.EventId.Id})[/] " +
               message;
    }
}