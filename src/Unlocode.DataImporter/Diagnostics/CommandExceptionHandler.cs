using System.Data.OleDb;

using Scsl.Unlocode.Core.Diagnostics;

namespace Unlocode.DataImporter.Diagnostics;

internal static class CommandExceptionHandler
{
    public static int Handle(Exception ex, IDiagnosticsSink diagnostics, bool verbose)
    {
        var (eventId, message, code) = ex switch
        {
            OleDbException =>
                (DiagnosticsEvents.DatabaseError,
                    "A database error occurred while accessing the MDB file.",
                    -2),

            UnauthorizedAccessException =>
                (DiagnosticsEvents.AccessDenied,
                    "Access to the MDB file was denied.",
                    -3),

            InvalidOperationException =>
                (DiagnosticsEvents.CommandFailed,
                    "the operation cound not be completed.",
                    -1),

            _ =>
                (DiagnosticsEvents.CommandFailed,
                    "Command execution failed.",
                    -99)
        };

        diagnostics.LogError(eventId, message, verbose ? ex : null);

        if (!verbose)
        {
            diagnostics.LogInfo(
                DiagnosticsEvents.VerboseHint,
                "Re-run the command with --verbose to see details.");
        }

        return code;
    }
}