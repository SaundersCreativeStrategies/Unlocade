namespace Scsl.Unlocode.Core.Diagnostics;

public sealed record DiagnosticsRecord(
    DateTimeOffset Timestamp,
    DiagnosticsLevel Level,
    DiagnosticsEventId EventId,
    string Message,
    Exception? Exception = null);