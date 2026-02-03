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
            AnsiConsole.MarkupLine($"[grey][INFO][/]{eventId}: {message}");
    }

    public void LogWarn(DiagnosticsEventId eventId, string message)
    {
        if (_verbose)
            AnsiConsole.MarkupLine($"[yellow][WARNING][/]{eventId}: {message}");
    }

    public void LogError(DiagnosticsEventId eventId, string message, Exception? exception = null)
    {
        AnsiConsole.MarkupLine($"[red][ERROR][/]{eventId}: {message}");

        if (_verbose && exception != null)
            AnsiConsole.WriteException(exception);
    }
}