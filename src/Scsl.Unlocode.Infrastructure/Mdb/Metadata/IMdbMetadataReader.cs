using Scsl.Unlocode.Core.Metadata;

namespace Scsl.Unlocode.Infrastructure.Mdb.Metadata;

public interface IMdbMetadataReader
{
    IReadOnlyList<MdbTableInfo> GetTables(string mdbPath);
}