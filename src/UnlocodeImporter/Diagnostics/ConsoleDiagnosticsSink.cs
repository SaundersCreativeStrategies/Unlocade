using System.Globalization;

using Scsl.Unlocode.Core.Diagnostics;

using Spectre.Console;

namespace UnlocodeImporter.Diagnostics;

public sealed class ConsoleDiagnosticsSink : IDiagnosticsSink
{
    private readonly bool _verbose;
    private readonly List<long> _operationTimings = new();

    public ConsoleDiagnosticsSink(bool verbose)
    {
        _verbose = verbose;
    }

    public void LogInfo(DiagnosticsEventId eventId, string message)
    {
        if (_verbose && eventId == DiagnosticsEvents.OperationRuntime)
        {
            if (TryExtractElapsedMilliseconds(message, out var ms))
            {
                _operationTimings.Add(ms);
            }
        }

        if (_verbose)
            Write(DiagnosticsRecord.Info(eventId, message));
    }

    public void LogWarn(DiagnosticsEventId eventId, string message)
    {
        if (_verbose)
            Write(DiagnosticsRecord.Warning(eventId, message));
    }

    public void LogError(DiagnosticsEventId eventId, string message, Exception? exception = null)
    {
        Write(DiagnosticsRecord.Error(eventId, message, exception));
    }

    public void WritePerformanceSummary()
    {
        if (!_verbose || _operationTimings.Count == 0)
            return;

        var total = _operationTimings.Sum();
        var max = _operationTimings.Max();
        var count = _operationTimings.Count;

        Write(DiagnosticsRecord.Info(
            DiagnosticsEvents.OperationRuntime,
            $"Performance Summary: {count} operations, total {total} ms, max {max} ms"));
    }

    private static void Write(DiagnosticsRecord record)
    {
        var output = SpectreDiagnosticsFormatter.Format(record);
        AnsiConsole.MarkupLine(output);
    }

    private static bool TryExtractElapsedMilliseconds(string message, out long elapsedMilliseconds)
    {
        // expected format:".... took {n} ms"
        elapsedMilliseconds = 0;

        var parts = message.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var index = Array.IndexOf(parts, "took");

        if (index >= 0 && index + 1 < parts.Length && long.TryParse(
                parts[index + 1], NumberStyles.Integer, CultureInfo.InvariantCulture, out var value))
        {
            elapsedMilliseconds = value;
            return true;
        }

        return false;
    }
}