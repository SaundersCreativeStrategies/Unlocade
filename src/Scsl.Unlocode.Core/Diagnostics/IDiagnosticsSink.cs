namespace Scsl.Unlocode.Core.Diagnostics;

public interface IDiagnosticsSink
{
    void LogInfo(DiagnosticsEventId eventId, string message);
    void LogWarn(DiagnosticsEventId eventId, string message);
    void LogError(DiagnosticsEventId eventId, string message, Exception? exception = null);
}