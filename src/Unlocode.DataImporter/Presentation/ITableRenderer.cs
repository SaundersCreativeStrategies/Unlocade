using System.Reflection;

using Unlocode.DataImporter.Presentation.Enums;

namespace Unlocode.DataImporter.Presentation;

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