namespace Scsl.Unlocode.Infrastructure.Diagnostics;

public interface IDiagnosticsSink
{
    void Info(string message);
    void Warn(string message);
    void Error(string message);
}