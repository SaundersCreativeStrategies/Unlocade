using Scsl.Unlocode.Core.Abstractions;
using Scsl.Unlocode.Core.Metadata;
using Scsl.Unlocode.Core.Query;

namespace Scsl.Unlocode.Infrastructure.Mdb.Metadata;

public interface IMdbQueryExecutor
{
    IReadOnlyList<MdbQueryRow> ExecuteQuery(MdbQueryRequest request);
}