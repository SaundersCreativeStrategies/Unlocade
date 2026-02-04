using System.Reflection;

namespace Unlocode.DataImporter.Presentation;

public static class TableRendererDefaults
{
    public static bool SimpleTypesOnly(PropertyInfo prop)
    {
        return prop.PropertyType.IsPrimitive
               || prop.PropertyType == typeof(string)
               || prop.PropertyType == typeof(bool)
               || prop.PropertyType.IsValueType;
    }

    public static string HumanizeHeader(PropertyInfo prop)
    {
        return string.Concat(
            prop.Name.Select((c, i) =>
                i > 0 && char.IsUpper(c) ? $" {c}" : c.ToString()));
    }

    public static string MysqlHeader(PropertyInfo prop) => prop.Name;

    public static string MySqlValueFormatter(object? value)
    {
        if (value == null) return "NULL";
        if (value is string str) return $"'{str.Replace("'", "''")}'";
        if (value is bool b) return b ? "Yes" : "No";
        return value.ToString() ?? "NULL";
    }
}