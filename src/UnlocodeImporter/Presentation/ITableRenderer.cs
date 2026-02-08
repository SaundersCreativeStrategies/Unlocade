using System.Reflection;

using UnlocodeImporter.Presentation.Enums;
using UnlocodeImporter.Presentation.Options;

namespace UnlocodeImporter.Presentation;

public interface ITableRenderer
{
    void Render<T>(
        IEnumerable<T> items,
        string? title = null,
        TableStyle style = TableStyle.Default,
        TableRenderOptions? options = null,
        Func<PropertyInfo, bool>? propertyFilter = null,
        Func<PropertyInfo, string>? headerFormatter = null,
        Func<object?, string>? valueFormatter = null);
}