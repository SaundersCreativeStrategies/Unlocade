using System.Data.OleDb;
using System.Diagnostics.CodeAnalysis;

using Scsl.Unlocode.Core.Diagnostics;

namespace Scsl.Unlocode.Infrastructure.Mdb.Factory;

internal static class OleDbConnectionFactory
{
    private const string Provider = "Microsoft.ACE.OLEDB.16.0";

    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    internal static OleDbConnection Create(string filePath, IDiagnosticsSink? diagnostics)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("MDB path cannot be null or empty", nameof(filePath));

        var sink = diagnostics ?? NullDiagnosticsSink.Instance;
        try
        {
            using var scope = new DiagnosticsScope(
                sink,
                DiagnosticsEvents.OleDbCreateStart,
                DiagnosticsEvents.OleDbCreateCompleted,
                "OLE DB connection creation");

            sink.LogInfo(DiagnosticsEvents.OleDbCreateStart, "Starting OLE DB connection creation");
            sink.LogInfo(DiagnosticsEvents.OleDbProviderSelected, $"OLE DB Provider selected: {Provider}");
            sink.LogInfo(DiagnosticsEvents.OleDbDataSourceSet, $"OLE DB data source set to {filePath}");

            var connectionString = $"Provider={Provider};Data Source={filePath};Persist Security Info=False;";

            var connection = new OleDbConnection(connectionString);

            sink.LogInfo(DiagnosticsEvents.OleDbConnectionCreated, "OLE DB connection object created");
            return connection;
        }
        catch (Exception ex)
        {
            sink.LogError(DiagnosticsEvents.OleDbCreateFailed, "Failed to create OLE DB connection", ex);
            throw;
        }
    }
}