namespace Scsl.Unlocode.Core.Diagnostics;

public sealed class NullDiagnosticsSink : IDiagnosticsSink
{
    public static readonly IDiagnosticsSink Instance = new NullDiagnosticsSink();

    private NullDiagnosticsSink() { }

    public void LogInfo(string message) { }
    public void LogWarn(string message) { }
    public void LogError(string message, Exception? ex = null) { }
}