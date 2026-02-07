namespace Scsl.Unlocode.Core.Diagnostics;

public sealed record DiagnosticsRecord(
    DiagnosticsEventId EventId,
    string Message,
    DateTimeOffset Timestamp,
    DiagnosticsLevel Level,
    Exception? Exception = null,
    long? ElapsedMilliseconds = null)
{
    public static DiagnosticsRecord Info(
        DiagnosticsEventId eventId,
        string message) => new(eventId, message, DateTimeOffset.Now, DiagnosticsLevel.Info);

    public static DiagnosticsRecord Warning(
        DiagnosticsEventId eventId,
        string message) => new(eventId, message, DateTimeOffset.Now, DiagnosticsLevel.Warning);

    public static DiagnosticsRecord Error(
        DiagnosticsEventId eventId,
        string message, Exception? exception = null) =>
        new(eventId, message, DateTimeOffset.Now, DiagnosticsLevel.Error, exception);
};