namespace Scsl.Unlocode.Core.Diagnostics;

public interface IDiagnosticsSink
{
    void LogInfo(string message);
    void LogWarn(string message);
    void LogError(string message, Exception? ex = null);
}