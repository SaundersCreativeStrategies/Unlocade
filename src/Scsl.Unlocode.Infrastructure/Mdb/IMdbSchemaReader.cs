using Scsl.Unlocode.Core.Metadata;

namespace Scsl.Unlocode.Infrastructure.Mdb;

public interface IMdbSchemaReader
{
    IReadOnlyList<MdbTableInfo> GetTables(string mdbPath);
}