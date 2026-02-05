using Scsl.Unlocode.Core.Diagnostics;

using Spectre.Console;

namespace Unlocode.DataImporter.Diagnostics;

public sealed class ConsoleDiagnostics : IDiagnosticsSink
{
    private readonly bool _verbose;

    public ConsoleDiagnostics(bool verbose)
    {
        _verbose = verbose;
    }

    public void LogInfo(DiagnosticsEventId eventId, string message)
    {
        if (_verbose)
            Write(new DiagnosticsRecord(DateTimeOffset.Now, DiagnosticsLevel.Info, eventId, message));
    }

    public void LogWarn(DiagnosticsEventId eventId, string message)
    {
        Write(new DiagnosticsRecord(
            DateTimeOffset.Now, DiagnosticsLevel.Warning, eventId, message));
    }

    public void LogError(DiagnosticsEventId eventId, string message, Exception? exception = null)
    {
        Write(new DiagnosticsRecord(
            DateTimeOffset.Now, DiagnosticsLevel.Error, eventId, message, exception));
    }

    private static void Write(DiagnosticsRecord record)
    {
        var output = SpectreDiagnosticsFormatter.Format(record);
        AnsiConsole.MarkupLine(output);
    }
}