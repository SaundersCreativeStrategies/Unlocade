using System.Reflection;

using Spectre.Console;

namespace Unlocode.DataImporter.Presentation;

public sealed class SpectreTableRenderer : ITableRenderer
{
    public void Render<T>(
        IEnumerable<T> items,
        string? title = null,
        TableStyle style = TableStyle.Default,
        Func<PropertyInfo, bool>? propertyFilter = null,
        Func<PropertyInfo, string>? headerFormatter = null,
        Func<object?, string>? valueFormatter = null)
    {
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
                Alignment = Justify.Left
            });
        }

        foreach (var values in list.Select(item => properties.Select(p =>
                 {
                     var value = p.GetValue(item);
                     return valueFormatter?.Invoke(value) ?? value?.ToString() ?? "NULL";
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

    private static void ApplyStyle(Table table, TableStyle style, string? title = null)
    {
        switch (style)
        {
            case TableStyle.MySql:
                table.Border(TableBorder.Ascii2)
                    .BorderColor(Color.Grey)
                    .Expand();
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