using Scsl.Unlocode.Core.Diagnostics;

using Spectre.Console;

namespace Unlocode.DataImporter.Diagnostics;

public sealed class ConsoleDiagnosticsSink : IDiagnosticsSink
{
    private readonly bool _verbose;

    public ConsoleDiagnosticsSink(bool verbose)
    {
        _verbose = verbose;
    }

    public void LogInfo(DiagnosticsEventId eventId, string message)
    {
        if (_verbose)
            Write(DiagnosticsRecord.Info(eventId, message));
    }

    public void LogWarn(DiagnosticsEventId eventId, string message)
    {
        Write(DiagnosticsRecord.Warning(eventId, message));
    }

    public void LogError(DiagnosticsEventId eventId, string message, Exception? exception = null)
    {
        Write(DiagnosticsRecord.Error(eventId, message, exception));
    }

    private static void Write(DiagnosticsRecord record)
    {
        var output = SpectreDiagnosticsFormatter.Format(record);
        AnsiConsole.MarkupLine(output);
    }
}