using System.Data.OleDb;
using System.Diagnostics.CodeAnalysis;

using Scsl.Unlocode.Core.Diagnostics;

namespace Scsl.Unlocode.Infrastructure.Mdb.Factory;

internal static class OleDbConnectionFactory
{
    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    internal static OleDbConnection Create(string filePath, IDiagnosticsSink? diagnostics)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("MDB path cannot be null or empty", nameof(filePath));

        try
        {
            var provider = AccessProviderDetector.SelectProvider(filePath);
            diagnostics?.LogInfo(DiagnosticsEvents.OleDbProviderSelected, $"Provider: {provider}");

            var connectionString = $"Provider={provider};Data Source={filePath};Persist Security Info=False;";
            var connection = new OleDbConnection(connectionString);

            diagnostics?.LogInfo(DiagnosticsEvents.OleDbConnectionCreated, $"Connection: {connectionString}");
            return connection;
        }
        catch (Exception ex)
        {
            diagnostics?.LogError(DiagnosticsEvents.OleDbCreateFailed, "Failed to create OLE DB connection", ex);
            throw;
        }
    }
}