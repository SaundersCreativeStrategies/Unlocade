namespace Scsl.Unlocode.Core.Diagnostics;

public sealed record DiagnosticsRecord(
    DateTimeOffset Timestamp,
    DiagnosticsLevel Level,
    DiagnosticsEventId EventId,
    string Message,
    Exception? Exception = null)
{
    public static DiagnosticsRecord Info(
        DiagnosticsEventId eventId,
        string message) => new(DateTimeOffset.Now, DiagnosticsLevel.Info, eventId, message);

    public static DiagnosticsRecord Warning(
        DiagnosticsEventId eventId,
        string message) => new(DateTimeOffset.Now, DiagnosticsLevel.Warning, eventId, message);

    public static DiagnosticsRecord Error(
        DiagnosticsEventId eventId,
        string message, Exception? exception = null) => new(DateTimeOffset.Now, DiagnosticsLevel.Error, eventId,
        message, exception);
};