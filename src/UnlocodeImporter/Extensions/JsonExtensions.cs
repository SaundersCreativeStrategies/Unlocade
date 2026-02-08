using System.Text.Json;

namespace UnlocodeImporter.Extensions;

public static class JsonExtensions
{
    public static IReadOnlyList<T> ParseJson<T>(this string json)
    {
        if (string.IsNullOrWhiteSpace(json)) return [];

        return JsonSerializer.Deserialize<List<T>>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? [];
    }
}