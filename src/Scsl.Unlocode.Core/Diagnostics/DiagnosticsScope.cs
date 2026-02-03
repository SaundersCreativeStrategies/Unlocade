using System.Diagnostics;

namespace Scsl.Unlocode.Core.Diagnostics;

public class DiagnosticsScope : IDisposable
{
    private readonly IDiagnosticsSink _sink;
    private readonly DiagnosticsEventId _startEvent;
    private readonly DiagnosticsEventId _completedEvent;
    private readonly string _operation;
    private readonly Stopwatch _stopwatch;

    public DiagnosticsScope(
        IDiagnosticsSink sink, DiagnosticsEventId startEvent,
        DiagnosticsEventId completedEvent, string operation)
    {
        _sink = sink;
        _completedEvent = completedEvent;
        _startEvent = startEvent;
        _operation = operation;

        _sink.LogInfo(_startEvent, $"{operation} started.");
        _stopwatch = Stopwatch.StartNew();
    }

    public void Dispose()
    {
       _stopwatch.Stop();

       _sink.LogInfo(_completedEvent,
           $"{_operation} completed in {_stopwatch.ElapsedMilliseconds} ms.");
    }
}