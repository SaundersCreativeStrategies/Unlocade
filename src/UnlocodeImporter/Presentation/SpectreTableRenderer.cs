using System.Reflection;

using Spectre.Console;

using UnlocodeImporter.Presentation.Enums;
using UnlocodeImporter.Presentation.Options;

namespace UnlocodeImporter.Presentation;

public sealed class SpectreTableRenderer : ITableRenderer
{
    public void Render<T>(
        IEnumerable<T> items,
        string? title = null,
        TableStyle style = TableStyle.Default,
        TableRenderOptions? options = null,
        Func<PropertyInfo, bool>? propertyFilter = null,
        Func<PropertyInfo, string>? headerFormatter = null,
        Func<object?, string>? valueFormatter = null)
    {
        var renderOptions = options ?? TableRenderOptions.Default;

        var list = items.ToList();
        if (list.Count == 0)
        {
            AnsiConsole.WriteLine(
                style == TableStyle.MySql ? "Empty set." : "No data found.");
            return;
        }

        var properties = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => propertyFilter?.Invoke(p) ?? true)
            .ToArray();

        var table = new Table();

        ApplyStyle(table, style, title);

        foreach (var prop in properties)
        {
            table.AddColumn( new TableColumn(
                headerFormatter?.Invoke(prop) ?? prop.Name)
            {
                Alignment = Justify.Left,
                NoWrap = true
            });
        }

        foreach (var values in list.Select(item => properties.Select(p =>
                 {
                     var value = p.GetValue(item);
                     var raw =valueFormatter?.Invoke(value) ?? value?.ToString() ?? "NULL";
                     return Truncate(raw.Trim(), renderOptions);
                 }).ToArray()))
        {
            table.AddRow(values);
        }

        AnsiConsole.Write(table);

        if (style == TableStyle.MySql)
        {
            AnsiConsole.WriteLine(
                $"{list.Count} row{(list.Count == 1 ? "" : "s")} in set");
        }
    }

    private static string Truncate(string value, TableRenderOptions options)
    {
        if(options.MaxColumnWidth is null)
            return value;

        var max = options.MaxColumnWidth.Value;
        if (value.Length <= max)
            return value;

        return options.TruncateMode switch
        {
            TruncateMode.Strict => value[..max],
            TruncateMode.Friendly => FriendlyTruncate(value, max, options.TruncationSuffix),
            _ => value
        };
    }

    private static string FriendlyTruncate(string value, int max, string suffix)
    {
        if (max <= suffix.Length)
            return suffix[..max];

        var cut = max - suffix.Length;
        return $"{value[..cut]}{suffix}";
    }

    private static void ApplyStyle(Table table, TableStyle style, string? title = null)
    {
        switch (style)
        {
            case TableStyle.MySql:
                table.Border(TableBorder.Ascii2)
                    .BorderColor(Color.Grey);
                break;

            case TableStyle.Default:
            default:
                table
                    .Border(TableBorder.Rounded);
                if (!string.IsNullOrWhiteSpace(title))
                    table.Title(title);
                break;
        }
    }
}