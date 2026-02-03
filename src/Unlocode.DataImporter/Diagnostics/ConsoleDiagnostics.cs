using Scsl.Unlocode.Core.Diagnostics;

using Spectre.Console;

namespace Unlocode.DataImporter.Diagnostics;

public class ConsoleDiagnostics : IDiagnosticsSink
{
    private readonly bool _verbose;

    public ConsoleDiagnostics(bool verbose)
    {
        _verbose = verbose;
    }
    public void LogInfo(string message)
    {
        if(_verbose)
            AnsiConsole.MarkupLine($"[grey][Info]:[/] {message}");
    }

    public void LogWarn(string message)
    {
        if(_verbose)
            AnsiConsole.MarkupLine($"[yellow][Warning]:[/] {message}");
    }

    public void LogError(string message, Exception? ex = null)
    {
        AnsiConsole.MarkupLine($"[red][Error]:[/] {message}");

        if (_verbose && ex != null)
        {
            AnsiConsole.WriteException(ex);
        }
    }
}