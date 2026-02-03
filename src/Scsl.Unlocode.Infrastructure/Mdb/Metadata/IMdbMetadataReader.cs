using Scsl.Unlocode.Core.Metadata;

namespace Scsl.Unlocode.Infrastructure.Mdb.Metadata;

public interface IMdbMetadataReader
{
    IReadOnlyList<MdbTableInfo> GetTables(string mdbPath);
    IReadOnlyList<MdbColumnInfo> GetTableSchema(string mdbPath, string table);
}