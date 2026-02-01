using Scsl.Unlocode.Core.Metadata;

namespace Scsl.Unlocode.Infrastructure.Mdb;

public interface IMdbMetadataReader
{
    IReadOnlyList<MdbTableInfo> GetTables(string mdbPath);
}