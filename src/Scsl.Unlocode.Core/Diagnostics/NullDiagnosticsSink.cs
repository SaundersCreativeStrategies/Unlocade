namespace Scsl.Unlocode.Core.Diagnostics;

public sealed class NullDiagnosticsSink : IDiagnosticsSink
{
    public static readonly IDiagnosticsSink Instance = new NullDiagnosticsSink();

    private NullDiagnosticsSink() { }

    public void LogInfo(DiagnosticsEventId eventId, string message)  { }

    public void LogWarn(DiagnosticsEventId eventId, string message)  { }

    public void LogError(DiagnosticsEventId eventId, string message, Exception? exception = null) { }
}