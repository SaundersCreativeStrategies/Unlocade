using System.Data.OleDb;

using Scsl.Unlocode.Core.Diagnostics;

namespace Scsl.Unlocode.Infrastructure.Mdb.Factory;

internal static class OleDbConnectionFactory
{
    private const string Provider = "Microsoft.ACE.OLEDB.16.0";

    internal static OleDbConnection Create(string filePath, IDiagnosticsSink? diagnostics)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("MDB path cannot be null or empty", nameof(filePath));

        diagnostics?.LogInfo(DiagnosticsEvents.OleDbCreateStart, "Starting OLE DB connection creation");
        diagnostics?.LogInfo(DiagnosticsEvents.OleDbProviderSelected, $"OLE DB Provider: {Provider}");
        diagnostics?.LogInfo(DiagnosticsEvents.OleDbDataSourceSet, $"MDB Path: {filePath}");

        var connectionString = $"Provider={Provider};Data Source={filePath};Persist Security Info=False;";

        diagnostics?.LogInfo(DiagnosticsEvents.OleDbConnectionCreated, "OLE DB connection object created");

        return new OleDbConnection(connectionString);
    }
}