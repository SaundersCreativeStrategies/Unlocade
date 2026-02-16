namespace Scsl.Unlocode.Core.Abstractions;

public interface IDictionaryRow
{
    IReadOnlyDictionary<string, object?> Values { get; }
}