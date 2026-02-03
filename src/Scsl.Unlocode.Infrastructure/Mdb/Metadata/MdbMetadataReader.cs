using System.Data;

using Scsl.Unlocode.Core.Diagnostics;
using Scsl.Unlocode.Core.Metadata;
using Scsl.Unlocode.Infrastructure.Mdb.Factory;

namespace Scsl.Unlocode.Infrastructure.Mdb.Metadata;

public sealed class MdbMetadataReader : IMdbMetadataReader
{
    private readonly IDiagnosticsSink? _diagnostics;

    public MdbMetadataReader(IDiagnosticsSink? diagnostics = null)
    {
        _diagnostics = diagnostics;
    }
    public IReadOnlyList<MdbTableInfo> GetTables(string mdbPath)
    {
        using var conn = OleDbConnectionFactory.Create(mdbPath, _diagnostics);

        _diagnostics?.LogInfo($"Opening OLE DB connection");
        conn.Open();

        _diagnostics?.LogInfo("Reading table schema");
        DataTable schema = conn.GetSchema("Tables");

        var result = new List<MdbTableInfo>();

        foreach (DataRow row in schema.Rows)
        {
            string? name = row["TABLE_NAME"].ToString();
            string? type = row["TABLE_TYPE"].ToString();

            if (type == "TABLE" && !name.StartsWith("MSys"))
            {
                result.Add(new MdbTableInfo()
                {
                    Name = name,
                    Type = type
                });
            }
        }

        _diagnostics?.LogInfo($"Tables found: {result.Count}");
        return result;
    }
}