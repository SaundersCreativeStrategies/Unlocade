namespace Scsl.Unlocode.Infrastructure.Diagnostics;

public interface IDiagnosticsSink
{
    void LogInfo(string message);
    void LogWarn(string message);
    void LogError(string message);
}