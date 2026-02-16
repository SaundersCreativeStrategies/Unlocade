using Scsl.Unlocode.Core.Abstractions;

namespace Scsl.Unlocode.Core.Query;

public sealed class MdbQueryRow : IDictionaryRow
{
    public IReadOnlyDictionary<string, object?> Values { get; }

    public MdbQueryRow(IDictionary<string, object?> values)
    {
        Values = new Dictionary<string, object?>(values);
    }
}