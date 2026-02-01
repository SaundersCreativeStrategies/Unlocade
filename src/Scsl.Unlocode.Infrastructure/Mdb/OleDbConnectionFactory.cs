using System.Data.OleDb;

using Scsl.Unlocode.Infrastructure.Diagnostics;

namespace Scsl.Unlocode.Infrastructure.Mdb;

public static class OleDbConnectionFactory
{
    private const string Provider = "Microsoft.ACE.OLEDB.16.0";

    public static OleDbConnection Create(string filePath, IDiagnosticsSink? diagnostics)
    {
        if(string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("MDB path cannot be null or empty", nameof(filePath));

        var connectionString = $"Provider={Provider};Data Source={filePath};Persist Security Info=False;";

        // Verbose logging
        diagnostics?.LogInfo($"OLE DB Provdider: {Provider}");
        diagnostics?.LogInfo($"MDB Path: {filePath}");
        diagnostics?.LogInfo($"Creating OLE DB Connection");

        return new OleDbConnection(connectionString);
    }
}